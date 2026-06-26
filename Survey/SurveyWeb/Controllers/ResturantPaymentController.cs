using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using SurveyWeb.Models.Resturan;
using SurveyWeb.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class ResturantPaymentController : BaseProfileController
    {
        private const string targetExceptionUrl= "/ResturantPayment/Index";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.ResturantPaymentBiz.Instance.GetAllPagedListByUser(grid, Current_UserId);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                ResturantPaymentData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<ResturantPayment> ResturantPaymentColumns { get; private set; } = GetResturantPaymentColumns();
        public static GridColumnModelList<ResturantPayment> GetResturantPaymentColumns()
        {
            if (ResturantPaymentColumns == null)
            {
                ResturantPaymentColumns = new GridColumnModelList<ResturantPayment>();
                ResturantPaymentColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ResturantPaymentColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.PaymentTypeEnumName).SetCaption("نوع").SetWidth("200");
                ResturantPaymentColumns.Add(x => x.Price).SetCaption("مبلغ(ريال)").SetColumnRenderer(new NumberColumnRenderer()).SetSearchable(true).SetCellType(GridCellType.DECIMAL).SetWidth("100");
                ResturantPaymentColumns.Add(x => x.PaymentDateShamsi).SetCaption("تاريخ پرداخت").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.VarizKonande).SetCaption("واريز کننده").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.RefId).SetCaption("شماره فیش/کد رهگیری").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.IsAccepted).SetCaption("تاييد شده").SetWidth("50");
            }
            return ResturantPaymentColumns;
        }

        public async Task<ActionResult> Create(int? id, PaymentTypeEnum? type, bool IsOnlinePayment = false)
        {
            if (id != null)
            {
                ResturantPayment cartable = await ResturantPaymentBiz.Instance.Get(id.Value);
                if (cartable != null)
                {
                    return View(cartable);
                }

            }
            if (type == null)
            {
                throw new HandledException("خطا.نوع پرداخت مشخص نیست", targetExceptionUrl);
            }

            Models.Resturant resturant = await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId);
            if (resturant == null)
            {
                throw new HandledException("لطفا مجددا وارد سیستم شوید", "/");
            }
            ResturantPayment model = new ResturantPayment()
            {
                ResturantId = resturant.Id,
                UserId = Current_UserId,
                PaymentDate = DateTime.Now,
                PaymentTypeEnumId = type.Value,
                IsOnlinePayment = IsOnlinePayment
            };

            List<PaymentType> payments = new List<PaymentType>();
            switch (type)
            {
                case PaymentTypeEnum.YearlyByDegree:
                    PaymentType paymentYearlyByDegree = await PaymentTypeBiz.Instance.FindByDegreeAndType(resturant);
                    if (paymentYearlyByDegree == null)
                    {
                        throw new HandledException("برای مراکز پذیرایی شما حق عضویت سالانه تعریف نشده است. لطفا با پشتیبانی تماس بگیرید. ", targetExceptionUrl);
                    }
                    payments.Add(paymentYearlyByDegree);
                    model.PaymentType = paymentYearlyByDegree;
                    model.PaymentTypeId = paymentYearlyByDegree.Id;
                    model.Price = paymentYearlyByDegree.Price;
                    break;
                case PaymentTypeEnum.YearlyByMeter:
                    if (resturant.MeterGround <= 0)
                    {
                        throw new HandledException("متراژ زمین شما نامعتبر است", targetExceptionUrl);
                    }
                    PaymentType paymentYearlyByMeter = await PaymentTypeBiz.Instance.FindByMeter(resturant);
                    if (paymentYearlyByMeter == null)
                    {
                        throw new HandledException(" حق پرداخت سالیانه بر اساس متراژ تعریف نشده است. لطفا با پشتیبانی تماس بگیرید. ", targetExceptionUrl);
                    }
                   
                    model.Price = resturant.MeterGround * paymentYearlyByMeter.Price;
                    paymentYearlyByMeter.Title = resturant.MeterGround+ " متر زمین، "+ paymentYearlyByMeter.Title+":" + paymentYearlyByMeter.Price.ToString("N0") +"ریال ,جمع کل ";
                    paymentYearlyByMeter.Price= model.Price;
                    payments.Add(paymentYearlyByMeter);
                    model.PaymentType = paymentYearlyByMeter;
                    model.PaymentTypeId = paymentYearlyByMeter.Id;
                    break;
                //case PaymentTypeEnum.Karyabi:
                //case PaymentTypeEnum.Tablighat:
                default:
                    payments = await PaymentTypeBiz.Instance.GetAll(x => x.PaymentTypeEnumId == type && !x.Archive);
                    if (payments.Count <= 0)
                    {
                        throw new HandledException(" متاسفانه هیچ پرداختی تعریف نشده است. لطفا با پشتیبانی تماس بگیرید. ", targetExceptionUrl);
                    }
                    break;
            }

            ViewBag.PaymentTypeList = payments;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ResturantPayment model, System.Web.HttpPostedFileBase File)
        {
            model.UserId = Current_UserId;
            // PaymentType paymentType = await PaymentTypeBiz.Instance.Get(model.PaymentTypeId);
            // if (paymentType==null)
            // {
            //     throw new HandledException("نوع پرداخت يافت نشد", targetExceptionUrl);
            // }
            //// model.Price = paymentType.Price;
             
            if (model.Id == 0)
            {
                if (model.Price<=0)
                {
                    model.Price =( await PaymentTypeBiz.Instance.Get(model.PaymentTypeId)).Price;
                }
                //Models.Resturant resturant =await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId);
                //model.ResturantId = resturant.Id;
                model.IsAccepted = false;
                model.FishPic = SaveFile(File, pathFile.ResturantPayment);
            }
            else
            {
                model.FishPic = EditFile(File, pathFile.ResturantPayment, model.FishPic);
            }
            if (string.IsNullOrEmpty(model.FishPic))
            {
                throw new HandledException("لطفا فیش را بارگزاری کنید", targetExceptionUrl);
            }
            var res = await Biz.ResturantPaymentBiz.Instance.Save(model);
            await Biz.ResturantBiz.Instance.FindandResturantChanges(model.ResturantId, model.Id == 0 ?ResturantAddorEditnote.AddPayment:ResturantAddorEditnote.EditPayment);
            SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            //return View("Resturant", res);
            return RedirectToAction("Index", "ResturantPayment");
        }

        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                ResturantPayment cartable = await ResturantPaymentBiz.Instance.Get(id);
                if (cartable == null)
                {
                    throw new HandledException("خطا. رکورد یافت نشد", targetExceptionUrl);
                }
                if (cartable.IsAccepted)
                {
                    throw new HandledException("بدلیل تایید این پرداخت ،امکان حذف آن وجود ندارد", targetExceptionUrl);
                }
                if (!string.IsNullOrEmpty(cartable.FishPic) && System.IO.File.Exists("/images/" + pathFile.ResturantPayment + "/" + cartable.FishPic))
                    System.IO.File.Delete(Server.MapPath("/images/" + pathFile.ResturantPayment + "/" + cartable.FishPic));

                await Biz.ResturantPaymentBiz.Instance.Remove(id);

                return Json(new ApiJsonResult() { success = true, Message = "حذف با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception e)
            {
                return Json(new ApiJsonResult() { success = true, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}