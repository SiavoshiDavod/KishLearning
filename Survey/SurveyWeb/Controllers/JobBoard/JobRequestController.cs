using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.JobBoard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers.JobBoard
{
    public class JobRequestController : BaseController
    {
        // GET: JobCategory
        public ActionResult Index()
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            return View("~/Views/JobBoard/JobRequest/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Add(JobRequest model)
        {
            model.CreatedDate = DateTime.Now;
            model.CreatedDateShamsi = DateTimeExtensions.ToGregorianDate(DateTime.Now.ToString()).ToString();
            model.UserID = Current_UserId;
            JobRequestBiz.Instance.Add(model);
            return null;
        }

        public ActionResult Find(int id)
        {
            var item = JobRequestBiz.Instance.FindWrapper(id);
            return PartialView("~/Views/JobBoard/JobRequest/Details.cshtml", item);
        }

        public ActionResult AppliedBefore(int jobPositionId)
        {
            var item = JobRequestBiz.Instance.AppliedBefore(jobPositionId, Current_UserId);
            if (item != null)
                return Json(true);
            return Json(false);
        }

        public ActionResult Remove(int id)
        {
            JobRequestBiz.Instance.Remove(id);
            return PartialView("~/Views/JobBoard/JobRequest/List.cshtml");
        }

        public ActionResult FindByUserID()
        {
            ViewBag.PageTitle = "موقعیت های شغلی درخواست داده شده";
            var list = JobRequestBiz.Instance.FindByUserID(Current_UserId);
            return PartialView("~/Views/JobBoard/Client/Index.cshtml", list);
        }

        public ActionResult FindAllByJobPositionID()
        {
            ViewBag.PageTitle = "مشاهده درخواست ها";
            //var list = JobRequestBiz.Instance.FindByJobPositionID(Current_UserId);
            return PartialView("~/Views/JobBoard/JobRequest/Index.cshtml");
        }

        public ActionResult ConfirmResume(int id)
        {
            JobRequestBiz.Instance.ChangeStatus(id, JobRequest.StatusType.Accepted);
            return PartialView("~/Views/JobBoard/JobRequest/List.cshtml");

        }

        public ActionResult RejectResume(int id)
        {
            JobRequestBiz.Instance.ChangeStatus(id, JobRequest.StatusType.Rejected);
            return PartialView("~/Views/JobBoard/JobRequest/List.cshtml");
        }

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.JobRequestBiz.Instance.LoadPagedList(grid, Current_UserId);
            foreach (var item in list)
            {
                item.StatusName = item.Status.GetAttribute<DisplayAttribute>().Name;
            }
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
        public static GridColumnModelList<JobRequestWrapper> JobRequestColumns { get; private set; } = GetJobJobRequestColumns();
        public static GridColumnModelList<JobRequestWrapper> GetJobJobRequestColumns()
        {
            if (JobRequestColumns == null)
            {
                JobRequestColumns = new GridColumnModelList<JobRequestWrapper>();
                JobRequestColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                JobRequestColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                JobRequestColumns.Add(x => x.FirstName).SetWidth("200");
                JobRequestColumns.Add(x => x.LastName).SetWidth("200");
                JobRequestColumns.Add(x => x.Phone).SetWidth("200");
                JobRequestColumns.Add(x => x.Company).SetWidth("200");
                JobRequestColumns.Add(x => x.JobPositionTitle).SetWidth("200");
                JobRequestColumns.Add(x => x.StatusName).SetWidth("200");
            }
            return JobRequestColumns;
        }
    }
}