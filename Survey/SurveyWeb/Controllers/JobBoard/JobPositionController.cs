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
    public class JobPositionController : BaseController
    {
        // GET: JobPosition
        public ActionResult Index()
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();

            return View("~/Views/JobBoard/JobPosition/Index.cshtml");
        }

        public ActionResult Add(JobPosition model )
        {
            if (string.IsNullOrEmpty(model.Description))
                throw new Exception("لطفا توضیحات موقعیت شغلی را کامل کنید");
            model.CreatedDate = DateTime.Now;
            model.CreatedDateShamsi = DateTimeExtensions.ToGregorianDate(DateTime.Now.ToString()).ToString();
            model.UserID = Current_UserId;
            JobPositionBiz.Instance.Add(model);
            return PartialView("~/Views/JobBoard/JobPosition/List.cshtml");
        }

        public ActionResult Find(int id)
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            var item = JobPositionBiz.Instance.Find(id);
            return PartialView("~/Views/JobBoard/JobPosition/Details.cshtml", item);
        }

        [AllowAnonymous]
        public ActionResult FindAll()
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            ViewBag.PageTitle = "همه موقعیت های شغلی";

            var list = JobPositionBiz.Instance.FindAll();
            return View("~/Views/JobBoard/Client/Index.cshtml", list);
        }

        public ActionResult FindAllByUserID()
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            ViewBag.PageTitle = "همه موقعیت های شغلی ایجاد شده";

            var list = JobPositionBiz.Instance.FindAllByUserID(Current_UserId);
            return View("~/Views/JobBoard/Client/Index.cshtml", list);
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            var item = JobPositionBiz.Instance.JobPositionDetails(id);
            return View("~/Views/JobBoard/Client/Details.cshtml", item);
        }

        public ActionResult Remove(int id)
        {
            JobPositionBiz.Instance.Remove(id);
            return PartialView("~/Views/JobBoard/JobPosition/List.cshtml");
        }
        
        public ActionResult Update(JobPosition model)
        {
            model.UpdateDate = DateTime.Now;
            model.UpdateDateShamsi = DateTimeExtensions.ToGregorianDate(DateTime.Now.ToString()).ToString();

            JobPositionBiz.Instance.Update(model);
            return PartialView("~/Views/JobBoard/JobPosition/List.cshtml");
        }

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.JobPositionBiz.Instance.GetAllPagedList(grid);
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
    }
}