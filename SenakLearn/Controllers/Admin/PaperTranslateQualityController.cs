using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class PaperTranslateQualityController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "کیفیت ترجمه";
            ViewBag.ControllerName = "PaperTranslateQuality";
            return View("DropDown");
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.PaperTranslateQualityBiz.Instance.GetAllPagedList(grid);
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
            ViewBag.Title = "کیفیت ترجمه";
            ViewBag.Id = id;
            ViewBag.DropDownTitle = id == 0 ? "" : Biz.PaperTranslateQualityBiz.Instance.Get(id)?.DropDownTitle ?? "";
            ViewBag.ControllerName = "PaperTranslateQuality";
            ViewBag.HasEnglish = false;
            ViewBag.HasIcon = false;
            return View("DropDownSave");
        }
        [HttpPost]
        public ActionResult Create(PaperTranslateQuality model)
        {
            Biz.PaperTranslateQualityBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Biz.PaperTranslateQualityBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}