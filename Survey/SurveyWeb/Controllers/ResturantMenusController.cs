using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Controllers
{
    public class ResturantMenusController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid, bool Archive = false)
        {
            var list = Biz.ResturantBiz.Instance.GetAllResturantMenuPagedList(grid, Archive);
            //foreach (var item in list)
            //{
            //    item.Resturant.ResturantMenu = null;
            //}
            PagedList<ResturantMenu> obj = CloneUsingJsonConvertExtension.Clone(list);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = obj.ToArray(),
                ResturantMenuData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<ResturantMenu> ResturantMenuColumns { get; private set; } = GetResturantMenuColumns();
        public static GridColumnModelList<ResturantMenu> GetResturantMenuColumns()
        {
            if (ResturantMenuColumns == null)
            {
                ResturantMenuColumns = new GridColumnModelList<ResturantMenu>();
                ResturantMenuColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ResturantMenuColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                ResturantMenuColumns.Add(x => x.ResturantName).SetCaption("نام مرکزپذیرایی").SetWidth("300");
                ResturantMenuColumns.Add(x => x.Code).SetCaption("کد منو").SetWidth("100");
                ResturantMenuColumns.Add(x => x.Description).SetCaption("نام").SetWidth("100");
                ResturantMenuColumns.Add(x => x.Accepted).SetCaption("وضعیت").SetWidth("100");
                ResturantMenuColumns.Add(x => x.AdminDescription).SetCaption("توضیحات").SetWidth("200");
            }
            return ResturantMenuColumns;
        }

        public async Task<ActionResult> Accept(int id,string desc,bool accept=true)
        {
            await Biz.ResturantBiz.Instance.AcceptResturantMenu(id, accept, true, desc);
            return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SetFinalPrice(int id, int price)
        {
            await Biz.ResturantBiz.Instance.SetFinalPriceResturantMenu(id, price);
            return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SetName(int id, string name)
        {
            await Biz.ResturantBiz.Instance.SetNameResturantMenu(id, name);
            return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SetDescription(int id, string desc)
        {
            await Biz.ResturantBiz.Instance.SetDescriptionResturantMenu(id, desc);
            return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Details(int ResturantMenuId)
        {
            if ( ResturantMenuId > 0)
            {
                ResturantMenu res = await Biz.ResturantBiz.Instance.FindResturantMenuIncludeDetail(ResturantMenuId);
                if (res != null)
                {
                    return View(res);
                }
            }
            throw new HandledException("رکورد مورد نظر یافت نشد", "/ResturantMenus/Index");
        }
        public async Task<ActionResult> RemoveDetail(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantDetailMenuByAdmin(id);
            return Json(new ApiJsonResult() { success = res }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "ResturantMenu");
        }
    }
}
