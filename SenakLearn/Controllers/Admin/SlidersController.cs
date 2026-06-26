using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class SlidersController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "بنرسایت";
            ViewBag.ControllerName = "Sliders";
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.SlidersBiz.Instance.GetAllPagedList(grid);
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
            ViewBag.Title = "بنرسایت";
            ViewBag.ControllerName = "Sliders";
            if (id != 0)
            {
                var obj = Biz.SlidersBiz.Instance.Get(id);
                return View(obj);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Slider model, HttpPostedFileBase ImageFile)
        {
            if (model.Id == 0)
            {
                model.IconPath = SaveFile(ImageFile, pathFile.Slider);
            }
            else
            {
                model.IconPath = EditFile(ImageFile, pathFile.Slider, model.IconPath);
            }
            Biz.SlidersBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            //Slider Slider = Biz.SlidersBiz.Instance.Find(id);
            //System.IO.File.Delete(Server.MapPath("/images/" + pathFile.Slider + "/" + Slider.myFile));
            //db.Slider.Remove(Slider);
            //db.SaveChanges();
            //return RedirectToAction("Index");

            Biz.SlidersBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Slider> SliderColumns { get; private set; } = GetSliderColumns();
        public static GridColumnModelList<Slider> GetSliderColumns()
        {
            if (SliderColumns == null)
            {
                SliderColumns = new GridColumnModelList<Slider>();
                SliderColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                SliderColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                SliderColumns.Add(x => x.DropDownTitle).SetCaption("عنوان").SetWidth("50");
                SliderColumns.Add(x => x.PreTitle).SetCaption("پیش عنوان").SetWidth("50");
                SliderColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("100");
                SliderColumns.Add(x => x.Color).SetCaption("رنگ").SetWidth("50");
                SliderColumns.Add(x => x.Archive).SetCaption("وضعیت").SetWidth("50");
                SliderColumns.Add(x => x.ImageForGrid).SetCaption("عکس").SetWidth("50");
            }
            return SliderColumns;
        }
    }
}