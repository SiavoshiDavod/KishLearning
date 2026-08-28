using Antlr.Runtime;
using DocumentFormat.OpenXml.Office2010.Excel;
using SenakLearn.Biz;
using SenakLearn.Models;
using SenakLearn.Models.Parsian;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class FactorController : BaseController
    {
        private SWEntities db = new SWEntities();
        public async Task<ActionResult> Index(FactorModel model)
        {
            if (Current_learn_user == null)
            {
                TempData["ReturnUrl"] = Request.Url.PathAndQuery;
                return Redirect("/users/login");
            }

            if(model.Id!=0)
            {
                var factorDb = FactorBiz.Instance.Get(model.Id);
                return View(factorDb);
            }

            var existsFactor = FactorBiz.Instance.LoadFactor(Current_learn_user.id, model.IdForSale, model.ServiceName);

            if (existsFactor == null)
            {
                model.CreateDate = DateTime.Now;
                model.CreatedDate = model.CreateDate;


                model.FactorNo = FactorBiz.Instance.GetFactorCode(model.ServiceName);
                if (model.FactorNo == null)
                    throw new Exception("صدور فاکتور با مشکل روبرو شد با مدیر سایت تماس بگیرید!");
                if (Current_learn_userId != 0)
                {
                    model.UserName = Current_learn_user.user_name;
                    model.Mobile = Current_learn_user.Mobile;
                }
                model= FactorBiz.Instance.LoadNewFactore(model);
                return View(model);
            }
            else
            {
                return View(existsFactor);
            }
        }
        public async Task<ActionResult> Details(FactorModel model)
        {
            var factor = FactorBiz.Instance.Get(model.Id);
            return View("~/Views/Factor/Index.cshtml", factor);
        }

        //[HttpPost]
        //public async Task<ActionResult> PaymentFactor(FactorModel model)
        //{
        //    if (Current_learn_user == null)
        //    {
        //        TempData["ReturnUrl"] = Request.Url.PathAndQuery;
        //        return Redirect("/users/login");
        //    }
        //    if (string.IsNullOrEmpty(model.Mobile))
        //    {
        //        throw new Exception("موبایل کاربر الزامی است !");
        //    }
        //    if (model.Id != 0)
        //    {
        //        var factorInDb = FactorBiz.Instance.Get(model.Id);
        //        if (factorInDb.StatusId == FactorStatusEnum.Factor_Status_Success)
        //            throw new Exception("این فاکتور قبلا پرداخت شده است !");
        //    }
        //    if (Current_learn_userId != 0)
        //    {
        //        model.UserId = Current_learn_user.id;
        //        model.UserName = Current_learn_user.user_name;
        //        model.Mobile = Current_learn_user.Mobile?.ToEnglishNumber();
        //    }
        //    long factorId = 0;
        //    if (model.Id == 0)
        //        factorId = FactorBiz.Instance.AddFactor(model);
        //    else
        //    {
        //        factorId = model.Id;
        //        model.StatusId = FactorStatusEnum.Factor_Status_Sended;
        //    }
        //    FactorBiz.Instance.UpdateFactor(model);

        //    ZarinpalPaymentResponse result = zarinpalBiz.Instance.PerPaymentFactor((int)model.Amount, model.Descript, model.Email, model.Mobile, model.UserId.Value, model.Id, Current_learn_user);
        //    return Json(new { factorId, result }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<ActionResult> PaymentFactor(FactorModel model)
        {
            if (Current_learn_user == null)
            {
                TempData["ReturnUrl"] = Request.Url.PathAndQuery;
                return Redirect("/users/login");
            }

            if (string.IsNullOrEmpty(model.Mobile))
            {
                throw new Exception("موبایل کاربر الزامی است !");
            }

            if (model.Id != 0)
            {
                var factorInDb = FactorBiz.Instance.Get(model.Id);
                if (factorInDb != null && factorInDb.StatusId == FactorStatusEnum.Factor_Status_Success)
                    throw new Exception("این فاکتور قبلا پرداخت شده است !");
            }

            if (Current_learn_userId != 0)
            {
                model.UserId = Current_learn_user.id;
                model.UserName = Current_learn_user.user_name;
                model.Mobile = Current_learn_user.Mobile?.ToEnglishNumber();
            }

            long factorId = 0;
            if (model.Id == 0)
            {
                model.StatusId = FactorStatusEnum.Factor_Status_Sended;
                factorId = FactorBiz.Instance.AddFactor(model);
                model.Id = factorId; // انتساب ID جدید به مدل
            }
            else
            {
                factorId = model.Id;
                model.StatusId = FactorStatusEnum.Factor_Status_Sended;
                FactorBiz.Instance.UpdateFactor(model);
            }

            // فراخوانی درگاه پارسیان با آرگومان‌های صحیح و مرتب
            ParsianPaymentResponse result = ParsianBankBiz.Instance.PerPaymentFactor(
                (int)model.Amount,
                model.Descript,
                model.Email,
                model.Mobile,
                model.UserId.Value,
                factorId,
                Current_learn_user
            );

            return Json(new { factorId, result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ParsianCallback()
        {
            // دریافت پارامترهای ارسالی از سمت درگاه شاپرک پارسیان
            string tokenStr = Request.Form["Token"];
            string statusStr = Request.Form["status"];
            string orderIdStr = Request.Form["OrderId"];
            string rrnStr = Request.Form["RRN"];

            if (string.IsNullOrEmpty(tokenStr) || string.IsNullOrEmpty(orderIdStr))
            {
                TempData["ErrorMessage"] = "اطلاعات بازگشتی از بانک ناقص است.";
                return RedirectToAction("Index", "Home");
            }

            long factorId = long.Parse(orderIdStr);
            var factor = FactorBiz.Instance.Get(factorId);

            if (factor == null)
            {
                TempData["ErrorMessage"] = "فاکتور مورد نظر یافت نشد.";
                return RedirectToAction("Index", "Home");
            }

            // اگر وضعیت اولیه بازگشت صفر بود (کاربر در درگاه پرداخت موفق داشته است)
            if (statusStr == "0")
            {
                long token = long.Parse(tokenStr);

                // مرحله وریفای نهایی تراکنش با متد تاییدیه پارسیان
                var verifyResult = ParsianBankBiz.Instance.VerifyPayment(token);

                if (verifyResult.IsSuccess)
                {
                    // به‌روزرسانی اطلاعات فاکتور در دیتابیس
                    using (var context = new SWEntities())
                    {
                        var item = context.Factors.FirstOrDefault(a => a.Id == factor.Id);
                        if (item != null)
                        {
                            item.StatusId = FactorStatusEnum.Factor_Status_Success;
                            item.PaymentTransaction = tokenStr;
                            item.PaymentTrace = verifyResult.RRN > 0 ? verifyResult.RRN.ToString() : rrnStr;
                            item.PaymentDatetime = DateTime.Now;
                            context.SaveChanges();
                        }
                    }

                    // TODO: فعال‌سازی دسترسی کاربر به دوره، کتاب یا فایل خریداری شده
                    // بر اساس factor.ServiceName و factor.IdForSale

                    TempData["SuccessMessage"] = "پرداخت با موفقیت انجام شد و دسترسی شما فعال گردید.";
                    return RedirectToAction("Details", new { id = factor.Id });
                }
                else
                {
                    factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
                    FactorBiz.Instance.UpdateFactor(factor);
                    TempData["ErrorMessage"] = "تأیید نهایی پرداخت توسط بانک انجام نشد: " + verifyResult.Message;
                    return RedirectToAction("Details", new { id = factor.Id });
                }
            }
            else
            {
                factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
                FactorBiz.Instance.UpdateFactor(factor);
                TempData["ErrorMessage"] = "پرداخت توسط کاربر لغو شد یا در درگاه بانکی ناموفق بود.";
                return RedirectToAction("Details", new { id = factor.Id });
            }
        }


    }
}