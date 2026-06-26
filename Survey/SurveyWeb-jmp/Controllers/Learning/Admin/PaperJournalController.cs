using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class PaperJournalController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "ژورنال";
            ViewBag.ControllerName = "PaperJournal";
            return View("DropDown");
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.PaperJournalBiz.Instance.GetAllPagedList(grid);
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
            ViewBag.Title = "ژورنال";
            ViewBag.Id = id;
            ViewBag.ControllerName = "PaperJournal";
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
                var obj = Biz.PaperJournalBiz.Instance.Get(id);
                ViewBag.DropDownTitle = obj?.DropDownTitle ?? "";
                ViewBag.DropDownTitleE = obj?.DropDownTitleE ?? "";
                ViewBag.IconPath = obj?.IconPath ?? "";
            }
            return View("DropDownSave");
        }
        [HttpPost]
        public ActionResult Create(PaperJournal model, HttpPostedFileBase ImageFile)
        {
            if (model.Id == 0)
            {
                model.IconPath = SaveFile(ImageFile, pathFile.PaperPublisher);
            }
            else
            {
                model.IconPath = EditFile(ImageFile, pathFile.PaperPublisher, model.IconPath);
            }
            Biz.PaperJournalBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Biz.PaperJournalBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}