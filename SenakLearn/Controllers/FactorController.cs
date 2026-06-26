using SenakLearn.Biz;
using SenakLearn.Models;

using System;
using System.Collections.Generic;
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
        [HttpPost]
        public async Task<ActionResult> PaymentFactor(FactorModel model)
        {
            if (string.IsNullOrEmpty(model.Mobile))
            {
                throw new Exception("موبایل کاربر الزامی است !");
            }
            if (model.Id != 0)
            {
                var factorInDb = FactorBiz.Instance.Get(model.Id);
                if (factorInDb.StatusId == FactorStatusEnum.Factor_Status_Success)
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
                factorId = FactorBiz.Instance.AddFactor(model);
            else
            {
                factorId = model.Id;
                model.StatusId = FactorStatusEnum.Factor_Status_Sended;
            }
            FactorBiz.Instance.UpdateFactor(model);

            ZarinpalPaymentResponse result = zarinpalBiz.Instance.PerPaymentFactor((int)model.Amount, model.Descript, model.Email, model.Mobile, model.UserId.Value, model.Id, Current_learn_user);
            return Json(new { factorId, result }, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public async Task<ActionResult> Print(int id, int type = 1)
        //{
        //    StiReport Report;
        //    try
        //    {
        //        Report = new StiReport();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    var mrtFileName = "MyStaticFiles\\Mrt\\ReportOrder.mrt";
        //    Report.Load(mrtFileName);

        //    Report.Render(false);
        //    Report.RegBusinessObject("Order", null);
        //    Report.RegBusinessObject("Order_Line", null);
        //    Report.Dictionary.Synchronize();
        //    return File(PrintStiToPdf(Report, "").FileContents, "application/pdf");
        //}
    }
}