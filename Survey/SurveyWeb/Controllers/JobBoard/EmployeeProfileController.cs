using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.JobBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers.JobBoard
{
    public class EmployeeProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            var item = EmployeeProfileBiz.Instance.FindByUserId(Current_UserId);
            return View("~/Views/JobBoard/EmployeeProfile/Index.cshtml", item);
        }

        public ActionResult Edit()
        {
            var item = EmployeeProfileBiz.Instance.FindByUserId(Current_UserId);
            return View("~/Views/JobBoard/EmployeeProfile/Details.cshtml", item);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeProfile item, HttpPostedFileBase img, HttpPostedFileBase resume)
        {
            item.UserID = Current_UserId;
            if (img != null && img.ContentLength > 0)
            {
                var type = img.ContentType.ToLower();
                if (!type.Contains("image"))
                {
                    throw new Exception("فایل تصویر معتبر نیست ");
                }
                item.ProfileImageURI = SaveFile(img, pathFile.UserProfileImage, item.UserID.ToString());
            }

            if (resume != null && resume.ContentLength > 0)
            {
                var fileByte = new byte[resume.ContentLength];
                resume.InputStream.Read(fileByte, 0, (int)resume.InputStream.Length);
                item.ResumeFile = fileByte;
            }

            EmployeeProfileBiz.Instance.Add(item);
            return RedirectToAction("Index");
        }

        public ActionResult WorkExperience()
        {
            ViewBag.currentUser = Current_UserId;
            return View("~/Views/JobBoard/EmployeeProfile/WorkExperience/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AddWorkExperience(WorkExperience item)
        {
            item.UserID = Current_UserId;
            EmployeeProfileBiz.Instance.AddWorkExperience(item);
            ViewBag.currentUser = Current_UserId;
            return PartialView("~/Views/JobBoard/EmployeeProfile/WorkExperience/List.cshtml");
        }

        public ActionResult RemoveWorkExperience(int id)
        {
            ViewBag.currentUser = Current_UserId;
            EmployeeProfileBiz.Instance.RemoveWorkExperience(id);
            return PartialView("~/Views/JobBoard/EmployeeProfile/WorkExperience/List.cshtml");
        }

        public ActionResult EducationalBackground()
        {
            ViewBag.currentUser = Current_UserId;
            return View("~/Views/JobBoard/EmployeeProfile/EducationalBackground/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AddEducationalBackground(EducationalBackground item)
        {
            item.UserID = Current_UserId;
            EmployeeProfileBiz.Instance.AddEducationalBackground(item);
            ViewBag.currentUser = Current_UserId;
            return PartialView("~/Views/JobBoard/EmployeeProfile/EducationalBackground/List.cshtml");
        }

        public ActionResult RemoveEducationalBackground(int id)
        {
            ViewBag.currentUser = Current_UserId;
            EmployeeProfileBiz.Instance.RemoveEducationalBackground(id);
            return PartialView("~/Views/JobBoard/EmployeeProfile/EducationalBackground/List.cshtml");
        }

        public ActionResult DownloadResume(int id)
        {
            var item = EmployeeProfileBiz.Instance.Find(id);

            if (item.ResumeFile != null && item.ResumeFile.Length > 0)
            {
                return File(item.ResumeFile, "application/pdf");
            }

            return null;
        }

        public ActionResult ShowAll()
        {
            return View("~/Views/JobBoard/EmployeeProfileDisplay/Index.cshtml");
        }

        public ActionResult Find(int id)
        {
            var item = EmployeeProfileBiz.Instance.Find(id);
            return PartialView("~/Views/JobBoard/EmployeeProfileDisplay/Details.cshtml", item);
        }

        public ActionResult LoadVerifiedList(GridSettings grid)
        {
            var list = EmployeeProfileBiz.Instance.GetAllVerifiedPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                NewsData = "Null"
            },
                      JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<EmployeeProfile> EmployeeProfileColumns { get; private set; } = GetEmployeeProfileColumns();
        public static GridColumnModelList<EmployeeProfile> GetEmployeeProfileColumns()
        {
            if (EmployeeProfileColumns == null)
            {
                EmployeeProfileColumns = new GridColumnModelList<EmployeeProfile>();
                EmployeeProfileColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                EmployeeProfileColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                EmployeeProfileColumns.Add(x => x.Username).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.MilitaryStatusTitle).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.Phone).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.Specialty).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.ProvinceOfResidence).SetWidth("200");
            }
            return EmployeeProfileColumns;
        }

        public ActionResult LoadWorkExperienceList(GridSettings grid, int userID)
        {
            var list = EmployeeProfileBiz.Instance.GetWorkExperiencePagedList(grid, userID);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                NewsData = "Null"
            },
                      JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<WorkExperience> WorkExperienceColumns { get; private set; } = GetWorkExperienceColumns();
        public static GridColumnModelList<WorkExperience> GetWorkExperienceColumns()
        {
            if (WorkExperienceColumns == null)
            {
                WorkExperienceColumns = new GridColumnModelList<WorkExperience>();
                WorkExperienceColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                WorkExperienceColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                WorkExperienceColumns.Add(x => x.FromDate).SetWidth("100");
                WorkExperienceColumns.Add(x => x.ToDate).SetWidth("200");
                WorkExperienceColumns.Add(x => x.CompanyName).SetWidth("200");
                WorkExperienceColumns.Add(x => x.Position).SetWidth("200");
            }
            return WorkExperienceColumns;
        }

        public ActionResult LoadEducationalBackgroundList(GridSettings grid, int userID)
        {
            var list = EmployeeProfileBiz.Instance.GetEducationalBackgroundPagedList(grid, userID);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                NewsData = "Null"
            },
                      JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<EducationalBackground> EducationalBackgroundColumns { get; private set; } = GetEducationalBackgroundColumns();
        public static GridColumnModelList<EducationalBackground> GetEducationalBackgroundColumns()
        {
            if (EducationalBackgroundColumns == null)
            {
                EducationalBackgroundColumns = new GridColumnModelList<EducationalBackground>();
                EducationalBackgroundColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                EducationalBackgroundColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                EducationalBackgroundColumns.Add(x => x.FromDate).SetWidth("100");
                EducationalBackgroundColumns.Add(x => x.ToDate).SetWidth("200");
                EducationalBackgroundColumns.Add(x => x.InstituteName).SetWidth("200");
                EducationalBackgroundColumns.Add(x => x.Field).SetWidth("200");
            }
            return EducationalBackgroundColumns;
        }
    }
}