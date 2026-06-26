using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SurveyWeb
{
    public static class SelectorExtension
    {
        public static MvcHtmlString VideoSelector<TModel>(this HtmlHelper<TModel> helper, string idExpression,
             string nameExpression,string value, object htmlAttributes)
        {
            string baseUrl = "'" + ((HttpRuntime.AppDomainAppVirtualPath.Length > 1) ? (HttpRuntime.AppDomainAppVirtualPath + "/") : "/") + "'";

            var htmlAttributesDic =
                (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) ??
                new Dictionary<string, object>();

            if (htmlAttributesDic.Keys.Contains("class"))
                htmlAttributesDic["class"] = htmlAttributesDic["class"] + " searchable";
            else
                htmlAttributesDic.Add("class", "searchable");

            if (!htmlAttributesDic.Keys.Contains("readonly"))
                htmlAttributesDic.Add("readonly", "readonly");

            if (!htmlAttributesDic.Keys.Contains("placeholder"))
                htmlAttributesDic.Add("placeholder", "لطفا دابل کلیک کنید");

            htmlAttributesDic.Add("onkeydown", "openVideoSelectorAsModal(event," + "'#" + idExpression + "',$(this)"+ ");");
            htmlAttributesDic.Add("ondblclick", "openVideoSelectorAsModal(event," + "'#" + idExpression + "',$(this)" + ");");

            var nameValue = "";
            //if (Guid.TryParse(value , out Guid videoId))
            //{
            //    //using (SWEntities db = new SWEntities())
            //    //{
            //    //    nameValue = db.VideoFiles.FirstOrDefault(x => x.VideoId == videoId)?.titel;
            //    //}
            //}

            var builder = new StringBuilder();
            builder.AppendLine("<script src='/Scripts/Learn.js' type='text/javascript'></script>");
            builder.AppendLine(helper.Hidden(idExpression, value).ToString());
            builder.AppendLine(helper.TextBox(nameExpression, nameValue, htmlAttributesDic).ToString());
            return new MvcHtmlString(builder.ToString());
        }

    }
}