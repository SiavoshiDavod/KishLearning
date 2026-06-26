using System;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;

namespace SenakLearn.Controllers
{
    public class UserCommentController : BaseController
    {
        public ActionResult ContactUs(int? parentId, int? id, PageType type = PageType.None)
        {
            UserCommnet model = new UserCommnet { TypeId = id, PageTypeId = type, ParentId = parentId, Name = Current_learn_user?.Name + " " + Current_learn_user?.Family };
            if (type != PageType.None)
            {
                return PartialView("ContactUs", model);
            }
            return View("ContactUs", model);
        }
        [HttpPost]
        public ActionResult Create(UserCommnet model)
        {
            if (Current_learn_userId != 0)
            {
                model.UserId = Current_learn_userId;
                model.Name = Current_learn_user?.Name + " " + Current_learn_user?.Family;
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    model.Name = Current_learn_user?.user_name;
                }
               
            }
            //CheckGoogleRecapcha(model.googlerecaptcha);

            model.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                UserCommnetBiz.Instance.Save(model);

                TempData["SuccessMessage"] = "پیام شما با موفقیت ثبت شد. در اسرع وقت به آن رسیدگی می شود ";
                return Redirect("/");
            }
            try
            {
                model.Validate();
                ModelState.AddModelError(string.Empty, "اطلاعات را کامل پرکنید");
            }
            catch (Exception)
            {
                //  ModelState.AddModelError(string.Empty,e.Message);
            }
            return View("ContactUs", model);
        }
        
        public ActionResult ShowComments(int? id, PageType type = PageType.None)
        {
            var comments = Biz.UserCommnetBiz.Instance.GetAll(x => x.PageTypeId == type && x.TypeId == id && x.Status);
            return PartialView("ShowComment", comments);
        }
    }
}