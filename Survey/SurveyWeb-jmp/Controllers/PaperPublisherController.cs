using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class PaperPublisherController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "ناشر";
            ViewBag.ControllerName = "PaperPublisher";
            return View("DropDown");
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.PaperPublisherBiz.Instance.GetAllPagedList(grid);
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
            ViewBag.Title = "ناشر";
            ViewBag.Id = id;
            ViewBag.ControllerName = "PaperPublisher";
            ViewBag.HasEnglish = true;
            ViewBag.HasIcon = true;
            if (id == 0)
            {
                ViewBag.DropDownTitle = "";
                ViewBag.DropDownTitleE = "";
                ViewBag.IconPath = "";
            }
            else
            {
                var obj = Biz.PaperPublisherBiz.Instance.Get(id);
                ViewBag.DropDownTitle = obj?.DropDownTitle ?? "";
                ViewBag.DropDownTitleE = obj?.DropDownTitleE ?? "";
                ViewBag.IconPath = obj?.IconPath ?? "";
            }
            return View("DropDownSave");
        }
        [HttpPost]
        public ActionResult Create(PaperPublisher model, HttpPostedFileBase ImageFile)
        {
            if (model.Id==0)
            {
                model.IconPath = SaveFile(ImageFile, pathFile.PaperPublisher);
            }
            else
            {
                model.IconPath = EditFile(ImageFile, pathFile.PaperPublisher, model.IconPath);
            }
            Biz.PaperPublisherBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Biz.PaperPublisherBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}