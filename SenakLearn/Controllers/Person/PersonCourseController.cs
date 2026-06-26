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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Person
{
    public class PersonCourseController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = PersonCourseBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<Person_Course> PersonCourseColumns { get; private set; } = GetColumns();
        public static GridColumnModelList<Person_Course> GetColumns()
        {
            if (PersonCourseColumns == null)
            {
                PersonCourseColumns = new GridColumnModelList<Person_Course>();
                PersonCourseColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                PersonCourseColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                PersonCourseColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
                PersonCourseColumns.Add(x => x.CourseLeader).SetCaption("مجری دوره").SetWidth("300");
                //PersonCourseColumns.Add(x => x.InOut).SetCaption("نوع دوره").SetWidth("50");
                PersonCourseColumns.Add(x => x.Code).SetCaption("کد دوره").SetWidth("120");
                PersonCourseColumns.Add(x => x.Duration).SetCaption("مدت دوره").SetWidth("120");
                PersonCourseColumns.Add(x => x.FromDate).SetCaption("شروع دوره").SetWidth("120");
                PersonCourseColumns.Add(x => x.ToDate).SetCaption("پایان دوره").SetWidth("120");
                PersonCourseColumns.Add(x => x.Description).SetCaption("توضیحات دوره").SetWidth("300");
            }
            return PersonCourseColumns;
        }
        public async Task<ActionResult> GetReportExcel()
        {

            var memoryStream =await PersonCourseBiz.Instance.GetAllReportExcel();

            FileContentResult result = new FileContentResult(memoryStream.ToArray(), "application/vnd.ms-excel")
            {
                FileDownloadName = "Person_Course_" + DateTime.Now.ToShortDateString() + ".xlsx"
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
            var PersonCourse = PersonCourseBiz.Instance.Get(id.Value);
            if (PersonCourse == null)
            {
                return HttpNotFound();
            }
            return View(PersonCourse);
        }

        // GET: Roles/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View(new Person_Course());
            }
            var PersonCourse = PersonCourseBiz.Instance.Get(id.Value);
            if (PersonCourse == null)
            {
                return View(new Person_Course());
            }
            return View(PersonCourse);
        }

        // PersonCourse: Roles/Create
        // To protect from overPersonCourseing attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person_Course PersonCourse)
        {
            var ImageFile = this.Request.Files[0];
            if ((ImageFile!=null && ImageFile.ContentLength>0))
                PersonCourse.CertificateFile = SaveFile(ImageFile, pathFile.PersonCertificate);
            if (PersonCourse.Id != 0)
            {
                var personInDb = PersonCourseBiz.Instance.Get(PersonCourse.Id);
                if (personInDb != null && !string.IsNullOrEmpty(personInDb.CertificateFile) && string.IsNullOrEmpty(PersonCourse.CertificateFile))
                    PersonCourse.CertificateFile = personInDb.CertificateFile;

            }
            PersonCourseBiz.Instance.Save(PersonCourse);
            return RedirectToAction("Index", "PersonCourse");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PersonCourse = PersonCourseBiz.Instance.Get(id.Value);
            if (PersonCourse == null)
            {
                return HttpNotFound();
            }
            return View(PersonCourse);
        }

        // PersonCourse: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonCourseBiz.Instance.Remove(id);
            return RedirectToAction("Index", "PersonCourse");
        }
    }
}