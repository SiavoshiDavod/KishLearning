using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.WebPages;

namespace MVC.Controls
{
    public class MenuItem
    {
        string _Code;
        string _Title;
        string _URL;
        string _Description;
        string _MenuClass = "blue";
        string _Parent;
        string _Access;
        string _MenuID = "mega-menu-1";

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }        

        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }        

        public string Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }       

        public string Access
        {
            get { return _Access; }
            set { _Access = value; }
        }
        public string MenuClass
        {
            get { return _MenuClass; }
            set { this._MenuClass = value; }
        }
        public string MenuId
        {
            get { return _MenuID; }
            set { this._MenuID = value; }
        }

        public List<MenuItem> Children { get; set; }
    }
    public static class MenuHelper
    {
        public static Menu Menu(this HtmlHelper html, MenuItem menu)
        {
            return new Menu(html, menu);
        }
    }
    public class Menu : IHtmlString
    {
        private readonly HtmlHelper _html;
        private readonly MenuItem _menu;
        private Func<MenuItem, string> _displayProperty = item => item.ToString();
        private Func<MenuItem, HelperResult> _itemTemplate;
        private bool _isVertical = false;
        private string _mainUrl = "";

        public Menu(HtmlHelper html, MenuItem menu)
        {
            if (html == null) throw new ArgumentNullException("html");
            _html = html;
            _menu = menu;
            // The ItemTemplate will default to rendering the DisplayProperty
            _itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }
        public Menu IsVertical()
        {
            this._isVertical = true;
            return this;
        }
        public Menu MainUrl(string mainUrl)
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
                    menuBuilder.AppendLine("<ul class=\"sanatyar-menu\">");
                }
                else
                {
                    menuBuilder.AppendLine("<ul class=\"sf-menu sf-vertical\" style=\"width:100%\">");
                }
                //iterate through all our submenut items
                foreach (var item in _menu.Children)
                    //call our recursive menu builder
                    menuBuilder.Append(ResolveMenuLevel(item,this._mainUrl));
                
                //close our menu tag
                menuBuilder.AppendLine("</ul>");
                //return the html
                menuBuilder.AppendLine("<script type=\"text/javascript\">");
                // initialise plugins
                menuBuilder.AppendLine("jQuery(function () {");
                menuBuilder.AppendLine(" $('ul.sf-menu').supersubs({minWidth:12,maxWidth:30,extraWidth:1}).superfish();");
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
        private static string ResolveMenuLevel(MenuItem menu,string mainUrl)
        {
            //create our menu html reference
            StringBuilder menuBuilder = new StringBuilder();
            //create the menu item,
            menuBuilder.AppendLine(string.Format("<li><a href=\"{0}\"  title=\"{1}\">{2}</a>",
                ((HttpRuntime.AppDomainAppVirtualPath != null && HttpRuntime.AppDomainAppVirtualPath.Length > 1)?(HttpRuntime.AppDomainAppVirtualPath+"/"):"/")+ 
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
