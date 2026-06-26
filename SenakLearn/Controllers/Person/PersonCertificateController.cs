using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.Biz.Person;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using SenakLearn.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Person
{
    public class PersonCertificateController : BaseAdminController
    {
        public ActionResult Index(int userId)
        {
            return View(new Person_Certificate { UserId = userId });
        }
        public ActionResult IndexPartial(int userId)
        {
            return PartialView(new Person_Certificate { UserId = userId });
        }
        public ActionResult LoadList(GridSettings grid, int userId)
        {
            var list = PersonCertificateBiz.Instance.GetAllPagedList(grid, userId);
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
        public static GridColumnModelList<Person_Certificate> PersonCertificateColumns { get; private set; } = GetColumns();
        public static GridColumnModelList<Person_Certificate> GetColumns()
        {
            if (PersonCertificateColumns == null)
            {
                PersonCertificateColumns = new GridColumnModelList<Person_Certificate>();
                PersonCertificateColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                PersonCertificateColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                PersonCertificateColumns.Add(x => x.Code).SetCaption("کد مدرک").SetWidth("100");
                PersonCertificateColumns.Add(x => x.IssueDate).SetCaption("تاریخ صدور").SetWidth("100");
                PersonCertificateColumns.Add(x => x.InOutTitle).SetCaption("نوع دوره").SetWidth("50");
                PersonCertificateColumns.Add(x => x.TypeCourse).SetHidden(true);
                PersonCertificateColumns.Add(x => x.Person_Course).SetCaption("نام دوره").SetWidth("200");
                PersonCertificateColumns.Add(x => x.Duration).SetCaption("مدت دوره").SetWidth("100");
                PersonCertificateColumns.Add(x => x.CourseLeader).SetCaption("مجری دوره").SetWidth("200");
                PersonCertificateColumns.Add(x => x.Person_Teacher).SetCaption("نام استاد").SetWidth("200");
                PersonCertificateColumns.Add(x => x.UrlCertificate).SetCaption("تصویر مدرک").SetWidth("200");
            }
            return PersonCertificateColumns;
        }

        public static GridColumnModelList<PersonCertificateReportSearach> PersonCertificateColumnReports { get; private set; } = GetColumnReports();
        public static GridColumnModelList<PersonCertificateReportSearach> GetColumnReports()
        {
            if (PersonCertificateColumnReports == null)
            {
                PersonCertificateColumnReports = new GridColumnModelList<PersonCertificateReportSearach>();
                PersonCertificateColumnReports.Add(x => x.PersonCertificateId).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                PersonCertificateColumnReports.Add(x => x.act).SetCaption("").SetWidth("50");
                PersonCertificateColumnReports.Add(x => x.Code).SetCaption("کد مدرک").SetWidth("130");
                PersonCertificateColumnReports.Add(x => x.Person_Course).SetCaption("نام دوره").SetWidth("200");
                PersonCertificateColumnReports.Add(x => x.CourseLeader).SetCaption("مجری دوره").SetWidth("150");
                PersonCertificateColumnReports.Add(x => x.IssueDatePersian).SetCaption("تاریخ صدور").SetWidth("100");
                PersonCertificateColumnReports.Add(x => x.InOut).SetCaption("نوع دوره").SetWidth("50");
                PersonCertificateColumnReports.Add(x => x.TeacherName).SetCaption("نام استاد").SetWidth("100");
                PersonCertificateColumnReports.Add(x => x.Duration).SetCaption("مدت دوره").SetWidth("100");
                PersonCertificateColumnReports.Add(x => x.PersonName).SetCaption("پرسنل").SetWidth("200");
                PersonCertificateColumnReports.Add(x => x.PersonCode).SetCaption("کد پرسنل").SetWidth("100");
                PersonCertificateColumnReports.Add(x => x.PersonOrg).SetCaption("سازمان پرسنل").SetWidth("200");
            }
            return PersonCertificateColumnReports;
        }
        public ActionResult Report(PersonCertificateReportSearach search)
        {
            if (search == null) search = new PersonCertificateReportSearach();
            ViewBag.CoursList = PersonCourseBiz.Instance.DropDownAll();
            ViewBag.CourseLeader = PersonCourseBiz.Instance.DropDownLeaders();
            ViewBag.Persons = UserBiz.Instance.DropDownUsers();
            ViewBag.Orgs = OrgBiz.Instance.DropDown(true);
            //var accesMenu = ViewData["AccessMenu"];
            SetViewBagMenu(Current_learn_user);
            return View(search);
        }
        public ActionResult LoadListReport(GridSettings grid, PersonCertificateReportSearach search)
        {
            var list = PersonCertificateBiz.Instance.GetAllPagedListReport(grid, search);
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
        public ActionResult GetReportExcel(PersonCertificateReportSearach search)
        {
            
            var memoryStream = PersonCertificateBiz.Instance.GetAllReportExcel( search);

            FileContentResult result = new FileContentResult(memoryStream.ToArray(), "application/vnd.ms-excel")
            {
                FileDownloadName = "Person_Certificate_"+DateTime.Now.ToShortDateString()+".xlsx"
            };
            return result;
        }
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PersonCertificate = PersonCertificateBiz.Instance.Get(id.Value);
            if (PersonCertificate == null)
            {
                return HttpNotFound();
            }
            return View(PersonCertificate);
        }

        // GET: Roles/Create
        public ActionResult Create(int userId)
        {
            var user = UserBiz.Instance.Find(userId);
            if (user == null)
            {
                return HttpNotFound("کاربر یافت نشد !");
            }
            var listCourse = PersonCourseBiz.Instance.DropDown();
            ViewBag.CourseList = listCourse;
            var listTeacher = PersonTeacherBiz.Instance.DropDown();
            ViewBag.TeacherList = listTeacher;
            return View(new Person_Certificate() { UserId = userId, UserName = user.Name + " " + user.Family + " : " + user.user_name });

        }
        public ActionResult Edit(int id)
        {
            var PersonCertificate = PersonCertificateBiz.Instance.Get(id);
            if (PersonCertificate == null)
            {
                return HttpNotFound();
            }
            var listCourse = PersonCourseBiz.Instance.DropDown();
            ViewBag.CourseList = listCourse;
            var listTeacher = PersonTeacherBiz.Instance.DropDown();
            ViewBag.TeacherList = listTeacher;
            var user = UserBiz.Instance.Find(PersonCertificate.UserId);
            PersonCertificate.UserName = user.Name + " " + user.Family + " : " + user.user_name;
            return View("~/Views/PersonCertificate/Create.cshtml", PersonCertificate);
        }

        // PersonCertificate: Roles/Create
        // To protect from overPersonCertificateing attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person_Certificate PersonCertificate)
        {
            HttpPostedFileBase file = Request.Files[0];
            if (file == null)
            { return HttpNotFound("فایل مدرک را انتخاب نمایید !"); }
            if (PersonCertificate.Id != 0)
            {
                var PersonCertificate_Db = PersonCertificateBiz.Instance.Get(PersonCertificate.Id);
                if (PersonCertificate_Db == null)
                { return Json(new { status = "NOK" }, JsonRequestBehavior.AllowGet); }
                RemoveFile(PersonCertificate_Db.UrlCertificate, pathFile.PersonCertificate);
            }
            PersonCertificate.UrlCertificate = SaveFile(file, pathFile.PersonCertificate);
            var result = PersonCertificateBiz.Instance.Save(PersonCertificate);
            if (result)
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = "NOK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PersonCertificate = PersonCertificateBiz.Instance.Get(id.Value);
            if (PersonCertificate == null)
            {
                return HttpNotFound();
            }
            return View(PersonCertificate);
        }

        // PersonCertificate: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = PersonCertificateBiz.Instance.Get(id);
            var result = RemoveFile(item.UrlCertificate, pathFile.PersonCertificate);
            //if (result == false)
            //    return HttpNotFound("فایل مدرک یافت نشد!");
            PersonCertificateBiz.Instance.Remove(id);
            return RedirectToAction("Index", "PersonCertificate", new { userId = item.UserId });
        }
    }
}