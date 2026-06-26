using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class PaperUniversityController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "دانشگاه";
            ViewBag.ControllerName = "PaperUniversity";
            return View("DropDown");
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.PaperUniversityBiz.Instance.GetAllPagedList(grid);
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
            ViewBag.Title = "دانشگاه";
            ViewBag.Id = id;
            ViewBag.DropDownTitle = id == 0 ? "" : Biz.PaperUniversityBiz.Instance.Get(id)?.DropDownTitle ?? "";
            ViewBag.ControllerName = "PaperUniversity";
            ViewBag.HasEnglish = false;
            ViewBag.HasIcon = false;
            return View("DropDownSave");
        }
        [HttpPost]
        public ActionResult Create(PaperUniversity model)
        {
            Biz.PaperUniversityBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Biz.PaperUniversityBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}