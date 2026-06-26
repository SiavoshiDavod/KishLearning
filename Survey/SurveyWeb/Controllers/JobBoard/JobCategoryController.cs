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
    public class JobCategorysController : BaseAdminController
    {
        // GET: JobCategory
        public ActionResult Index()
        {
            return View("~/Views/JobBoard/JobCategory/Index.cshtml");
        }

        public ActionResult Add(JobCategory model )
        {
            model.CreatedDate = DateTime.Now;
            model.CreatedDateShamsi = DateTimeExtensions.ToGregorianDate(DateTime.Now.ToString()).ToString();
            model.UserID = Current_UserId;
            JobCategoryBiz.Instance.Add(model);
            return PartialView("~/Views/JobBoard/JobCategory/List.cshtml");
        }

        public ActionResult Find(int id)
        {
           var item = JobCategoryBiz.Instance.Find(id);
            return PartialView("~/Views/JobBoard/JobCategory/Details.cshtml", item);
        }

        public ActionResult Remove(int id)
        {
            JobCategoryBiz.Instance.Remove(id);
            return PartialView("~/Views/JobBoard/JobCategory/List.cshtml");
        }
        public ActionResult Update(JobCategory model)
        {
            JobCategoryBiz.Instance.Update(model);
            return PartialView("~/Views/JobBoard/JobCategory/List.cshtml");
        }

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.JobCategoryBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<JobCategory> JobCategoryColumns { get; private set; } = GetJobCategoryColumns();
        public static GridColumnModelList<JobCategory> GetJobCategoryColumns()
        {
            if (JobCategoryColumns == null)
            {
                JobCategoryColumns = new GridColumnModelList<JobCategory>();
                JobCategoryColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                JobCategoryColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                JobCategoryColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("200");
                //JobCategoryColumns.Add(x => x.UserID).SetCaption("خلاصه").SetWidth("500");
            }
            return JobCategoryColumns;
        }
    }
}