using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace MVC.Controls.Attributes
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.RouteData.Values["lang"] != null && !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
            //{
            //    var lang = filterContext.RouteData.Values["lang"].ToString();
            //    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            //}
            //else
            //{
            //    var cookie = filterContext.HttpContext.Request.Cookies["Sanatyar.Portal.MVC.CurrentUICulture"];
            //    var langHeader = string.Empty;
            //    if (cookie != null)
            //    {
            //        langHeader = cookie.Value;
            //        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
            //    }
            //    else
            //    {
            //        langHeader = filterContext.HttpContext.Request.UserLanguages[0];
            //        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
            //    }
            //    filterContext.RouteData.Values["lang"] = langHeader;

            //}
            //HttpCookie _cookie = new HttpCookie("", Thread.CurrentThread.CurrentUICulture.Name);
            //_cookie.Expires = DateTime.Now.AddYears(1);
            //filterContext.HttpContext.Response.SetCookie(_cookie);


            //base.OnActionExecuting(filterContext);
            string language = (string)filterContext.RouteData.Values["language"] ?? "fa";
            string culture = (string)filterContext.RouteData.Values["culture"] ?? "IR";

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));


        }
    }
}