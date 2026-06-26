using SurveyWeb.Biz;
using SurveyWeb.Models.Resturan;
using SurveyWeb.Models.wrapper;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class ShekayatController : BaseController
    {
        // GET: Shekayat
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Save(Shekayat model)
        {
            try
            {
                model.Ip = ip;
                var res = await ShekayatBiz.Instance.Save(model);
                //return Json(new ApiJsonResult() { success = true, Message = " کدرهگیری خود را یادداشت کنید: " + res.TrackingCode }, JsonRequestBehavior.AllowGet);

                TempData["SuccessMessage"] = " کدرهگیری خود را یادداشت کنید: " + res.TrackingCode;
                return Redirect("/");
            }
            catch (Exception e)
            {
                SetLog(e);
                ViewBag.Message = e.Message;
                return View("Index", model);
                //return Json(new ApiJsonResult() { success = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Track(int trackId)
        {
            try
            {
                if (trackId > 10000)
                {
                    Shekayat res = await ShekayatBiz.Instance.GetInclude(new Shekayat() { Id= trackId - 10000 }, "Cartable");
                    return Json(new ApiJsonResult() { Data = res, success = true, Message = res.Cartable.Name }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new ApiJsonResult() { success = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ApiJsonResult() { success = false, Message = "موردی یافت نشد" }, JsonRequestBehavior.AllowGet);
        }
    }
}
