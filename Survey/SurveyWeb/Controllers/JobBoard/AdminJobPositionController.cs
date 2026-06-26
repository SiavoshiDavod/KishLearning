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
    public class AdminJobPositionController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();

            return View("~/Views/JobBoard/Admin/JobPositionVerification/Index.cshtml");
        }

        public ActionResult Find(int id)
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            var item = JobPositionBiz.Instance.Find(id);
            return PartialView("~/Views/JobBoard/Admin/JobPositionVerification/Details.cshtml", item);
        }

        public ActionResult LoadNotVerifiedList(GridSettings grid)
        {
            var list = Biz.JobPositionBiz.Instance.GetAllNotVerifiedPagedList(grid);
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

        public static GridColumnModelList<JobPosition> JobPositionColumns { get; private set; } = GetJobPositionColumns();
        public static GridColumnModelList<JobPosition> GetJobPositionColumns()
        {
            if (JobPositionColumns == null)
            {
                JobPositionColumns = new GridColumnModelList<JobPosition>();
                JobPositionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                JobPositionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                JobPositionColumns.Add(x => x.Title).SetWidth("200");
                JobPositionColumns.Add(x => x.Location).SetWidth("200");
                JobPositionColumns.Add(x => x.SalaryFrom).SetWidth("200");
                JobPositionColumns.Add(x => x.SalaryTo).SetWidth("200");
                JobPositionColumns.Add(x => x.Companyname).SetWidth("200");
                //JobPositionColumns.Add(x => x.UserID).SetCaption("خلاصه").SetWidth("500");
            }
            return JobPositionColumns;
        }

        public ActionResult AdminVerification(int id)
        {
            JobPositionBiz.Instance.AdminVerification(id);
            return PartialView("~/Views/JobBoard/Admin/EmployeeProfileVerification/List.cshtml");
        }




    }
}