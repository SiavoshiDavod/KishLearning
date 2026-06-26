using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class BaseProfileController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var Current_learn_user = GetSessionUser();
                if (Current_learn_user.RoleId != Models.Roles.User)
                {
                    filterContext.Result = new RedirectResult("/Home/UserLogin?ReturnUrl=" + filterContext.HttpContext.Request.RawUrl);
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Home/UserLogin?ReturnUrl=" + filterContext.HttpContext.Request.RawUrl);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
