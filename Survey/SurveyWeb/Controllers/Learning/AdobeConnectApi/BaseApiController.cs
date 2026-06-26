using SenakLearn.Controllers;
using System.Web.Mvc;

namespace AdobeConnectService.Controllers
{
    public class BaseApiController : BaseAdminController
    {

        public ClassUsingSdk AdobeConnectSdk
        {
            get
            {
                var adob=new ClassUsingSdk(Current_learn_user.Email, Current_learn_user.PassAdobe,Current_learn_user.BREEZESESSION);
                Session["BREEZESESSION"] = adob.BREEZESESSION;
                return adob;
            }
        }
        internal ActionResult Ok(object o)
        {
            return Json(o, JsonRequestBehavior.AllowGet);
        }
    }
}
