using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.JobBoard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers.JobBoard
{
    public class AdminEmployeeProfileController : BaseAdminController
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View("~/Views/JobBoard/Admin/EmployeeProfileVerification/Index.cshtml");
        }

        public ActionResult Find(int id)
        {
            var item = EmployeeProfileBiz.Instance.Find(id);
            return PartialView("~/Views/JobBoard/Admin/EmployeeProfileVerification/Details.cshtml", item);
        }

        public ActionResult AdminVerification(int id)
        {
            EmployeeProfileBiz.Instance.AdminVerification(id);
            return PartialView("~/Views/JobBoard/Admin/EmployeeProfileVerification/List.cshtml");
        }

        public ActionResult LoadNotVerifiedList(GridSettings grid)
        {
            var list = Biz.EmployeeProfileBiz.Instance.GetAllNotVerifiedPagedList(grid);
           
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
                EmployeeProfileColumns.Add(x => x.Username).SetWidth("100");
                EmployeeProfileColumns.Add(x => x.MilitaryStatusTitle).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.Phone).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.Specialty).SetWidth("200");
                EmployeeProfileColumns.Add(x => x.ProvinceOfResidence).SetWidth("200");
            }
            return EmployeeProfileColumns;
        }
    }
}