using System;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SenakLearn.Models;
using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using System.Threading.Tasks;

namespace SenakLearn.Controllers.Admin
{
    public class CoursController : SenakLearn.Controllers.BaseAdminController
    {
        private SWEntities _db = new SWEntities();

        // GET: Admin/Cours
        public ActionResult Index()
        {
            //  var learnCours = _db.learn_cours.Include(l => l.learn_cours_group).Include(l => l.learn_teacher);
            return View();
        }

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.CourseBiz.Instance.GetAllPagedList(grid);
            Parallel.ForEach(list, x => { x.image = x.learn_teacher?.FullName; x.doc2 = x.learn_cours_group.name; x.learn_teacher = null; });
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                UserData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<learn_cours> CourseColumns { get; private set; } = GetCourseColumns();
        public static GridColumnModelList<learn_cours> GetCourseColumns()
        {
            if (CourseColumns == null)
            {
                CourseColumns = new GridColumnModelList<learn_cours>();
                CourseColumns.Add(x => x.id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                CourseColumns.Add(x => x.InterviewPathVideo).SetCaption("عملیات").SetWidth("50").SetSortable(false).SetSearchable(false);
                CourseColumns.Add(x => x.image).SetCaption("استاد").SetWidth("70").SetSortable(false).SetSearchable(false);
                CourseColumns.Add(x => x.doc2).SetCaption("گروه").SetWidth("70").SetSortable(false).SetSearchable(false);
                CourseColumns.Add(x => x.name).SetCaption("نام ").SetWidth("150");
                CourseColumns.Add(x => x.time).SetCaption("مدت ").SetWidth("50");
                CourseColumns.Add(x => x.status).SetCaption("وضعیت").SetWidth("50");
                CourseColumns.Add(x => x.num_present).SetCaption("تعداد جلسات").SetWidth("50");
                CourseColumns.Add(x => x.Monetary).SetCaption("هزینه(ریال) ").SetWidth("100");
                CourseColumns.Add(x => x.IsFavoriteS).SetCaption("نمایش در صفحه اصلی سایت").SetWidth("50");
                CourseColumns.Add(x => x.doc).SetCaption("توضیحات ").SetWidth("250");
            }
            return CourseColumns;
        }

        // GET: Admin/Cours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_cours learnCours = _db.learn_cours.Find(id);
            if (learnCours == null)
            {
                return HttpNotFound();
            }
            return View(learnCours);
        }

        // GET: Admin/Cours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // 
        public async Task<ActionResult> Create(learn_cours learnCours, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learnCours.image = SaveFile(ImageFile, pathFile.cours);
                _db.learn_cours.Add(learnCours);
                await _db.SaveChangesAsync();
                await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Offline, learnCours.id_group);
                if (learnCours.id_teacher != null)
                    await Biz.TeacherBiz.Instance.UpdateCourseCount(learnCours.id_teacher.Value);
                return RedirectToAction("Index");
            }

            return View(learnCours);
        }

        // GET: Admin/Cours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_cours learnCours = _db.learn_cours.Find(id);
            if (learnCours == null)
            {
                return HttpNotFound();
            }
            return View(learnCours);
        }

        // POST: Admin/Cours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // 
        public ActionResult Edit(learn_cours learnCours, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learnCours.image = EditFile(ImageFile, pathFile.cours, learnCours.image);

                _db.Entry(learnCours).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(learnCours);
        }

        // GET: Admin/Cours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_cours learnCours = _db.learn_cours.Find(id);
            if (learnCours == null)
            {
                return HttpNotFound();
            }
            return View(learnCours);
        }

        // POST: Admin/Cours/Delete/5
        [HttpPost, ActionName("Delete")]
        //    
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var learnCours = _db.learn_cours.Find(id);
            if (learnCours == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (learnCours.image != null && learnCours.image != "no-photo.jpg")
                    if (System.IO.File.Exists(Server.MapPath("/images/cours/" + learnCours.image)))
                        System.IO.File.Delete(Server.MapPath("/images/cours/" + learnCours.image));
            }
            int groupId = learnCours.id_group;
            var teacherId = learnCours.id_teacher;
            _db.learn_cours.Remove(learnCours);
            await _db.SaveChangesAsync();
            await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Offline, groupId, false);
            if (teacherId != null)
                await Biz.TeacherBiz.Instance.UpdateCourseCount(learnCours.id_teacher.Value, false);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
