using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using SurveyWeb.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace SurveyWeb.Controllers
{
    public class ResturantUsersController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid, bool Archive = true)
        {
            var list = Biz.UserBiz.Instance.GetAllPagedList(grid, Archive);
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

        public ActionResult Accept(int userId)
        {
            try
            {
                var user = Biz.UserBiz.Instance.Accept(userId);
                string content = $"ثبت نام اولیه شما تأیید شد، نام کاربری شما: {user.UserName} و کلمه عبور: {user.Pass} لطفا وارد سیستم شده و اطلاعات خود را تکمیل نمایید. با تشکر جامعه مراکز پذیرایی کیش";
                var res = Sms.Send.Instance.send(user.Mobile, content);
                return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ApiJsonResult { success = false, ErrorMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await UserBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            User user = await UserBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.UserImageUrl = SaveFile(File, pathFile.User);
                await UserBiz.Instance.Save(user);
            }
            else
            {
                // user.UserImageUrl = EditFile(File, pathFile.User, user.UserImageUrl);
                await UserBiz.Instance.UpdateAdmin(user);
            }
            return RedirectToAction("Index", "ResturantUsers");
        }



        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await UserBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await UserBiz.Instance.Remove(id);
            return RedirectToAction("Index", "ResturantUsers");
        }

    }
}