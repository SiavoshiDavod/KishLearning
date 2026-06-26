using SurveyWeb.Biz;
using SurveyWeb.Models;
using SurveyWeb.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class StarRatingController : BaseController
    {
        public ActionResult ShowRating(int id, PageType type = PageType.None)
        {
            Tuple<double, int> current = StarRatingBiz.Instance.GetRateByTypeAndId(type, id);
            StarRating model = new StarRating { TypeId = id, PageTypeId = type, UserId = Current_UserId, Ip = $" <p>({current.Item2} رای, میانگین: <strong>{current.Item1}</strong> از 5)</p>" };
            return PartialView("_StarRating", model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(StarRating model)
        {
            if (Current_UserId != 0)
            {
                model.UserId = Current_UserId;
            }
            model.Ip = HttpContext.Request.UserHostAddress;

            model.CreatedDate = DateTime.Now;

            try
            {
                model.Validate();
                if (await Biz.StarRatingBiz.Instance.SaveRating(model))
                    return Json(new ApiJsonResult { success = true, Message = "امتیاز شما با موفقیت ثبت شد " }, JsonRequestBehavior.AllowGet);
                return Json(new ApiJsonResult { success = false, ErrorMessage = "شما قبلا به این مرکزپذیرایی رای داده بودید" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ApiJsonResult { success = false, ErrorMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}