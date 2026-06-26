using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.WebPages;

namespace MVC.Controls.Menus.Mega
{
   
    /// <summary>
    /// 
    /// </summary>
    public static class MegaMenuHelper
    {
        public static MegaMenu MegaMenu(this HtmlHelper html, MenuItem menu)
        {
            return new MegaMenu(html, menu);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MegaMenu : IHtmlString
    {
        private readonly HtmlHelper _html;
        private readonly MenuItem _menu;
        private Func<MenuItem, string> _displayProperty = item => item.ToString();
        private Func<MenuItem, HelperResult> _itemTemplate;
        private bool _isVertical = false;
        private string _mainUrl = "";

        public MegaMenu(HtmlHelper html, MenuItem menu)
        {
            if (html == null) throw new ArgumentNullException("html");
            _html = html;
            _menu = menu;
            // The ItemTemplate will default to rendering the DisplayProperty
            _itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }
        public MegaMenu IsVertical()
        {
            this._isVertical = true;
            return this;
        }
        public MegaMenu MainUrl(string mainUrl)
        {
            this._mainUrl = mainUrl;
            return this;
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
            if (_menu == null || _menu.Children == null || _menu.Children.Count() == 0)
                return string.Empty;
            else
            {
                //create our string builder to hold our html
                StringBuilder menuBuilder = new StringBuilder();
                //create the opener tag add the class topnav (will use for jquery menu)
                if (!_isVertical)
                {
                    //menuBuilder.AppendLine("<ul class=\"sf-menu\">");
                    menuBuilder.AppendLine("<div class=\"" + _menu.MenuClass + "\">");
                }
                else
                {
                    menuBuilder.AppendLine("<div class=\"" + _menu.MenuClass + "\">");
                }
                menuBuilder.AppendLine("<ul class=\"mega-menu\" id=\"" + _menu.MenuId + "\">");
                //iterate through all our submenut items
                foreach (var item in _menu.Children)
                    //call our recursive menu builder
                    menuBuilder.Append(ResolveMenuLevel(item, this._mainUrl));
                menuBuilder.AppendLine("</ul>");
                //close our menu tag
                menuBuilder.AppendLine("</div>");
                //return the html
                menuBuilder.AppendLine("<script type=\"text/javascript\">");
                // initialise plugins
                menuBuilder.AppendLine("jQuery(function () {");
                menuBuilder.AppendLine(" $('#"+_menu.MenuId+"').dcMegaMenu({");
                menuBuilder.AppendLine("rowItems: '5',");
                menuBuilder.AppendLine("speed: 'fast'");
                menuBuilder.AppendLine(" });");
                menuBuilder.AppendLine(" });");
                menuBuilder.AppendLine("</script>");

                return menuBuilder.ToString();
            }
        }

        /// <summary>
        /// Recursive menu html resolver, can be called on to iteself for N-Level of recursions
        /// </summary>
        /// <param name="menu">the item to parse</param>
        /// <returns></returns>
        private static string ResolveMenuLevel(MenuItem menu, string mainUrl)
        {
            //create our menu html reference
            StringBuilder menuBuilder = new StringBuilder();
            //create the menu item,
            menuBuilder.AppendLine(string.Format("<li><a href=\"{0}\"  title=\"{1}\">{2}</a>",
                ((HttpRuntime.AppDomainAppVirtualPath != null && HttpRuntime.AppDomainAppVirtualPath.Length > 1) ? (HttpRuntime.AppDomainAppVirtualPath + "/") : "/") +
                                            menu.URL,
                                            menu.Description,
                                            menu.Title));
            //if there are sub items (sub menus) parse them
            if (menu.Children.Count() > 0)
            {
                //create a submenu opener tag add our class subnav, this will be for our jquery implementation
                menuBuilder.AppendLine("<ul>");
                //loop our subitems
                foreach (var item in menu.Children)
                    //build our menu through our self calling method
                    menuBuilder.Append(ResolveMenuLevel(item, mainUrl));
                //close the submenu tag
                menuBuilder.AppendLine("</ul>");
            }
            menuBuilder.AppendLine("</li>");
            //return this level of the menu, remeber is recursive so this is N-Levels deep
            return menuBuilder.ToString();
        }


        #region IHtmlString Members

        public string ToHtmlString()
        {
            return ToString();
        }

        #endregion
    }
}
