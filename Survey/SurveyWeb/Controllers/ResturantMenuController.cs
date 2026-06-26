using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Controllers
{
    public class ResturantMenuController : BaseProfileController
    {

        // GET: ResturantDetailMenus
        public async Task<ActionResult> Index()
        {
            if (Current_UserId > 0)
            {
                List<ResturantMenu> res = await Biz.ResturantBiz.Instance.FindMenuByUserId(Current_UserId);
                return View(res);
            }
            throw new Exception("لطفا ابتدا اطلاعات مرکزپذیرایی خود را تکمیل کنید");
        }

        public async Task<ActionResult> Details(int ResturantMenuId)
        {
            if (Current_UserId > 0 && ResturantMenuId>0)
            {
                ResturantMenu res = await Biz.ResturantBiz.Instance.FindResturantMenuIncludeDetail(ResturantMenuId, Current_UserId);
                if (res != null)
                {
                    return View(res);
                }
            }
            throw new HandledException("رکورد مورد نظر یافت نشد", "/ResturantMenu/index");
        }

        public async Task<ActionResult> Save(string desc)
        {
            var res = await Biz.ResturantBiz.Instance.SaveResturantMenu(Current_UserId,desc);
            return Json(new ApiJsonResult() { success = res }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Remove(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantMenu(id);
            await Biz.ResturantBiz.Instance.FindandResturantChanges(res, ResturantAddorEditnote.RemoveMenu);
            return Json(new ApiJsonResult() { success = res>0 }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "ResturantMenu");
        }


        public async Task<ActionResult> Create(int menuId,int? id)
        {
            if (id==null)
            {
                 return PartialView(new ResturantDetailMenu() { ResturantMenuId=menuId});
            }
            ResturantDetailMenu res = await Biz.ResturantBiz.Instance.FindResturantDetailMenu(id.Value);
            if (res != null && res.ResturantMenuId==menuId)
            {
                return PartialView(res);
            }
            throw new HandledException("رکورد مورد نظر یافت نشد", "/ResturantMenu/index");
        }
        [HttpPost]
        public async Task<ActionResult> SaveDetail(ResturantDetailMenu model)
        {
            var res = await Biz.ResturantBiz.Instance.SaveResturantMenuDetail(model);
            return Json(new ApiJsonResult() { success = res}, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "ResturantMenu");
        }

        public async Task<ActionResult> RemoveDetail(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantDetailMenu(id);
            return Json(new ApiJsonResult() { success = res }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "ResturantMenu");
        }
    }
}
