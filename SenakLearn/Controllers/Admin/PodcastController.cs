using System;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SenakLearn.Models;
using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using System.Threading.Tasks;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace SenakLearn.Controllers.Admin
{
    public class PodcastController : SenakLearn.Controllers.BaseAdminController
    {
        private SWEntities _db = new SWEntities();
        public ActionResult Index()
        {
            //  var learnCours = _db.learn_cours.Include(l => l.learn_cours_group).Include(l => l.learn_teacher);
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            grid.SortOrder = "dsc";
            grid.SortColumn = "id";
            var list = Biz.PodcastBiz.Instance.GetAllPagedList(grid);
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

        public static GridColumnModelList<learn_cours> PodcastColumns { get; private set; } = GetPodcastColumns();
        public static GridColumnModelList<learn_cours> GetPodcastColumns()
        {
            if (PodcastColumns == null)
            {
                PodcastColumns = new GridColumnModelList<learn_cours>();
                PodcastColumns.Add(x => x.id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                PodcastColumns.Add(x => x.InterviewPathVideo).SetCaption("عملیات").SetWidth("270").SetSortable(false).SetSearchable(false);
                PodcastColumns.Add(x => x.image).SetCaption("استاد").SetWidth("70").SetSortable(false).SetSearchable(false);
                PodcastColumns.Add(x => x.doc2).SetCaption("گروه").SetWidth("70").SetSortable(false).SetSearchable(false);
                PodcastColumns.Add(x => x.name).SetCaption("نام ").SetWidth("200");
                PodcastColumns.Add(x => x.time).SetCaption("مدت ").SetWidth("70");
                PodcastColumns.Add(x => x.status).SetCaption("وضعیت").SetWidth("70");
                //PodcastColumns.Add(x => x.num_present).SetCaption("تعداد جلسات").SetWidth("50");
                PodcastColumns.Add(x => x.Monetary).SetCaption("هزینه(ریال) ").SetWidth("100");
                //PodcastColumns.Add(x => x.IsFavoriteS).SetCaption("نمایش صفحه اصلی").SetWidth("100");
                PodcastColumns.Add(x => x.doc).SetCaption("توضیحات ").SetWidth("200");
            }
            return PodcastColumns;
        }

        public ActionResult Create()
        {
            return View(new SenakLearn.Models.learn_cours());
        }

        [HttpPost]
        // 
        public async Task<ActionResult> Create(learn_cours learnCours, HttpPostedFileBase ImageFile)
        {
            learnCours.num_present = 1;
            learnCours.TypeCours = 2;//نوع تعریف پادکست است
            if (ImageFile == null)
            {
                //throw new Exception("");
                ModelState.AddModelError("image", "انتخاب تصویر اجباری است !");
                return View(learnCours);
            }
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
        [HttpPost]
        // 
        public ActionResult Edit(learn_cours learnCours, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learnCours.TypeCours = 2;
                learnCours.num_present = 1;
                learnCours.image = EditFile(ImageFile, pathFile.cours, learnCours.image);

                _db.Entry(learnCours).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(learnCours);
        }

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
    }
}