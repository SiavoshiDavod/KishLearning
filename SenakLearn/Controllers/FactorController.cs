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

        private void SetBookDownloadFlags(FactorModel factor)
        {
            bool isBookProduct = factor != null
                && factor.StatusId == SenakLearn.Models.FactorStatusEnum.Factor_Status_Success
                && !string.IsNullOrWhiteSpace(factor.ServiceName)
                && factor.ServiceName.Trim().Contains("کتاب")
                && !string.IsNullOrEmpty(factor.IdForSale);

            ViewBag.IsBookProduct = isBookProduct;
            ViewBag.BookId = factor?.IdForSale ?? string.Empty;
        }

        public async Task<ActionResult> Index(FactorModel model)
        {
            if (Current_learn_user == null)
            {
                TempData["ReturnUrl"] = Request.Url.PathAndQuery;
                return Redirect("/users/login");
            }

            if (model.Id != 0)
            {
                var factorDb = FactorBiz.Instance.Get(model.Id);
                SetBookDownloadFlags(factorDb);
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
                    model.FullName = (!string.IsNullOrEmpty(Current_learn_user.Name) || !string.IsNullOrEmpty(Current_learn_user.Family)) ? (Current_learn_user.Name + " " + Current_learn_user.Family) : string.Empty;
                }


                model = FactorBiz.Instance.LoadNewFactore(model);
                SetBookDownloadFlags(model);
                return View(model);
            }
            else
            {
                SetBookDownloadFlags(existsFactor);
                return View(existsFactor);
            }
        }
        public async Task<ActionResult> Details(FactorModel model)
        {
            var factor = FactorBiz.Instance.Get(model.Id);
            SetBookDownloadFlags(factor);
            return View("~/Views/Factor/Index.cshtml", factor);
        }

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

            // ۱. بررسی وضعیت فاکتور موجود؛ اگر قبلاً پرداخت شده اجازه پرداخت مجدد داده نشود
            if (model.Id != 0)
            {
                var factorInDb = FactorBiz.Instance.Get(model.Id);
                if (factorInDb != null && factorInDb.StatusId == FactorStatusEnum.Factor_Status_Success)
                {
                    throw new Exception("این فاکتور قبلا پرداخت شده است !");
                }
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
                model.Id = factorId;
            }
            else
            {
                factorId = model.Id;
                model.StatusId = FactorStatusEnum.Factor_Status_Sended;
                FactorBiz.Instance.UpdateFactor(model);
            }

            // ۲. ایجاد یک تلاش جدید و منحصر‌به‌فرد در جدول FactorePaymentRequest
            string userIp = Request.UserHostAddress;
            long paymentRequestId = FactorPaymentRequestBiz.Instance.CreateRequest(
                factorId,
                model.UserId,
                model.Amount,
                userIp
            );

            // ۳. ارسال paymentRequestId به عنوان OrderId به پارسیان (تضمین یکتایی در هر تلاش)
            ParsianPaymentResponse result = ParsianBankBiz.Instance.PerPaymentFactor(
                (int)model.Amount,
                model.Descript,
                model.Email,
                model.Mobile,
                model.UserId.Value,
                paymentRequestId, // شناسه یکتای درخواست پرداخت به جای factorId ارسال می‌شود
                Current_learn_user
            );

            // ۴. ثبت توکن و وضعیت اولیه بازگشتی وب‌سرویس
            FactorPaymentRequestBiz.Instance.UpdateToken(
                paymentRequestId,
                result.Token.ToString(),
                result.Status,
                result.Message,
                result.ResponseContent
            );

            return Json(new { factorId, paymentRequestId, result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParsianCallback()
        {
            string tokenStr = Request.Form["Token"];
            string statusStr = Request.Form["status"];
            string orderIdStr = Request.Form["OrderId"];
            string rrnStr = Request.Form["RRN"];
            string hashCardNumber = Request.Form["HashCardNumber"];

            var formDump = new System.Text.StringBuilder();
            foreach (string key in Request.Form.AllKeys)
            {
                formDump.AppendFormat("{0}={1}; ", key, Request.Form[key]);
            }
            string rawCallbackData = formDump.ToString();

            if (string.IsNullOrEmpty(tokenStr) || string.IsNullOrEmpty(orderIdStr))
            {
                TempData["ErrorMessage"] = "اطلاعات بازگشتی از بانک ناقص است.";
                return RedirectToAction("Index", "Home");
            }

            if (!long.TryParse(orderIdStr, out long paymentRequestId))
            {
                TempData["ErrorMessage"] = "شناسه درخواست پرداخت نامعتبر است.";
                return RedirectToAction("Index", "Home");
            }

            using (var context = new SWEntities())
            {
                try
                {
                    var payRequest = context.FactorePaymentRequests.FirstOrDefault(a => a.Id == paymentRequestId);
                    if (payRequest == null)
                    {
                        TempData["ErrorMessage"] = "رکورد درخواست پرداخت یافت نشد.";
                        return RedirectToAction("Index", "Home");
                    }

                    var factor = context.Factors.FirstOrDefault(a => a.Id == payRequest.FactorId);
                    if (factor == null)
                    {
                        TempData["ErrorMessage"] = "فاکتور مربوط به این پرداخت یافت نشد.";
                        return RedirectToAction("Index", "Home");
                    }

                    payRequest.CallbackDateTime = DateTime.Now;
                    payRequest.RawCallbackData = rawCallbackData;
                    // برش رشته جهت اطمینان از عدم سرریز طول فیلد دیتابیس
                    payRequest.CardNumberMasked = !string.IsNullOrEmpty(hashCardNumber) && hashCardNumber.Length > 50
                                                    ? hashCardNumber.Substring(0, 50)
                                                    : hashCardNumber;
                    payRequest.RRN = !string.IsNullOrEmpty(rrnStr) && rrnStr.Length > 50 ? rrnStr.Substring(0, 50) : rrnStr;

                    if (short.TryParse(statusStr, out short st))
                        payRequest.TerminalStatus = st;

                    if (factor.StatusId == FactorStatusEnum.Factor_Status_Success)
                    {
                        TempData["SuccessMessage"] = "این فاکتور قبلاً با موفقیت تسویه شده است.";
                        return RedirectToAction("Details", new { id = factor.Id });
                    }

                    if (statusStr == "0")
                    {
                        if (long.TryParse(tokenStr, out long token) && token > 0)
                        {
                            var verifyResult = ParsianBankBiz.Instance.VerifyPayment(token);

                            payRequest.VerifyDateTime = DateTime.Now;
                            payRequest.VerifyStatus = verifyResult.Status;
                            payRequest.VerifyMessage = verifyResult.Message;

                            if (verifyResult.IsSuccess)
                            {
                                payRequest.StatusId = PaymentRequestStatusEnum.Succeeded;
                                payRequest.RRN = verifyResult.RRN > 0 ? verifyResult.RRN.ToString() : rrnStr;

                                factor.StatusId = FactorStatusEnum.Factor_Status_Success;
                                factor.PaymentTransaction = tokenStr;
                                factor.PaymentTrace = payRequest.RRN;
                                factor.PaymentDatetime = DateTime.Now;

                                context.SaveChanges();

                                TempData["SuccessMessage"] = "پرداخت با موفقیت انجام شد و سفارش شما ثبت گردید.";
                                return RedirectToAction("Details", new { id = factor.Id });
                            }
                            else
                            {
                                payRequest.StatusId = PaymentRequestStatusEnum.Failed;
                                factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
                                context.SaveChanges();

                                TempData["ErrorMessage"] = "تأیید نهایی پرداخت توسط بانک انجام نشد: " + verifyResult.Message;
                                return RedirectToAction("Details", new { id = factor.Id });
                            }
                        }
                        else
                        {
                            payRequest.StatusId = PaymentRequestStatusEnum.Failed;
                            factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
                            context.SaveChanges();

                            TempData["ErrorMessage"] = "توکن دریافتی نامعتبر است.";
                            return RedirectToAction("Details", new { id = factor.Id });
                        }
                    }
                    else
                    {
                        payRequest.StatusId = PaymentRequestStatusEnum.Failed;
                        factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
                        context.SaveChanges();

                        string errDesc = ParsianBankBiz.Instance.GetParsianErrorMessage(short.TryParse(statusStr, out short s) ? s : (short)-1531);
                        TempData["ErrorMessage"] = "پرداخت توسط درگاه لغو شد. علت: " + errDesc;
                        return RedirectToAction("Details", new { id = factor.Id });
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    var errList = new System.Text.StringBuilder();
                    foreach (var errItem in dbEx.EntityValidationErrors)
                    {
                        foreach (var err in errItem.ValidationErrors)
                        {
                            errList.AppendFormat("[Entity: {0}, Property: {1}, Error: {2}] ",
                                errItem.Entry.Entity.GetType().Name, err.PropertyName, err.ErrorMessage);
                        }
                    }

                    // لاگ خطا در فایل در ریشه پروژه جهت مشاهده بلادرنگ
                    string logPath = Server.MapPath("~/App_Data/Payment_EF_Errors.log");
                    System.IO.File.AppendAllText(logPath, DateTime.Now + " - " + errList.ToString() + Environment.NewLine);

                    TempData["ErrorMessage"] = "خطای اعتبارسنجی فیلدهای دیتابیس: " + errList.ToString();
                    return RedirectToAction("Index", "Home");
                }
            }
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
        //        if (factorInDb != null && factorInDb.StatusId == FactorStatusEnum.Factor_Status_Success)
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
        //    {
        //        model.StatusId = FactorStatusEnum.Factor_Status_Sended;
        //        factorId = FactorBiz.Instance.AddFactor(model);
        //        model.Id = factorId; // انتساب ID جدید به مدل
        //    }
        //    else
        //    {
        //        factorId = model.Id;
        //        model.StatusId = FactorStatusEnum.Factor_Status_Sended;
        //        FactorBiz.Instance.UpdateFactor(model);
        //    }

        //    // فراخوانی درگاه پارسیان با آرگومان‌های صحیح و مرتب
        //    ParsianPaymentResponse result = ParsianBankBiz.Instance.PerPaymentFactor(
        //        (int)model.Amount,
        //        model.Descript,
        //        model.Email,
        //        model.Mobile,
        //        model.UserId.Value,
        //        factorId,
        //        Current_learn_user
        //    );

        //    return Json(new { factorId, result }, JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //public ActionResult ParsianCallback()
        //{
        //    // دریافت پارامترهای ارسالی از سمت درگاه شاپرک پارسیان
        //    string tokenStr = Request.Form["Token"];
        //    string statusStr = Request.Form["status"];
        //    string orderIdStr = Request.Form["OrderId"];
        //    string rrnStr = Request.Form["RRN"];

        //    if (string.IsNullOrEmpty(tokenStr) || string.IsNullOrEmpty(orderIdStr))
        //    {
        //        TempData["ErrorMessage"] = "اطلاعات بازگشتی از بانک ناقص است.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    long factorId = long.Parse(orderIdStr);
        //    var factor = FactorBiz.Instance.Get(factorId);

        //    if (factor == null)
        //    {
        //        TempData["ErrorMessage"] = "فاکتور مورد نظر یافت نشد.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // اگر وضعیت اولیه بازگشت صفر بود (کاربر در درگاه پرداخت موفق داشته است)
        //    if (statusStr == "0")
        //    {
        //        long token = long.Parse(tokenStr);

        //        // مرحله وریفای نهایی تراکنش با متد تاییدیه پارسیان
        //        var verifyResult = ParsianBankBiz.Instance.VerifyPayment(token);

        //        if (verifyResult.IsSuccess)
        //        {
        //            // به‌روزرسانی اطلاعات فاکتور در دیتابیس
        //            using (var context = new SWEntities())
        //            {
        //                var item = context.Factors.FirstOrDefault(a => a.Id == factor.Id);
        //                if (item != null)
        //                {
        //                    item.StatusId = FactorStatusEnum.Factor_Status_Success;
        //                    item.PaymentTransaction = tokenStr;
        //                    item.PaymentTrace = verifyResult.RRN > 0 ? verifyResult.RRN.ToString() : rrnStr;
        //                    item.PaymentDatetime = DateTime.Now;
        //                    context.SaveChanges();
        //                }
        //            }

        //            // TODO: فعال‌سازی دسترسی کاربر به دوره، کتاب یا فایل خریداری شده
        //            // بر اساس factor.ServiceName و factor.IdForSale

        //            TempData["SuccessMessage"] = "پرداخت با موفقیت انجام شد و دسترسی شما فعال گردید.";
        //            return RedirectToAction("Details", new { id = factor.Id });
        //        }
        //        else
        //        {
        //            factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
        //            FactorBiz.Instance.UpdateFactor(factor);
        //            TempData["ErrorMessage"] = "تأیید نهایی پرداخت توسط بانک انجام نشد: " + verifyResult.Message;
        //            return RedirectToAction("Details", new { id = factor.Id });
        //        }
        //    }
        //    else
        //    {
        //        factor.StatusId = FactorStatusEnum.Factor_Status_NoPayment;
        //        FactorBiz.Instance.UpdateFactor(factor);
        //        TempData["ErrorMessage"] = "پرداخت توسط کاربر لغو شد یا در درگاه بانکی ناموفق بود.";
        //        return RedirectToAction("Details", new { id = factor.Id });
        //    }
        //}


    }
}