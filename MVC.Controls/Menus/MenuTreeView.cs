using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.WebPages;
using System.Web.UI;
using MVC.Controls.Tree;
using System.Web.Script.Serialization;

namespace MVC.Controls.Menus
{
    public static class MenuTreeViewHelper
    {
        /// <summary>
        /// Create an HTML tree from a recursive collection of items
        /// </summary>
        public static MenuTreeView<T> MenuTreeView<T>(this HtmlHelper html, string TreeId, string SelectedMainMenuId)
        {
            return new MenuTreeView<T>(html, TreeId, SelectedMainMenuId);
        }
    }


    /// <summary>
    /// Create an HTML tree from a resursive collection of items
    /// </summary>
    public class MenuTreeView<T> : IHtmlString
    {
        public static readonly int ACCOUNT_ROOT_MENU = 98;
        public static readonly int USERMANAGEMENT_ROOT_MENU = 3;
        public static readonly int SALES_ROOT_MENU = 90;
        public static readonly int CHEST_ROOT_MENU = 167;
        public static readonly int STORE_ROOT_MENU = 48;
        public static readonly int PAYROLL_ROOT_MENU = 174;
        public static readonly int PERSONEL_ROOT_MENU = 175;
        public static readonly int KARGOZINI_ROOT_MENU = 176;

        public static readonly string ACCOUNT_MENU_TREE_ID = "AccountMainMenuTree";
        public static readonly string USERMANAGEMENT_MENU_TREE_ID = "UserManagementMainMenuTree";
        public static readonly string SALES_MENU_TREE_ID = "SalesMainMenuTree";
        public static readonly string CHEST_MENU_TREE_ID = "ChestMainMenuTree";
        public static readonly string STORE_MENU_TREE_ID = "StoreMainMenuTree";
        public static readonly string PAYROLL_MENU_TREE_ID = "PayrollMainMenuTree";
        public static readonly string PERSONEL_MENU_TREE_ID = "PersonelMainMenuTree";
        public static readonly string KARGOZINI_MENU_TREE_ID = "KargoziniMainMenuTree";

        private readonly HtmlHelper _html;
        private readonly IEnumerable<T> _items = Enumerable.Empty<T>();
        private Func<T, string> _displayProperty = item => item.ToString();
        private Func<T, IEnumerable<T>> _childrenProperty;
        private string _emptyContent = "No children";
        private bool _cShowCreate = false;
        private string cCreateFunctionName = "";
        private bool _cShowEdit = false;
        private bool _showContextMenu = false;
        private string cEditFunctionName = "";
        private bool _ajaxLoadContent = true;
        private bool _cShowRemove = false;
        private string cRemoveFunctionName = "";
        private string _onSelect = "";
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();
        private IDictionary<string, object> _childHtmlAttributes = new Dictionary<string, object>();
        private Func<T, HelperResult> _itemTemplate;
        private string TreeId;
        private string _langDirection = "rtl";
        private string _selectedMainMenuId;
        private string contextMenu;
        private bool _isCheckbox = false;
        private bool _isCheckboxTwoState = false;
        private bool _showCRRM = false;
        private string _checkedNodes;
        private TreeJsonModel _treeJsonModel;
        /// <summary>
        /// Menu Tree View
        /// </summary>
        /// <param name="html"></param>
        /// <param name="TreeId"></param>
        /// <param name="MemberValue"></param>
        public MenuTreeView(HtmlHelper html, string TreeId, string SelectedMainMenuId)
        {
            if (html == null) throw new ArgumentNullException("html");
            _html = html;
            //_items = items;
            this._selectedMainMenuId = SelectedMainMenuId;
            this.TreeId = TreeId;
            // The ItemTemplate will default to rendering the DisplayProperty
            _itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }

        /// <summary>
        /// The property which will display the text rendered for each item
        /// </summary>
        public MenuTreeView<T> ItemText(Func<T, string> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            _displayProperty = selector;
            return this;
        }


        /// <summary>
        /// The template used to render each item in the tree view
        /// </summary>
        public MenuTreeView<T> ItemTemplate(Func<T, HelperResult> itemTemplate)
        {
            if (itemTemplate == null) throw new ArgumentNullException("itemTemplate");
            _itemTemplate = itemTemplate;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="showContextMenu"></param>
        /// <returns></returns>
        public MenuTreeView<T> ShowContextMenu(bool showContextMenu)
        {
            this._showContextMenu = showContextMenu;
            return this;
        }
        public MenuTreeView<T> ShowCheckbox(bool showCheckbox)
        {
            this._isCheckbox = showCheckbox;
            this._isCheckboxTwoState = false;
            return this;
        }
        public MenuTreeView<T> ShowCheckbox(bool showCheckbox, bool isTwoState)
        {
            this._isCheckbox = showCheckbox;
            this._isCheckboxTwoState = isTwoState;
            return this;
        }
        public MenuTreeView<T> ShowCRRM(bool showCRRM)
        {
            this._showCRRM = showCRRM;
            return this;
        }
        public MenuTreeView<T> ContextMenu(string contextMenu)
        {
            this.contextMenu = contextMenu;
            return this;
        }
        public MenuTreeView<T> showContextCreate(bool showCreate, string functionName)
        {
            this._cShowCreate = showCreate;
            this.cCreateFunctionName = functionName;
            return this;
        }
        public MenuTreeView<T> showContextEdit(bool showEdit, string functionName)
        {
            this._cShowEdit = showEdit;
            this.cEditFunctionName = functionName;
            return this;
        }
        public MenuTreeView<T> AjaxLoadContent(bool _ajaxLoadContent)
        {
            this._ajaxLoadContent = _ajaxLoadContent;
            return this;
        }
        public MenuTreeView<T> LanguageDirection(string langDirection)
        {
            this._langDirection = langDirection;
            return this;
        }

        public MenuTreeView<T> showContextRemove(bool showRemove, string functionName)
        {
            this._cShowRemove = showRemove;
            this.cRemoveFunctionName = functionName;
            return this;
        }
        public MenuTreeView<T> OnSelect(string onSelect)
        {
            this._onSelect = onSelect;
            return this;
        }
        public MenuTreeView<T> CheckedNodes(string checkedNodes)
        {
            this._checkedNodes = checkedNodes;
            return this;
        }
        /// <summary>
        /// The property which returns the children items
        /// </summary>
        public MenuTreeView<T> Children(Func<T, IEnumerable<T>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            _childrenProperty = selector;
            return this;
        }

        /// <summary>
        /// Content displayed if the list is empty
        /// </summary>
        public MenuTreeView<T> EmptyContent(string emptyContent)
        {
            if (emptyContent == null) throw new ArgumentNullException("emptyContent");
            _emptyContent = emptyContent;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public MenuTreeView<T> HtmlAttributes(object htmlAttributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public MenuTreeView<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) throw new ArgumentNullException("htmlAttributes");
            _htmlAttributes = htmlAttributes;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public MenuTreeView<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public MenuTreeView<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) throw new ArgumentNullException("htmlAttributes");
            _childHtmlAttributes = htmlAttributes;
            return this;
        }
        /// <summary>
        /// Tree Json Model
        /// </summary>
        /// <param name="TreeJsonModel"></param>
        /// <returns></returns>
        public MenuTreeView<T> TreeJsonModel(TreeJsonModel TreeJsonModel)
        {
            this._treeJsonModel = TreeJsonModel;
            return this;
        }
        /// <summary>
        /// convert tree json model to json
        /// </summary>
        /// <returns></returns>
        private string TreeJsonModelJson()
        {

            if (this._treeJsonModel != null)
            {
                var json = new JavaScriptSerializer().Serialize(_treeJsonModel.children);
                return json;
            }
            return null;
        }
        /// <summary>
        /// load tree json model for selected menu
        /// </summary>
        /// <param name="treeJsonModel"></param>
        /// <param name="parentMenuCode"></param>
        /// <returns></returns>
        private TreeJsonModel loadTreeJsonModelForSelectedMenu(TreeJsonModel treeJsonModel, string parentMenuCode)
        {

            string nodeCode = treeJsonModel.attr.Code;
            //selectedMenuExists = false;
            //if (parentMenuCode.ToString() == nodeCode)
            //{
            //    return treeJsonModel;
            //}
            //else
            //{
            foreach (var childTreeJsonModel in (List<TreeJsonModel>)treeJsonModel.children)
            {
                
                if (parentMenuCode.ToString() == nodeCode)
                {
                    return treeJsonModel;
                }
                var result = loadTreeJsonModelForSelectedMenu(childTreeJsonModel, parentMenuCode);
                if (result != null) return result;
            }
            //}
            return null;
        }
        private StringBuilder loadTreeJsonModelForSelectedMenu1(TreeJsonModel treeJsonModel, string parentMenuCode)
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<ul>");

            foreach (TreeJsonModel childTreeJsonModel in (List< TreeJsonModel>)treeJsonModel.children)
            {
                builder.Append("<li>").AppendLine(childTreeJsonModel.data.title);
                
                var result = loadTreeJsonModelForSelectedMenu1(childTreeJsonModel, parentMenuCode);
                builder.AppendLine(result.ToString());
                builder.AppendLine("</li>");
            }
            builder.AppendLine("</ul>");
            return builder;
        }
        /// <summary>
        /// menu json data
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        private string AccordionMenuJsonData(string menuCode,out bool isActiveAccordionMenu)
        {
            TreeJsonModel menuTreeJsonModel = loadTreeJsonModelForSelectedMenu(_treeJsonModel, menuCode);
            isActiveAccordionMenu = false;
            if (menuTreeJsonModel != null)
            {
                var json = new JavaScriptSerializer().Serialize(menuTreeJsonModel.children);
                if (_selectedMainMenuId != null)
                {
                    if (json.Contains("\"Code\":\"" + _selectedMainMenuId + "\"")) { isActiveAccordionMenu = true; }

                }
                return json;
            }
            return null;
        }
        private StringBuilder AccordionMenuJsonData1(string menuCode)
        {
            return loadTreeJsonModelForSelectedMenu1(_treeJsonModel, menuCode);
           
        }
        public string ToHtmlString()
        {
            return ToString();
        }

        public void Render()
        {

            var writer = _html.ViewContext.Writer;
            using (var textWriter = new HtmlTextWriter(writer))
            {
                textWriter.Write(ToString());
            }
        }


        /// <summary>
        /// menu tree to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            StringBuilder jsBuilder = new StringBuilder();

            jsBuilder.AppendLine(" <div id=\"MainMenuAccordionId\" style=\"min-height: 550px;width:100%;overflow:auto;font-family:B Nazanin\">");

            if (_treeJsonModel == null) return "";

            List<TreeJsonModel> rootMenus =  (List<TreeJsonModel>)_treeJsonModel.children;

            foreach (TreeJsonModel rootMenu in rootMenus)
            {
              jsBuilder.Append(" \t<h3>").Append(rootMenu.data.title).AppendLine("</h3>");
              jsBuilder.AppendLine(" \t\t<div class=\"MainMenuAccordion_Accordion\" style=\"min-height: 450px;padding:0;overflow:hidden;\">");
              jsBuilder.Append("\t\t\t<div id=\"").Append(rootMenu.attr.Code).Append("_MENU_TREE_ID").AppendLine("\" class=\"menuTreeDiv\" style=\"margin-bottom:20px\"></div>");
              jsBuilder.AppendLine(" \t\t</div>");
            }

           
            jsBuilder.AppendLine("</div>");
            jsBuilder.AppendLine("<script type=\"text/javascript\">");
            jsBuilder.AppendLine("\t$(function () {");
            bool isActiveAccordionMenu = false;
            int accordionMenuIndex = 0;
            int ActiveAccordionMenuIndex = 0;
             foreach (TreeJsonModel rootMenu in rootMenus)
            {
                
                 string data =   AccordionMenuJsonData(rootMenu.attr.Code,out isActiveAccordionMenu);
                 if (isActiveAccordionMenu)
                 {
                     ActiveAccordionMenuIndex = accordionMenuIndex;
                 }
                 jsBuilder.Append(generateJsTree(rootMenu.attr.Code+ "_MENU_TREE_ID", data));
                 accordionMenuIndex++;
             }
             jsBuilder.AppendLine("\t$(\"#MainMenuAccordionId\").accordion({");
            jsBuilder.AppendLine("\t\tanimated: false,");
            jsBuilder.AppendLine("\t\tcollapsible: true,");
            jsBuilder.AppendLine("\t\theightStyle: \"fill\",");
            jsBuilder.AppendLine("\t\tactive: "+ActiveAccordionMenuIndex);
            jsBuilder.AppendLine("\t});");
            jsBuilder.AppendLine("\t$(\"#MainMenuAccordionId h3 span.ui-icon\").attr('class', 'ui-icon ui-icon-triangle-1-w');");
            if (_selectedMainMenuId != null)
            {
                jsBuilder.Append("\t$('li[code=").Append(_selectedMainMenuId).Append("]').find('a').attr('class','jstree-hovered'); ");
            }

            jsBuilder.AppendLine("});");
            jsBuilder.AppendLine("</script>");
            return jsBuilder.ToString();
        }
        /// <summary>
        /// generate Js Tree
        /// </summary>
        /// <returns></returns>
        private string generateJsTree(string MenuTreeId, string menuData)
        {

            StringBuilder jsBuilder = new StringBuilder();
            jsBuilder.AppendLine("\t\t$('#" + MenuTreeId + "').jstree({");
          // jsBuilder.Append("\t\t\t\"plugins\": [\"themes\", \"json_data\",\"crrm\", \"contextmenu\",\"dnd\",\"ui\",\"cookies\"");
            jsBuilder.Append("\t\t\t\"plugins\": [\"themes\", \"ui\",\"json_data\", \"search\"");
            //jsBuilder.Append("\t\t\t\"plugins\": [\"themes\", \"ui\",\"json_data\", \"cookies\"");
            if (_isCheckbox)
            {
                jsBuilder.Append(",\"checkbox\"");
            }
            if (_showContextMenu)
            {
                jsBuilder.Append(", \"contextmenu\"");
            }
            if (_showCRRM)
            {
                jsBuilder.Append(",\"crrm\",\"dnd\"");
            }
            else
            {
               // jsBuilder.Append(",\"sort\"");
            }
           jsBuilder.AppendLine("]");
           if (_isCheckboxTwoState)
           {
               jsBuilder.AppendLine("\t\t\t,\"checkbox\": { \"two_state\": true }");
           }
            jsBuilder.AppendLine("\t\t\t,\"json_data\" : {");
            jsBuilder.Append("\t\t\t\t\"data\" : [");
            jsBuilder.Append(menuData);
            jsBuilder.AppendLine("]");
            jsBuilder.AppendLine("\t\t\t\t\t}");
            if (_langDirection == "rtl")
            {
                jsBuilder.AppendLine("\t\t\t\t\t,\"themes\": {");
                jsBuilder.AppendLine("\t\t\t\t\t\t\"theme\": \"default-rtl\"");
                jsBuilder.AppendLine("\t\t\t\t\t}");
            }

            if (_showCRRM)
            {
                jsBuilder.AppendLine("\t\t\t\t\t, \"crrm\": {");
                jsBuilder.AppendLine("\t\t\t\t\t\t \"move\": {");
                jsBuilder.AppendLine("\t\t\t\t\t\t\t \"check_move\": function (m) {");
                jsBuilder.AppendLine("\t\t\t\t\t\t\t\t return true;");
                jsBuilder.AppendLine("\t\t\t\t\t\t\t }");
                jsBuilder.AppendLine("\t\t\t\t\t\t }");
                jsBuilder.AppendLine("\t\t\t\t\t }");

            }
            jsBuilder.AppendLine("\t\t\t\t\t,\"ui\" : {");
            jsBuilder.AppendLine("\t\t\t\t\t\t\"select_limit\" : 1");
            //jsBuilder.AppendLine("\t\t\t\t\t\t\"selected_parent_close\" : \"select_parent\"");
            jsBuilder.AppendLine("\t\t\t\t\t}");
            if (_langDirection == "rtl")
            {
                jsBuilder.Append("\t\t\t\t\t,\"core\": { rtl: true }");
                //if (!_ajaxLoadContent)
                //{
                //    jsBuilder.Append(",\"open_parents\": true,\"initially_open\": [\"99\"]");
                //}
                //jsBuilder.Append("}");
            }
            if (_showContextMenu)
            {
                jsBuilder.AppendLine("\t\t\t\t\t,\"contextmenu\": {");
                if (contextMenu != null && !"".Equals(contextMenu))
                {
                    jsBuilder.AppendLine("items: " + contextMenu);
                }
                else
                {
                    jsBuilder.AppendLine("items: function(node){");
                    jsBuilder.AppendLine("return{");
                    if (_cShowCreate)
                    {
                        jsBuilder.AppendLine("\"create\": {");
                        jsBuilder.AppendLine(" \"label\": \"ایجاد\",");
                        jsBuilder.AppendLine("\"icon\": \"../../Images/TreeImage/add.gif\",");
                        jsBuilder.AppendLine("\"action\": function (obj) {\n"
                             + cCreateFunctionName + "(node);\n "
                           + "}\n"
                            //"_disabled": function (obj) { alert("obj=" + obj); return "default" != obj.attr('rel'); }
                     + "},\n");
                    }
                    if (_cShowEdit)
                    {
                        jsBuilder.AppendLine("\"Edit\": {\n" +
                            // The item label
                        "\"label\": \"ویرایش\",\n" +
                        "\"icon\": \"../../Images/TreeImage/edit.gif\",\n" +
                            // The function to execute upon a click+
                        "\"action\": function (obj) { " + cEditFunctionName + "(node); }\n" +
                            //"_disabled": function (obj) { alert("obj=" + obj); return "default" != obj.attr('rel'); }
                     "},\n");
                    }
                    if (_cShowRemove)
                    {
                        jsBuilder.AppendLine("\"Delete\": {\n" +
                            // The item label
                        "\"label\": \"حذف\",\n" +
                        "\"icon\": \"../../Images/TreeImage/delete.gif\",\n" +
                            // The function to execute upon a click
                        "\"action\": function (obj) { " + cRemoveFunctionName + "(node); }\n" +
                            //"_disabled": function (obj) { alert("obj=" + obj); return "default" != obj.attr('rel'); }
                     "}\n");
                    }
                    jsBuilder.AppendLine("}");
                    jsBuilder.AppendLine("}");
                }
                jsBuilder.AppendLine("}");
            }

            jsBuilder.AppendLine("\t\t});");

            jsBuilder.AppendLine("\t\t$('#" + MenuTreeId + "').bind('select_node.jstree',function(e, data){");
            
            jsBuilder.AppendLine("\t\t\tvar href = data.rslt.obj.children(\"a\").attr(\"href\");");
            jsBuilder.AppendLine("\t\t\tif (href != null && href != '' && href != '#' && href.indexOf('/?SelectedMainMenuId') == -1) {");
            if (_ajaxLoadContent)
            {
                jsBuilder.AppendLine("\t\t\t\t\t$(\"#content\").load(href);");
            }
            else
            {
                //jsBuilder.AppendLine("\t\t\t\t\t$('#SelectedMainMenuId').val('333');");
                jsBuilder.AppendLine("\t\t\t\t\t document.location.href = href; ");
                jsBuilder.AppendLine("\t\t\t\t\t return false; ");
            }
            jsBuilder.AppendLine("\t\t\t}else{");
            jsBuilder.AppendLine("\t\t\t\t\treturn data.inst.toggle_node(data.rslt.obj);");//return data.instance.toggle_node(data.node);>3
            jsBuilder.AppendLine("\t\t\t\t}");
            //jsBuilder.AppendLine("\t\t\tvar tree = jQuery.jstree._reference('#" + MenuTreeId + "');");
            //jsBuilder.AppendLine("\t\t\ttree.refresh();");
            jsBuilder.AppendLine("\t\t});");
            jsBuilder.AppendLine("\t$('#" + MenuTreeId + "').css('background-color', 'white');");
            //jsBuilder.AppendLine("\t$('#" + MenuTreeId + " a').css('line-height', '46px');");
            //jsBuilder.AppendLine("\t$('#" + MenuTreeId + " a').css('height', '46px');");
            jsBuilder.Append("\t$('#" + MenuTreeId + "').on('loaded.jstree', function () {  $(this).jstree('open_all');");//$(this).jstree('open_all');
            if (_selectedMainMenuId != null)
            {
                jsBuilder.Append("\t$('a[href*=\"?SelectedMainMenuId=").Append(_selectedMainMenuId).Append("\"]').attr('class','jstree-clicked'); ");
            }
            jsBuilder.AppendLine("});");
            return jsBuilder.ToString();
        }
        private void AppendChildren(TagBuilder parentTag, T parentItem, Func<T, IEnumerable<T>> childrenProperty)
        {
            if (childrenProperty(parentItem) == null)
            {
                return;
            }
            var children = childrenProperty(parentItem).ToList();
            if (children.Count() == 0)
            {
                return;
            }

            var innerUl = new TagBuilder("ul");
            innerUl.MergeAttributes(_childHtmlAttributes);

            foreach (var item in children)
            {
                BuildNestedTag(innerUl, item, childrenProperty);
            }

            parentTag.InnerHtml += innerUl.ToString();
        }

        private void BuildNestedTag(TagBuilder parentTag, T parentItem, Func<T, IEnumerable<T>> childrenProperty)
        {
            var li = GetLi(parentItem);
            parentTag.InnerHtml += li.ToString(TagRenderMode.StartTag);
            AppendChildren(li, parentItem, childrenProperty);
            parentTag.InnerHtml += li.InnerHtml + li.ToString(TagRenderMode.EndTag);
        }

        private TagBuilder GetLi(T item)
        {
            var li = new TagBuilder("li")
            {
                InnerHtml = _itemTemplate(item).ToHtmlString()
            };

            return li;
        }


    }

}
