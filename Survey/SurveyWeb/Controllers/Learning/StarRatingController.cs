using SenakLearn.Biz;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class StarRatingController : BaseController
    {
        public ActionResult ShowRating(int id, PageType type = PageType.None)
        {
            Tuple<double, int> current = StarRatingBiz.Instance.GetRateByTypeAndId(type, id);
            StarRating model = new StarRating { TypeId = id, PageTypeId = type, UserId = Current_learn_userId, Ip = $" <p>({current.Item2} رای, میانگین: <strong>{current.Item1}</strong> از 5)</p>" };
            return PartialView("_StarRating", model);
        }

        [HttpPost]
        public ActionResult Create(StarRating model)
        {
            if (Current_learn_userId != 0)
            {
                model.UserId = Current_learn_userId;
            }
            model.Ip = HttpContext.Request.UserHostAddress;

            model.CreatedDate = DateTime.Now;

            try
            {
                model.Validate();
                if (Biz.StarRatingBiz.Instance.Save(model))
                    return Json(new { status = true, message = "امتیاز شما با موفقیت ثبت شد " }, JsonRequestBehavior.AllowGet);
                return Json(new { status = false, message = "شما قبلا به این دوره رای داده بودید" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}