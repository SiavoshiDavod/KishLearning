using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.Mvc;
using System.Web.Mvc;

namespace MVC.Controls
{
    /// <summary>
    /// Multi submit button action
    /// </summary>
    public static class MultiSubmitButtonExtension
    {
        public static MvcHtmlString MultiSubmitButton(this HtmlHelper helper, string url, string name)
        {
            return MultiSubmitButton(helper, url, name, null, (IDictionary<string, object>)null);
        }
        public static MvcHtmlString MultiSubmitButton(this HtmlHelper helper, string url, string name, string buttonText)
        {
            return MultiSubmitButton(helper, url, name, buttonText, null);
        }
        public static MvcHtmlString MultiSubmitButton(this HtmlHelper helper, string url)
        {
            return MultiSubmitButton(helper, url, null, null, (IDictionary<string, object>)null);
        }
        public static MvcHtmlString MultiSubmitButton(this HtmlHelper helper, string url, string name, string buttonText, object htmlAttributes)
        {
            return helper.MultiSubmitButton(url, name, buttonText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        public static MvcHtmlString MultiSubmitButton(this HtmlHelper helper, string url, string name, string buttonText, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();

            // Add onClick handler
            string onClick = "$(this).parents('form').attr('action', '" + url + "');";
            if (htmlAttributes.ContainsKey("targetUpdate"))
            {
                onClick += "$(this).parents('form').attr('data-ajax-update', '#" + htmlAttributes["targetUpdate"] + "');";
            }
            if (htmlAttributes.ContainsKey("onClick"))
            {
                htmlAttributes["onClick"] = onClick + ";" + htmlAttributes["onClick"] + ";";
            }
            else
            {
                htmlAttributes.Add("onClick", onClick);
            }
            
            return helper.SubmitButton(name, buttonText, htmlAttributes);
        }
    }
}