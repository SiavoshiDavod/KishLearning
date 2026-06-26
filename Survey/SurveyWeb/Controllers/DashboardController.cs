using SurveyWeb.Biz;
using SurveyWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class DashboardController : BaseAdminController
    {
        public async Task<ActionResult> Index()
        {
            User Current_learn_user = GetSessionUser();
            if (Current_UserId > 0 && Current_learn_user?.RoleId == Roles.User)
            {
                Resturant res = await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId);
                if (res != null)
                {
                    var personelValidation = await Biz.ResturantBiz.Instance.ResturantPersonelValidation(res.Id);
                    var checkListValidation = await Biz.ResturantBiz.Instance.ResturantCheckListValidation(res.Id);
                    if (personelValidation && checkListValidation && res.CartableId == 10)//جامعه مراکز پذیرایی=8,معاونت گردشگری=9
                    {
                        ViewBag.styleResturant = "background-color:lightgreen";
                    }
                    else
                    {
                        var t = "";
                        if (!checkListValidation)
                        {
                            t += "\nمدارک ضروری پیوست نشده است";
                        }
                        if (!personelValidation)
                        {
                            t += "\nحداقل بایستی اطلاعات دو نفر از پرسنل یا مدیران ثبت گردد";
                        }
                        var notAccepted = await CheckListTypeCartableBiz.Instance.GetCount(x => x.Accepted == false && x.ResturantId == res.Id);
                        if (notAccepted > 0 || !personelValidation || !checkListValidation)
                        {
                            ViewBag.styleResturant = "background-color:red";
                            if (notAccepted > 0)
                                t += "\nاطلاعات شما تایید نشده است";
                        }
                        else
                        {
                            //var accepted = await CheckListTypeCartableBiz.Instance.GetCount(x => x.Accepted == true && x.ResturantId == res.Id);
                            //if (accepted == 0)
                            t += $"وضعیت شما در مرحله {res.Cartable?.Name} است";
                            ViewBag.styleResturant = "background-color:yellow";
                        }
                        ViewBag.titleResturant = t;
                    }


                    var notAcceptedresturantMenus = await Biz.ResturantBiz.Instance.GetMenuByResturantId(res.Id, false);
                    if (notAcceptedresturantMenus > 0)
                        ViewBag.styleResturantMenu = "background-color:yellow";
                    else
                    {
                        var acceptedresturantMenus = await Biz.ResturantBiz.Instance.GetMenuByResturantId(res.Id, true);
                        if (acceptedresturantMenus == 0)
                            ViewBag.styleResturantMenu = "background-color:red";
                        else
                            ViewBag.styleResturantMenu = "background-color:lightgreen";
                    }


                    var notAcceptedresturantPaymentsYearlyByDegree = await ResturantPaymentBiz.Instance.GetCount(x => x.IsAccepted == false && x.ResturantId == res.Id && x.PaymentTypeEnumId == Models.Resturan.PaymentTypeEnum.YearlyByDegree);
                    var notAcceptedresturantPaymentsYearlyByMeter = await ResturantPaymentBiz.Instance.GetCount(x => x.IsAccepted == false && x.ResturantId == res.Id && x.PaymentTypeEnumId == Models.Resturan.PaymentTypeEnum.YearlyByMeter);
                    var notAcceptedresturantPayments = notAcceptedresturantPaymentsYearlyByMeter + notAcceptedresturantPaymentsYearlyByDegree;
                    if (notAcceptedresturantPayments > 0)
                        ViewBag.styleResturantPayment = "background-color:yellow";
                    else
                    {
                        var acceptedresturantPaymentsYearlyByDegree = await ResturantPaymentBiz.Instance.GetCount(x => x.IsAccepted == true && x.ResturantId == res.Id && x.PaymentTypeEnumId == Models.Resturan.PaymentTypeEnum.YearlyByDegree);
                        var acceptedresturantPaymentsYearlyByMeter = await ResturantPaymentBiz.Instance.GetCount(x => x.IsAccepted == true && x.ResturantId == res.Id && x.PaymentTypeEnumId == Models.Resturan.PaymentTypeEnum.YearlyByMeter);
                        var acceptedresturantPayments = acceptedresturantPaymentsYearlyByMeter + acceptedresturantPaymentsYearlyByDegree;
                        if (acceptedresturantPayments == 0)
                            ViewBag.styleResturantPayment = "background-color:red";
                        else
                            ViewBag.styleResturantPayment = "background-color:lightgreen";
                    }


                    ViewBag.styleResturantCert = "";
                }
                else
                {
                    ViewBag.styleResturant = "background-color:red";
                    ViewBag.styleResturantMenu = "background-color:red";
                    ViewBag.styleResturantPayment = "background-color:red";
                }
            }
            return View("Dashboard");
        }
    }
}