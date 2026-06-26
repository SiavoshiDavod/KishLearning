using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.WebPages;

namespace UC.MVC.Controls
{
   
    public static class BaseEntityRecordComboHelper
    {
        public static BaseEntityRecordCombo BaseEntityRecordCombo(this HtmlHelper html, int BaseEntity,string ModelName,string PropertyName)
        {
            return new BaseEntityRecordCombo(html, BaseEntity,ModelName,PropertyName);
        }
    }
    public class BaseEntityRecordCombo : IHtmlString
    {
        private readonly HtmlHelper _html;
        private int _BaseEntity = -1;
        private string _ModelName ;
        private string _PropertyName;

        public BaseEntityRecordCombo(HtmlHelper html, int BaseEntity,string ModelName,string PropertyName)
        {
            if (html == null) throw new ArgumentNullException("html");
            _html = html;
            _ModelName = ModelName;
            _PropertyName = PropertyName;
            _BaseEntity = BaseEntity;
        }
      
        /// <summary>
        /// Creates our dynamic menu HTML, is outputted as
        /// </summary>
        /// <param name="htmlHelper">this</param>
        /// <param name="menu">The menu object to parse</param>
        /// <returns></returns>
        //public static string DynamicMenuResolver(this HtmlHelper htmlHelper, MenuItem menu)
        public override string ToString()
        {
            //verify we actually have menu subitems, if we dont return an empty string
            if (_BaseEntity <= 0)
                return string.Empty;
            else
            {
                //create our string builder to hold our html
                StringBuilder menuBuilder = new StringBuilder();
                ////create the opener tag add the class topnav (will use for jquery menu)

                //    menuBuilder.AppendLine("<select name="\+_PropertyName+\">");
                //    men
                //    menuBuilder.AppendLine("<ul class=\"sf-menu sf-vertical\" style=\"width:100%\">");
                ////iterate through all our submenut items
                //foreach (var item in _menu.Children)
                //    //call our recursive menu builder
                //    menuBuilder.Append(ResolveMenuLevel(item,this._mainUrl));
                
                ////close our menu tag
                //menuBuilder.AppendLine("</ul>");
                ////return the html
                //menuBuilder.AppendLine("<script type=\"text/javascript\">");
                //// initialise plugins
                //menuBuilder.AppendLine("jQuery(function () {");
                //menuBuilder.AppendLine(" $('ul.sf-menu').supersubs({minWidth:12,maxWidth:30,extraWidth:1}).superfish();");
                //menuBuilder.AppendLine(" });");
                //menuBuilder.AppendLine("</script>");

                return menuBuilder.ToString();
            }
        }

     

        #region IHtmlString Members

        public string ToHtmlString()
        {
            return ToString();
        }

        #endregion
    }
}
