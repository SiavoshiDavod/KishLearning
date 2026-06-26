using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class BaseTeacherController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var Current_learn_user = GetSessionUser();
                if (Current_learn_user.RoleId != Models.Roles.Teacher)
                {
                    filterContext.Result = new RedirectResult("/Users/Login?ReturnUrl="+ filterContext.HttpContext.Request.RawUrl);
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Users/Login?ReturnUrl="+ filterContext.HttpContext.Request.RawUrl);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}