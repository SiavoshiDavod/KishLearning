using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class GroupUserController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "گروه کاربری";
            ViewBag.ControllerName = "GroupUser";
            return View("DropDown");
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.GroupBiz.Instance.GetAllPagedList(grid);
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
            ViewBag.Title = "گروه کاربری";
            ViewBag.Id = id;
            ViewBag.DropDownTitle = id == 0 ? "" : Biz.GroupBiz.Instance.Get(id)?.DropDownTitle ?? "";
            ViewBag.ControllerName = "GroupUser";
            ViewBag.HasEnglish = false;
            ViewBag.HasIcon = false;
            return View("DropDownSave");
        }
        [HttpPost]
        public ActionResult Create(Group model)
        {
            Biz.GroupBiz.Instance.Save(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Biz.GroupBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}