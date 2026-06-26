using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.Biz.Person;
using SenakLearn.JqGrid;
using SenakLearn.Models.Common;
using SenakLearn.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Person
{
    public class PersonTeacherController : BaseAdminController
    {
        public ActionResult Index()
        {
            var listCertificate = EntityMasterDataBiz.Instance.DropDown(1);
            ViewBag.CertificateList = listCertificate;
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = PersonTeacherBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                RoleData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Person_Teacher> PersonTeacherColumns { get; private set; } = GetColumns();
        public static GridColumnModelList<Person_Teacher> GetColumns()
        {
            if (PersonTeacherColumns == null)
            {
                PersonTeacherColumns = new GridColumnModelList<Person_Teacher>();
                PersonTeacherColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                PersonTeacherColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                PersonTeacherColumns.Add(x => x.TeacherName).SetCaption("نام استاد").SetWidth("300");
                PersonTeacherColumns.Add(x => x.Mobile).SetCaption("همراه").SetWidth("130");
                PersonTeacherColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("130");
                PersonTeacherColumns.Add(x => x.CertificateName).SetCaption("مدرک تحصیلی").SetWidth("130");
                PersonTeacherColumns.Add(x => x.Expertise).SetCaption("تخصص").SetWidth("130");
            }
            return PersonTeacherColumns;
        }
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PersonTeacher = PersonTeacherBiz.Instance.Get(id.Value);
            if (PersonTeacher == null)
            {
                return HttpNotFound();
            }
            return View(PersonTeacher);
        }

        // GET: Roles/Create
        public ActionResult Create(int? id)
        {
            var listCertificate = EntityMasterDataBiz.Instance.DropDown(1);
            ViewBag.CertificateList = listCertificate;
            if (id == null)
            {
                return View(new Person_Teacher());
            }
            var PersonTeacher = PersonTeacherBiz.Instance.Get(id.Value);

            if (PersonTeacher == null)
            {
                return View(new Person_Teacher());
            }
            return View(PersonTeacher);
        }

        // PersonTeacher: Roles/Create
        // To protect from overPersonTeachering attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person_Teacher PersonTeacher)
        {

            PersonTeacherBiz.Instance.Save(PersonTeacher);
            var listCertificate = EntityMasterDataBiz.Instance.DropDown(1);
            ViewBag.CertificateList = listCertificate;
            return RedirectToAction("Index", "PersonTeacher");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PersonTeacher = PersonTeacherBiz.Instance.Get(id.Value);
            if (PersonTeacher == null)
            {
                return HttpNotFound();
            }
            return View(PersonTeacher);
        }

        // PersonTeacher: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonTeacherBiz.Instance.Remove(id);
            var listCertificate = EntityMasterDataBiz.Instance.DropDown(1);
            ViewBag.CertificateList = listCertificate;
            return RedirectToAction("Index", "PersonTeacher");
        }
    }
}