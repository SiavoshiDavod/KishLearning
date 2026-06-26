using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class PaperTrendController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "گرایش";
            ViewBag.ControllerName = "PaperTrend";
            return View("DropDown");
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.PaperTrendBiz.Instance.GetAllPagedList(grid);
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
        public ActionResult Create(int id = 0)
        {
            ViewBag.Title = "گرایش";
            ViewBag.Id = id;
            ViewBag.DropDownTitle = id == 0 ? "" : Biz.PaperTrendBiz.Instance.Get(id)?.DropDownTitle ?? "";
            ViewBag.ControllerName = "PaperTrend";
            ViewBag.HasEnglish = false;
            ViewBag.HasIcon = false;
            return View("DropDownSave");
        }
        [HttpPost]
        public ActionResult Create(PaperTrend model)
        {
            Biz.PaperTrendBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Biz.PaperTrendBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}