using SenakLearn.Biz;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace SenakLearn.Controllers
{
    public class ZarinpalController : BaseController
    {
        // GET: Zarinpal
        public async Task<ActionResult> CallbackURL(string Status, string Authority)
        {
            var payment = zarinpalBiz.Instance.Callback(Status, Authority, out string message);
            if (payment == null)
            {
                TempData["ErrorMessage"] = message;
                return Redirect("/");
            }
            else
            {
                TempData["SuccessMessage"] = message;
                return Redirect("/Factor/Index?Id="+payment.FactorId);
            }
            //return View("CallbackURL", null, message);
        }
        public ActionResult Error(string Message)
        {
            TempData["ErrorMessage"] = Message;
            return Redirect("/");
        }
        public async Task<ActionResult> PaymentRequest(string Description, int? courseId, int? onlineClassId, int Amount = 0)
        {
            if (Current_learn_userId > 0)
            {
                var user = UserBiz.Instance.Find(Current_learn_userId);
                if (user != null)
                {
                    var message = zarinpalBiz.Instance.PaymentRequest(Amount, Description, user.Email, user.Mobile, user.id, courseId, onlineClassId, user, out string url);
                    if (url== "/MyClass/Index")
                    {
                        TempData["SuccessMessage"] = message;
                        await Biz.UserBiz.Instance.SendToAdminAsync(user,true);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = message;
                    }
                    return Redirect(url);
                }
            }
            var retUrl = "/users/RegisterOrLogin?ReturnUrl=";
            if (courseId != null)
                retUrl += "/Zarinpal/PaymentRequest?courseId=" + courseId + "&Amount=" + Amount;
            if (onlineClassId != null)
                retUrl += "/Zarinpal/PaymentRequest?onlineClassId=" + onlineClassId + "&Amount=" + Amount;
            return Redirect(retUrl);
        }
    }
}