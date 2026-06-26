using SurveyWeb.Biz;
using SurveyWeb.Models;
using SurveyWeb.Models.wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class AdvertisingController : BaseProfileController
    {
        public async Task<ActionResult> Index()
        {
            if (Current_UserId > 0)
            {
                Resturant res = await Biz.AdvertisingBiz.Instance.FindByUserIdIncludeAdvertising(Current_UserId);
                if (res==null)
                {
                    return null;
                }
                if (res.Advertising == null|| res.Advertising.Count<=0)
                {
                    return View(new Advertising() { ResturantId=res.Id,Archive=true});
                }
                else
                {
                    return View(res.Advertising.First());
                }
            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> Create(Advertising model, System.Web.HttpPostedFileBase File)
        {
            if (model.Id == 0)
            {
                model.Archive = true;
                model.ImageUrl = SaveFile(File, pathFile.Advertising);
            }
            else
            {
                model.ImageUrl = EditFile(File, pathFile.Advertising, model.ImageUrl);
            }
            var res = await Biz.AdvertisingBiz.Instance.Save(model);
            await Biz.ResturantBiz.Instance.FindandResturantChanges(model.ResturantId, model.Id == 0 ? ResturantAddorEditnote.AddAdvertising : ResturantAddorEditnote.EditAdvertising);
            SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            //return View("Resturant", res);
            return RedirectToAction("Index", "Advertising");
        }

        public async Task<ActionResult> AdvertisingAttachement(int AdvertisingId)
        {
            Advertising res = await Biz.AdvertisingBiz.Instance.GetInclude(new Advertising() { Id = AdvertisingId }, "AdvertisingAttachements");
            if (res != null)
                return PartialView(res);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public async Task<ActionResult> FindAdvertisingAttachements(int AdvertisingId, int? id)
        {
            if (id == null)
            {
                return PartialView("AdvertisingAttachementSave", new AdvertisingAttachement() { AdvertisingId = AdvertisingId });
            }
            AdvertisingAttachement res = await Biz.AdvertisingBiz.Instance.FindAdvertisingAttachement(id.Value);
            if (res == null || res.AdvertisingId != AdvertisingId)
            {
                return null;
            }
            return PartialView("AdvertisingAttachementSave", res);
        }

        [HttpPost]
        public async Task<ActionResult> SaveAdvertisingAttachement(AdvertisingAttachement model, System.Web.HttpPostedFileBase File)
        {
            if (File == null)
            {
                return null;
            }

            var type = File.ContentType.ToLower();
            if (type.Contains("image"))
            {
                model.IsVideo = false;
            }
            else if (type.Contains("video"))
            {
                model.IsVideo = true;
            }
            else
            {
                return Json(new ApiJsonResult() { success = false, ErrorMessage = "نوع فایل معتبر نیست" }, JsonRequestBehavior.AllowGet);
            }

            model.ImageUrl = SaveFile(File, pathFile.Advertising);
            var res = await Biz.AdvertisingBiz.Instance.SaveAdvertisingAttachement(model);
            //return RedirectToAction("CheckList", "CheckListTypeCartables");
            return Json(new ApiJsonResult() { success = true, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> RemoveAdvertisingAttachement(int id)
        {
            string oldFileName = await Biz.AdvertisingBiz.Instance.RemoveAdvertisingAttachement(id, true);
            if (!string.IsNullOrEmpty(oldFileName) && System.IO.File.Exists("/images/" + pathFile.Advertising + "/" + oldFileName))
                System.IO.File.Delete(Server.MapPath("/images/" + pathFile.Advertising + "/" + oldFileName));
            return Json(new ApiJsonResult() { success = true, Message = "حذف با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }
    }
}