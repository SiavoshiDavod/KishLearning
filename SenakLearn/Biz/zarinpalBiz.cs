using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AdobeConnectService;
using AdobeConnectService.AdobeConnect.Model;
using AdobeConnectSDK.Model;

namespace SenakLearn.Biz
{
    public class zarinpalBiz : RepositoryBase<SenakLearn.Models.ZarinpalPayment>
    {
        public static readonly zarinpalBiz Instance = new zarinpalBiz();
        public bool ExistByUser(int id, int userId)
        {
            using (var context = new SWEntities())
                return context.ZarinpalPayments.Any(x => x.Status == 100 && x.CourseId == id && x.UserId == userId);
        }
        public string SendWelcomeEmail(learn_user user, OnlineClass onlineClass)
        {
            long PrincipalId = 0;
            try
            {
                ClassUsingSdk adobe = new ClassUsingSdk(user.Email, string.IsNullOrEmpty(user.PassAdobe) ? "123456" : user.PassAdobe);
                if (adobe.IsLogin)
                {
                    PrincipalId = long.Parse(adobe.GetCurrentUserInfoViewModel().UserId);
                    var admin = ClassUsingSdkAdmin.Instance.GetAdminAdobe();
                    admin.PermissionSubscriptionUpdate(new PermaissionFilter() { AclId = onlineClass.AdobeScoId.Value, PrincipalId = long.Parse(adobe.GetCurrentUserInfoViewModel().UserId) }, true);
                }
            }
            catch (Exception)
            {
                try
                {
                    var admin = ClassUsingSdkAdmin.Instance.GetAdminAdobe();
                    if (admin.IsLogin)
                    {
                        if (PrincipalId == 0)
                        {
                            try
                            {
                                var Principal = admin.UserCreate(new PrincipalSetupViewModel() { FirstName = string.IsNullOrEmpty(user.Name) ? user.user_name : user.Name, LastName = string.IsNullOrEmpty(user.Family) ? user.user_name : user.Family, Email = user.Email, Password = string.IsNullOrEmpty(user.PassAdobe) ? "123456" : user.PassAdobe, Description = user.Mobile + " " + user.NationaCode + " " + user.Address });
                                PrincipalId = long.Parse(Principal.PrincipalId);

                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                        admin.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.LearnersOfGroupMembership, PrincipalId);//add user to Learners Group
                        admin.PermissionSubscriptionUpdate(new PermaissionFilter() { AclId = onlineClass.AdobeScoId.Value, PrincipalId = PrincipalId }, true);
                    }
                }
                catch (Exception)
                {
                }

            }
            var setting = SiteSetting.GetSetting.Instance.Get();
            string body = $"<div dir='rtl' style='text-align: right; '> جناب آقای / خانم <b>{user.NameForEmail}</b><br>به {setting.NameFa} خوش آمدید. <br>برای اتصال و استفاده از آموزش مجازی مراحل زیر را انجام دهید.<br>اول برنامه ادوبی را <a href='{setting.SiteUrl}Support/Index'>از اینجا</a> دانلود و نصب کنید .<br>دوم برای ورود به برنامه روی لینک زیر کلیک کنید.  <a href='{(string.IsNullOrEmpty(onlineClass.GoToAdobe) ? setting.AdobeServerUrl : onlineClass.GoToAdobe)}'>ورود به برنامه</a><br>سوم برای کار با برنامه و آشنایی با برنامه، فیلم های آموزشی را<a href='{setting.SiteUrl}Support/Student'>از اینجا</a> مشاهده فرمائید. <br>چهارم در صورت بروز مشکل با پشتیبانی سیستم تماس بگیرید. {setting.tel + " " + setting.Mobile}</div>";
            Biz.EmaiSmslBiz.Instance.AlertForClass(user.Email, "اطلاع رسانی کلاس آنلاین", body, setting);
            return "/MyClass/Index";
        }
        public string SendWelcomeEmail(learn_user user, learn_cours offlineClass)
        {
            var setting = SiteSetting.GetSetting.Instance.Get();
            string body = $"<div dir='rtl' style='text-align: right; '> جناب آقای / خانم <b>{user.NameForEmail}</b><br>به {setting.NameFa} خوش آمدید.<br>شما می توانید فیلم های ویدیویی {offlineClass.name}را  <a href='{setting.SiteUrl}DetailsCours/Index?type=1&id={offlineClass.id}'>از اینجا</a> دانلود و مشاهده کنید .<br> در صورت بروز مشکل با پشتیبانی سیستم تماس بگیرید. {setting.tel + " " + setting.Mobile}</div>";
            Biz.EmaiSmslBiz.Instance.AlertForClass(user.Email, "اطلاع رسانی ویدیوهای آموزشی", body, setting);
            return "/MyClass/Index";
        }

        public PagedList<ZarinpalPayment> GetAllPagedListCurrentUser(GridSettings grid, int userId)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                return context.ZarinpalPayments.Where(x => x.UserId == userId).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
        public List<ZarinpalPayment> GetAllPagedListCurrentUser(int skip, int take, int userId, bool? success)
        {
            using (var context = new SWEntities())
                return context.ZarinpalPayments.Where(x => x.UserId == userId && (success == null || (success == true && x.Status == 100) || (success == false && x.Status != 100))).OrderByDescending(x => x.Id).Take(take).Skip(skip).ToList();
        }

        //ﭘﺬﻳﺮﻧﺪﻩ : ﺁﺩﺭﺱ IP ﺍﺩﺭﺱ ﺁﻱ ﭘﻲ ) •  ﺳﺎﻳﺖ ﭘﺬﻳﺮﻧﺪﻩ ﻛﻪ ﺷﺨﺺ ﭘﺬﻳﺮﻧﺪﻩ ﻣﻲﺑﺎﻳﺪ ﺍﺯ ﻃﺮﻳﻖ ﺁﺯﻣﺎﻳﺸﮕﺎﻩ ﺯﺭﻳﻦ Main Server  ﭘﺎﻝ ﻓﺎﻳﻞ ﻫﺎ ﻭ ﺁﻣﻮﺯﺵ ﻫﺎﻱ ﻻﺯﻣﻪ ﺑﺮﺍﻱ ﺩﺭﻳﺎﻓﺖ ﺁﻱ ﭘﻲ ﺻﺤﻴﺢ ﺭﺍ ﺑﻜﺎﺭ ﮔﻴﺮﺩ ﻭ ﺁﻥ ﺭﺍ ﺑﻪ ﺳﺎﻳﺖ ﺯﺭﻳﻦ ﭘﺎﻝ ﺍﻋﻼﻡ ﻛﻨﺪ.
        public string PaymentRequest(int Amount, string Description, string Email, string Mobile, int userId, int? courseId, int? onlineClassId, learn_user user, out string returnUrl)
        {
            OnlineClass OnlineClasse = new OnlineClass();
            learn_cours learn_cours = new learn_cours();
            string message = "ثبت نام شما با موفقیت انجام شد";
            returnUrl = "/";

            using (var db = new SWEntities())
            {
                if (db.ZarinpalPayments.Any(X => X.UserId == userId && X.Status == 100 && (courseId == null || X.CourseId == courseId) && (onlineClassId == null || X.OnlineClassId == onlineClassId)))
                {
                    returnUrl = "/MyClass/Index";
                    return message = "شما قبلا در این دوره ثبت نام کرده اید. لطفا به پروفایل خود مراجعه کنید";
                }
                if (onlineClassId != null && onlineClassId > 0)
                {
                    OnlineClasse = db.OnlineClasses.FirstOrDefault(x => x.Id == onlineClassId);
                    if (OnlineClasse == null || OnlineClasse.Id <= 0)
                    {
                        return message = "خطا در انتخاب دوره";
                    }
                    if (OnlineClasse.ClassType != Enums.OnlineClassType.Registering)
                    {
                        db.OnlineClassRequests.Add(new Models.OnlineClassRequest() { UserId = userId, CreatedDate = DateTime.Now, OnlineClassId = OnlineClasse.Id });
                        db.SaveChanges();
                        returnUrl = "/MyClass/Index";
                        return message = " وضعیت کلاس " + OnlineClasse?.ClassTypeString + " است .در صورت برگزاری مجدد به شما اطلاع داده خواهد شد";
                    }

                    //if (db.ZarinpalPayments.Count(X => X.UserId == userId && X.OnlineClassId != null && X.OnlineClassId == onlineClassId) >= OnlineClasse.Capacity)
                    //{
                    //    db.OnlineClassRequests.Add(new Models.OnlineClassRequest() { UserId = userId, CreatedDate = DateTime.Now, OnlineClassId = OnlineClasse.Id });
                    //    db.SaveChanges();
                    //    return message = "متاسفانه ظرفیت کلاس پر شده است.در صورت برگزاری به شما اطلاع داده خواهد شد";
                    //}
                    Amount = OnlineClasse.Amount;
                    Description = OnlineClasse.name;
                }
                else if (courseId != null && courseId > 0)
                {
                    learn_cours = db.learn_cours.FirstOrDefault(x => x.id == courseId);
                    if (learn_cours == null)
                    {
                        return message = "خطا در انتخاب دوره";
                    }
                    if (learn_cours?.Monetary == null || learn_cours.Monetary <= 0)
                    {
                        return message = "این کلاس رایگان شده است";
                    }

                    Amount = learn_cours.Monetary.Value;
                    Description = learn_cours.name;
                }
                else { return message = "خطا در انتخاب دوره"; }
            }
            var zarinPayment = new Models.ZarinpalPayment { CourseId = courseId, OnlineClassId = onlineClassId, Amount = Amount, CreatedDate = DateTime.Now, UserId = userId };

            if (Amount <= 0)
            {
                using (var db = new SWEntities())
                {
                    zarinPayment.Status = 100;
                    db.ZarinpalPayments.Add(zarinPayment);
                    db.SaveChanges();
                    if (OnlineClasse.Id > 0)
                    {
                        OnlineClassBiz.Instance.CalculateClassType(OnlineClasse);
                    }
                    returnUrl = SendWelcomeEmail(user, OnlineClasse);
                }
            }
            else
            {
                string autohority;
                var BankUrl = Payment.Zarinpal.Instance.PaymentRequest(Amount, Description, Email, Mobile, out autohority);
                //var pay = new zarinpal.pay(MerchantID, Amount, Description, CallbackURL, Email, Mobile);
                // : (ﺷﻨﺎﺳﻪ ﻳﻜﺘﺎﻳﻲ ﻛﻪ ﺳﺎﻳﺖ ﺯﺭﻳﻦ Authority ﺷﻨﺎﺳﻪ ﻣﺮﺟﻊ ) • ﭘﺎﻝ ﺑﻪ ﺍﺯﺍﻱ ﻫﺮ ﺩﺭﺧﻮﺍﺳﺖ ﺧﺮﻳﺪ ﺑﻪ ﭘﺬﻳﺮﻧﺪﻩ ﺍﺭﺳﺎﻝ ﻣﻲﻛﻨﺪ، ﺟﻨﺲ ﺍﻳﻦ  ﻛﺎﺭﺍﻛﺘﺮ ﻣﻲﺑﺎﺷﺪ. 36 ﺑﺎ ﻃﻮﻝ RFC( ﺑﻮﺩﻩ ﻛﻪ ﻣﻄﺎﺑﻖ universally unique identifie) UUID ﭘﺎﺭﺍﻣﺘﺮ ﺍﺯ ﻧﻮﻉ 
                //var autohority = pay.StartPay();
                if (!string.IsNullOrEmpty(autohority))
                {
                    zarinPayment.Autohority = autohority;
                    using (var db = new SWEntities())
                    {
                        db.ZarinpalPayments.Add(zarinPayment);
                        db.SaveChanges();
                    }
                    returnUrl = BankUrl;
                }
                else
                {
                    message = BankUrl;
                }
            }
            return message;
        }
        public ZarinpalPayment Callback(string Status, string autohority, out string message)
        {
            message = "نتیجه پرداخت مشخص نمی باشد ! ";
            if (Status != "OK")
            {
                //message = "عملیات پرداخت با شکست مواجه شد";
                message = " عملیات تایید پرداخت با شکست مواجه شد " + Payment.StatusZarinPal.Dic[0];
                return null;
            }

            //var autohorityLong = long.Parse(autohority);

            long refId = 0;
            int value = 0;
            using (var db = new SWEntities())
            {
                var payment = db.ZarinpalPayments.Include(x => x.learn_user).FirstOrDefault(x => x.Autohority == autohority);
                if (payment == null)
                {
                    message = " اطلاعات پرداخت یافت نشد " + Payment.StatusZarinPal.Dic[0];
                    return null;
                }
                var factor = db.Factors.FirstOrDefault(a => a.Id == payment.FactorId);
                if (factor == null)
                {
                    message = " اطلاعات فاکتور یافت نشد " + Payment.StatusZarinPal.Dic[0];
                    return null;
                }
                var amount = payment.Amount;
                //if (obj.CreatedDate.AddMinutes(15)>DateTime.Now)
                //{
                //    //ﺍﺯ ﺯﻣﺎﻥ ﺍﺭﺳﺎﻝ ﻛﺎﺭﺑﺮ ﺑﻪ ﺯﺭﻳﻦ ﺩﻗﻴﻘﻪ( ﻣﺸﺘﺮﻱ ﻓﺮﺻﺖ ﺩﺍﺭﺩ ﻛﻪ ﻋﻤﻠﻴﺎﺕ ﭘﺮﺩﺍﺧﺖ ﺧﻮﺩ ﺭﺍ ﺩﺭ ﺳﺎﻳﺖ ﺯﺭﻳﻦ 15 ﭘﺎﻝ، ﺯﻣﺎﻥ ﻣﺤﺪﻭﺩﻱ )ﺣﺪﻭﺩ  ﭘﺎﻝ، ﺍﻧﺠﺎﻡ  . ﻣﻨﻘﻀﻲ ﻣﻲﺷﻮﺩ Authorityﺩﻫﺪ، ﺩﺭ ﻏﻴﺮ ﺍﻳﻨﺼﻮﺭﺕ 
                //    return "اطلاعات پرداخت یافت نشد" + StatusZarinPal[0];
                //}
                try
                {
                    value = Payment.Zarinpal.Instance.Verification(Status, autohority, amount, out refId);
                    payment.RefId = refId;
                    payment.Status = value;
                    message = Payment.StatusZarinPal.Dic[value];
                    if (refId > 0 || value == 100)
                    {
                        payment.RefId = refId;
                        factor.StatusId = FactorStatusEnum.Factor_Status_Success;

                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return null;
                }
                payment.Status = value;
                payment.UpdateDate = DateTime.Now;
                db.SaveChanges();
                //var errorResult = "اطلاعات دوره مورد نظر یافت نشد ولی " + StatusZarinPal[value] + " لطفا با بخش پشتیبانی سایت تماس بگیرید"; 
                return payment;
            }


        }

        /// <summary>
        /// آماده سازی درخواست برای ارسال به درگاه زرین پال
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="Description"></param>
        /// <param name="Email"></param>
        /// <param name="Mobile"></param>
        /// <param name="userId"></param>
        /// <param name="factorId"></param>
        /// <param name="user"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ZarinpalPaymentResponse PerPaymentFactor(int Amount, string Description, string Email, string Mobile, int userId, long factorId, learn_user user)
        {
            ZarinpalPaymentResponse Result = new ZarinpalPaymentResponse();
            FactorModel factor = new FactorModel();
            string message = "";
            //returnUrl = "/";

            using (var db = new SWEntities())
            {
                if (db.ZarinpalPayments.Any(X => X.UserId == userId && X.Status == 100 && (X.FactorId == factorId)))
                {
                    //returnUrl = "/Factor/Index"; 
                    Result.Message = "شما قبلا فاکتور را پرداخت کرده اید. لطفا به پروفایل خود مراجعه کنید";
                    Result.Status = false;
                    return Result;
                }
                if (factorId > 0)
                {
                    factor = db.Factors.FirstOrDefault(x => x.Id == factorId);
                    if (factor == null)
                    {
                        Result.Message = "خطا در انتخاب فاکتور";
                        Result.Status = false;
                        return Result;
                    }
                    if (factor?.Amount == null || factor?.Amount <= 0)
                    {
                        Result.Message = "این فاکتور رایگان شده است";
                        Result.Status = false;
                        return Result;
                    }

                    Amount = (int)factor?.Amount;
                    Description = factor.Descript;
                }
                else
                {
                    Result.Message = "خطا در انتخاب فاکتور";
                    Result.Status = false;
                    return Result;

                }
            }
            var zarinPayment = new Models.ZarinpalPayment { FactorId = factorId, Amount = Amount, CreatedDate = DateTime.Now, UserId = userId };

            if (Amount > 0)
            {
                string autohority;
                var BankUrlRequest = Payment.Zarinpal.Instance.PaymentRequestNew(Amount, Description, Email, Mobile, out autohority);
                //var pay = new zarinpal.pay(MerchantID, Amount, Description, CallbackURL, Email, Mobile);
                // : (ﺷﻨﺎﺳﻪ ﻳﻜﺘﺎﻳﻲ ﻛﻪ ﺳﺎﻳﺖ ﺯﺭﻳﻦ Authority ﺷﻨﺎﺳﻪ ﻣﺮﺟﻊ ) • ﭘﺎﻝ ﺑﻪ ﺍﺯﺍﻱ ﻫﺮ ﺩﺭﺧﻮﺍﺳﺖ ﺧﺮﻳﺪ ﺑﻪ ﭘﺬﻳﺮﻧﺪﻩ ﺍﺭﺳﺎﻝ ﻣﻲﻛﻨﺪ، ﺟﻨﺲ ﺍﻳﻦ  ﻛﺎﺭﺍﻛﺘﺮ ﻣﻲﺑﺎﺷﺪ. 36 ﺑﺎ ﻃﻮﻝ RFC( ﺑﻮﺩﻩ ﻛﻪ ﻣﻄﺎﺑﻖ universally unique identifie) UUID ﭘﺎﺭﺍﻣﺘﺮ ﺍﺯ ﻧﻮﻉ 
                //var autohority = pay.StartPay();
                if (!string.IsNullOrEmpty(autohority))
                {
                    zarinPayment.Autohority = autohority;
                    using (var db = new SWEntities())
                    {
                        db.ZarinpalPayments.Add(zarinPayment);
                        db.SaveChanges();
                    }
                    Result.BankUrl = BankUrlRequest.First().Value;
                    Result.Status = true;
                    Result.Authority = autohority;
                    Result.Payment = zarinPayment;
                    Result.CreateDate = DateTime.Now;
                    Result.ZarinpalStatus = 100;
                    return Result;
                }
                else
                {
                    Result.Message = BankUrlRequest.First().Value;
                    Result.ZarinpalStatus = BankUrlRequest.First().Key;
                    Result.Status = false;
                    return Result;

                }
            }

            return Result;
        }

    }
}