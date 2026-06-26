using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SenakLearn.Models;
using System.Data.Entity;
using SenakLearn.JqGrid;
using System.Threading.Tasks;
using MVC.Controls.Grid;

namespace SenakLearn.Controllers.Admin
{
    public class TeacherController : SenakLearn.Controllers.BaseAdminController
    {
        private SWEntities db = new SWEntities();

        // GET: Admin/Teacher
        public ActionResult Index()
        {
            return View(db.learn_teacher.ToList());
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.TeacherBiz.Instance.GetAllPagedList(grid);
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

        public static GridColumnModelList<learn_teacher> TeacherColumns { get; private set; } = GetTeacherColumns();
        public static GridColumnModelList<learn_teacher> GetTeacherColumns()
        {
            if (TeacherColumns == null)
            {
                TeacherColumns = new GridColumnModelList<learn_teacher>();
                TeacherColumns.Add(x => x.id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                TeacherColumns.Add(x => x.Resume).SetCaption("عملیات").SetWidth("50").SetSortable(false).SetSearchable(false);
                TeacherColumns.Add(x => x.FullName).SetCaption("نام ").SetWidth("150");
                TeacherColumns.Add(x => x.UserName).SetCaption("نام کاربری ").SetWidth("150");
                TeacherColumns.Add(x => x.email).SetCaption("ایمیل ").SetWidth("180");
                TeacherColumns.Add(x => x.status).SetCaption("وضعیت").SetWidth("50");
                TeacherColumns.Add(x => x.meli).SetCaption("کد ملی").SetWidth("80");
                TeacherColumns.Add(x => x.tel).SetCaption("تلفن ").SetWidth("80");
                TeacherColumns.Add(x => x.mobile).SetCaption("همراه ").SetWidth("80");
                TeacherColumns.Add(x => x.code).SetCaption("کد استاد ").SetWidth("80");
                TeacherColumns.Add(x => x.education).SetCaption("مدرک تحصیلی ").SetWidth("80");
                TeacherColumns.Add(x => x.IsFavoriteS).SetCaption("نمایش در صفحه اصلی سایت").SetWidth("50");
            }
            return TeacherColumns;
        }

        // GET: Admin/Teacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_teacher learn_teacher = db.learn_teacher.Find(id);
            if (learn_teacher == null)
            {
                return HttpNotFound();
            }
            return View(learn_teacher);
        }

        // GET: Admin/Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // 
        public ActionResult Create(learn_teacher learn_teacher, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learn_teacher.image = SaveFile(ImageFile, pathFile.teacher);
                learn_teacher.date_register = DateTime.Now;
                learn_teacher.status = true;
                db.learn_teacher.Add(learn_teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(learn_teacher);
        }

        // GET: Admin/Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_teacher learn_teacher = db.learn_teacher.Find(id);
            if (learn_teacher == null)
            {
                return HttpNotFound();
            }
            return View(learn_teacher);
        }

        // POST: Admin/Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // 
        public ActionResult Edit(learn_teacher learn_teacher, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learn_teacher.image = EditFile(ImageFile, pathFile.teacher, learn_teacher.image);
                db.Entry(learn_teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(learn_teacher);
        }

        // GET: Admin/Teacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_teacher learn_teacher = db.learn_teacher.Find(id);
            if (learn_teacher == null)
            {
                return HttpNotFound();
            }
            return View(learn_teacher);
        }

        // POST: Admin/Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        // 
        public ActionResult DeleteConfirmed(int id)
        {
            learn_teacher learn_teacher = db.learn_teacher.Find(id);
            if (learn_teacher == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (learn_teacher.image != null && learn_teacher.image != "no-photo.jpg")
                    if (System.IO.File.Exists(Server.MapPath("/images/teacher/" + learn_teacher.image)))
                        System.IO.File.Delete(Server.MapPath("/images/teacher/" + learn_teacher.image));
            }

            db.learn_teacher.Remove(learn_teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
