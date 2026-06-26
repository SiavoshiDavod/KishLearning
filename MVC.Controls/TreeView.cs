using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.WebPages;
using System.Web.UI;

namespace MVC.Controls
{
    public static class TreeViewHelper
    {
        /// <summary>
        /// Create an HTML tree from a recursive collection of items
        /// </summary>
        public static TreeView<T> TreeView<T>(this HtmlHelper html, IEnumerable<T> items, string TreeId, string MemberValue)
        {
            return new TreeView<T>(html, TreeId, MemberValue, items);
        }
    }


    /// <summary>
    /// Create an HTML tree from a resursive collection of items
    /// </summary>
    public class TreeView<T> : IHtmlString
    {
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
        private bool _isAjax = false;
        private bool _allItemsLoad = false;
        private string ajaxUrl = "";
        private bool _cShowRemove = false;
        private bool _cSearchable = false;
        private string cRemoveFunctionName = "";
        private string _onSelect = "";
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();
        private IDictionary<string, object> _childHtmlAttributes = new Dictionary<string, object>();
        private Func<T, HelperResult> _itemTemplate;
        private string TreeId;
        private string _langDirection = "rtl";
        private string MemberValue;
        private string contextMenu;
        private bool _isCheckbox = false;
        private bool _isCheckboxTwoState = false;
        private bool _showCRRM = false;
        private bool _sortable = true;
        private string _checkedNodes;
        private bool _cShowMenu1 = false;
        private string cMenu1FunctionName = "";
        private string cMenu1Title = "";
        private bool _cShowMenu2 = false;
        private string cMenu2FunctionName = "";
        private string cMenu2Title = "";
        private bool _openAll = false;

        public TreeView(HtmlHelper html, string TreeId, string MemberValue, IEnumerable<T> items)
        {
            if (html == null) throw new ArgumentNullException("html");
            _html = html;
            _items = items;
            this.MemberValue = MemberValue;
            this.TreeId = TreeId;
            // The ItemTemplate will default to rendering the DisplayProperty
            _itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }

        /// <summary>
        /// The property which will display the text rendered for each item
        /// </summary>
        public TreeView<T> ItemText(Func<T, string> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            _displayProperty = selector;
            return this;
        }


        /// <summary>
        /// The template used to render each item in the tree view
        /// </summary>
        public TreeView<T> ItemTemplate(Func<T, HelperResult> itemTemplate)
        {
            if (itemTemplate == null) throw new ArgumentNullException("itemTemplate");
            _itemTemplate = itemTemplate;
            return this;
        }
        public TreeView<T> ShowContextMenu(bool showContextMenu)
        {
            this._showContextMenu = showContextMenu;
            return this;
        }
        public TreeView<T> ShowCheckbox(bool showCheckbox)
        {
            this._isCheckbox = showCheckbox;
            this._isCheckboxTwoState = false;
            return this;
        }
        public TreeView<T> ShowCheckbox(bool showCheckbox, bool isTwoState)
        {
            this._isCheckbox = showCheckbox;
            this._isCheckboxTwoState = isTwoState;
            return this;
        }
        public TreeView<T> OpenAll(bool openAll)
        {
            this._openAll = openAll;
            return this;
        }
        public TreeView<T> ShowCRRM(bool showCRRM)
        {
            this._showCRRM = showCRRM;
            return this;
        }
        public TreeView<T> Sortable(bool sortable)
        {
            this._sortable = sortable;
            return this;
        }
        public TreeView<T> ContextMenu(string contextMenu)
        {
            this.contextMenu = contextMenu;
            return this;
        }
        public TreeView<T> showContextCreate(bool showCreate, string functionName)
        {
            this._cShowCreate = showCreate;
            this.cCreateFunctionName = functionName;
            return this;
        }
        public TreeView<T> showContextEdit(bool showEdit, string functionName)
        {
            this._cShowEdit = showEdit;
            this.cEditFunctionName = functionName;
            return this;
        }
        public TreeView<T> LanguageDirection(string langDirection)
        {
            this._langDirection = langDirection;
            return this;
        }
        public TreeView<T> IsAjax()
        {
            this._isAjax = true;
            return this;
        }
        public TreeView<T> AjaxUrl(string AjaxUrl)
        {
            this.ajaxUrl = AjaxUrl;
            this._allItemsLoad = false;
            return this;
        }
        public TreeView<T> AjaxUrl(string AjaxUrl, bool allItemLoad)
        {
            this.ajaxUrl = AjaxUrl;
            this._allItemsLoad = allItemLoad;
            return this;
        }
        public TreeView<T> showContextRemove(bool showRemove, string functionName)
        {
            this._cShowRemove = showRemove;
            this.cRemoveFunctionName = functionName;
            return this;
        }
        public TreeView<T> SetSearchable(bool searchable)
        {
            this._cSearchable = searchable;
            return this;
        }
        public TreeView<T> OnSelect(string onSelect)
        {
            this._onSelect = onSelect;
            return this;
        }
        public TreeView<T> CheckedNodes(string checkedNodes)
        {
            this._checkedNodes = checkedNodes;
            return this;
        }
        /// <summary>
        /// The property which returns the children items
        /// </summary>
        public TreeView<T> Children(Func<T, IEnumerable<T>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            _childrenProperty = selector;
            return this;
        }

        /// <summary>
        /// Content displayed if the list is empty
        /// </summary>
        public TreeView<T> EmptyContent(string emptyContent)
        {
            if (emptyContent == null) throw new ArgumentNullException("emptyContent");
            _emptyContent = emptyContent;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView<T> HtmlAttributes(object htmlAttributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) throw new ArgumentNullException("htmlAttributes");
            _htmlAttributes = htmlAttributes;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) throw new ArgumentNullException("htmlAttributes");
            _childHtmlAttributes = htmlAttributes;
            return this;
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

        private void ValidateSettings()
        {
            //if (_childrenProperty == null)
            //{
            //    throw new InvalidOperationException("You must call the Children() method to tell the tree view how to find child items");
            //}
        }

        /// <summary>
        /// Show Context Menu 1
        /// </summary>
        /// <param name="showMenu1"></param>
        /// <param name="functionName1"></param>
        /// <param name="TitleShowMenu1"></param>
        /// <returns></returns>
        public TreeView<T> showContextMenu1(bool showMenu1, string functionName1,string TitleShowMenu1)
        {
            this._cShowMenu1 = showMenu1;
            this.cMenu1FunctionName  = functionName1;
            this.cMenu1Title = TitleShowMenu1;
            return this;
        }

        /// <summary>
        /// Show Context Menu 2
        /// </summary>
        /// <param name="showMenu2"></param>
        /// <param name="functionName2"></param>
        /// <param name="TitleShowMenu2"></param>
        /// <returns></returns>
        public TreeView<T> showContextMenu2(bool showMenu2, string functionName2, string TitleShowMenu2)
        {
            this._cShowMenu2 = showMenu2;
            this.cMenu2FunctionName = functionName2;
            this.cMenu2Title = TitleShowMenu2;
            return this;
        }

        public override string ToString()
        {
            ValidateSettings();

            StringBuilder jsBuilder = new StringBuilder();
            if (_items != null && _items.Count() > 0)
            {
                var listItems = _items.ToList();

                var ul = new TagBuilder("ul");
                ul.MergeAttributes(_htmlAttributes);

                if (listItems.Count == 0)
                {
                    var li = new TagBuilder("li")
                    {
                        InnerHtml = _emptyContent
                    };
                    ul.InnerHtml += li.ToString();
                }

                foreach (var item in listItems)
                {
                    BuildNestedTag(ul, item, _childrenProperty);
                }
                jsBuilder.Append(ul.ToString());
            }

            jsBuilder.AppendLine("<script type=\"text/javascript\">");
            jsBuilder.AppendLine("\t$(function () {");
            if (_cSearchable)
            {
                jsBuilder.AppendLine("\t\t$('#" + TreeId + "').before('<div id=\"" + TreeId + "_Before\" > </div>');");
            }
            jsBuilder.AppendLine("\t\t$('#" + TreeId + "').jstree({");
            jsBuilder.Append("\t\t\t\"plugins\": [\"themes\", \"ui\",\"json_data\", \"search\"");
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
            else if (_sortable)
            {
                jsBuilder.Append(",\"sort\"");
            }
            jsBuilder.AppendLine("]");
            if (_isCheckboxTwoState)
            {
                jsBuilder.AppendLine("\t\t\t,\"checkbox\": { \"two_state\": true }");
            }
            if (_isAjax)
            {
                jsBuilder.AppendLine("\t\t\t,\"json_data\" : {");
                jsBuilder.AppendLine("\t\t\t\t\"ajax\" : {");
                jsBuilder.AppendLine("\t\t\t\t\t\"type\": 'POST',");
                jsBuilder.AppendLine("\t\t\t\t\t\"data\" : {\"action\": 'getChildren'},");
                if (!_allItemsLoad)
                {
                    jsBuilder.AppendLine("\t\t\t\t\t\"url\": function (node) { ");
                    jsBuilder.AppendLine("\t\t\t\t\t\tvar " + MemberValue + " = (node == '-1')?\"\":node.attr('" + MemberValue + "');");
                    if (ajaxUrl.Split('?').Length > 1)
                    {
                        jsBuilder.AppendLine("\t\t\t\t\t\t\treturn '" + ajaxUrl.Split('?')[0] + "'+'?" + MemberValue + "=' + " + MemberValue + "+'&" + ajaxUrl.Split('?')[1].Split('=')[0] + "='" + (ajaxUrl.Split('?')[1].Split('=')[1] == "" ? ";}" : "+'" + ajaxUrl.Split('?')[1].Split('=')[1] + "';}"));
                    }
                    else
                        jsBuilder.AppendLine("\t\t\t\t\t\t\treturn '" + ajaxUrl + "'+'?" + MemberValue + "=' + " + MemberValue + ";}");
                }
                else
                {
                    jsBuilder.AppendLine("\t\t\t\t\t\"url\": \"" + ajaxUrl + "\"");
                }
                jsBuilder.AppendLine("\t\t\t\t\t,\"success\": function (new_data) {");
                jsBuilder.AppendLine("\t\t\t\t\t\tif (new_data != null && new_data.length > 0)");
                jsBuilder.AppendLine("\t\t\t\t\t\t\treturn new_data;");
                if (_items == null)
                    jsBuilder.AppendLine("\t\t\t\t\t\t\treturn [data =\"" + _emptyContent + "\"];}");
                else
                    jsBuilder.AppendLine("\t\t\t\t\t\t\treturn [data =\"\"];}");
                jsBuilder.AppendLine(" \t\t\t\t\t\t}");
                jsBuilder.AppendLine("\t\t\t\t\t}");
                if (_langDirection == "rtl")
                {
                    jsBuilder.AppendLine("\t\t\t\t\t,\"themes\": {");
                    jsBuilder.AppendLine("\t\t\t\t\t\t\"theme\": \"default-rtl\"");
                    jsBuilder.AppendLine("\t\t\t\t\t}");
                }
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
            jsBuilder.AppendLine("\t\t\t\t\t\t\"select_limit\" : 1,");
            jsBuilder.AppendLine("\t\t\t\t\t\t\"selected_parent_close\" : \"select_parent\"");
            jsBuilder.AppendLine("\t\t\t\t\t}");
            if (_langDirection == "rtl")
            {
                jsBuilder.AppendLine("\t\t\t\t\t,\"core\": { rtl: true }");
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
                    if (_cShowMenu1)
                    {
                        jsBuilder.AppendLine("\"Menu1\": {");
                        jsBuilder.AppendLine(" \"label\": \"" + cMenu1Title + "\",");
                        jsBuilder.AppendLine("\"action\": function (obj) {\n"
                             + cMenu1FunctionName + "(node);\n "
                           + "}\n"
                     + "},\n");
                    }
                    if (_cShowMenu2)
                    {
                        jsBuilder.AppendLine("\"Menu2\": {");
                        jsBuilder.AppendLine(" \"label\": \"" + cMenu2Title + "\",");
                        jsBuilder.AppendLine("\"action\": function (obj) {\n"
                             + cMenu2FunctionName + "(node);\n "
                           + "}\n"
                     + "},\n");
                    }
                    jsBuilder.AppendLine("}");
                    jsBuilder.AppendLine("}");
                }
                jsBuilder.AppendLine("}");
            }
            //if(!string.IsNullOrEmpty(_onSelect)){
            //    jsBuilder.AppendLine("\t\t\t,callback: {");
            //    jsBuilder.AppendLine("\t\t\t\tonclick: function(NODE, TREE_OBJ){");
            //    jsBuilder.Append("\t\t\t\t\t").Append(_onSelect).AppendLine("(NODE,TREE_OBJ);");
            //    jsBuilder.AppendLine("\t\t\t\t}");
            //    jsBuilder.AppendLine("\t\t\t}");
            //}
            jsBuilder.AppendLine("\t\t});");
            if (!_allItemsLoad)
            {
                jsBuilder.AppendLine("\t\t$('#" + TreeId + "').on('click','a',function(){\n");
                jsBuilder.AppendLine("\t\t\tvar tree = jQuery.jstree._reference(\"#" + TreeId + "\");\n");
                jsBuilder.AppendLine("\t\t\tvar currentNode = tree._get_node(null, false);\n");
                //jsBuilder.AppendLine("console.log('curent node = '+currentNode['code']);\n");
                if (!string.IsNullOrEmpty(_onSelect))
                {
                    jsBuilder.Append("\t\t\t").Append(_onSelect).Append("(currentNode.attr('").Append(MemberValue).AppendLine("'));");
                }
                jsBuilder.AppendLine("\t\t\ttree.refresh(currentNode);\n");
             jsBuilder.AppendLine("\t\t});\n");
            }
            if (_openAll)
            {
                jsBuilder.AppendLine("\t\t$('#" + TreeId + "').bind('loaded.jstree', function (event, data) {\n");
                jsBuilder.AppendLine("\t\t\t$(this).jstree('open_all'); \n");
                jsBuilder.AppendLine("\t\t});\n");
            }
            if (_cSearchable)
            {
                jsBuilder.Append("$('#").Append(TreeId).Append("_Before").Append("').prepend('<input type=\"text\" id=\"").Append(TreeId).Append("_search").AppendLine("\">');");
                jsBuilder.Append("var to").Append(TreeId).Append("_search").AppendLine(" = false;");
                jsBuilder.Append("$('#").Append(TreeId).Append("_search").AppendLine("').keyup(function () {");
                jsBuilder.Append("if(to").Append(TreeId).Append("_search").Append(") { clearTimeout(to").Append(TreeId).Append("_search").AppendLine("); }");
                jsBuilder.Append("to").Append(TreeId).Append("_search").AppendLine(" = setTimeout(function () {");
                jsBuilder.Append("var v = $('#").Append(TreeId).Append("_search").AppendLine("').val();");
                jsBuilder.Append("$('#").Append(TreeId).Append("').jstree(true).search(v);").AppendLine("");
                jsBuilder.AppendLine("}, 250);");
                jsBuilder.AppendLine("});");
            }
            if (!string.IsNullOrEmpty(_checkedNodes))
            {
                //jsBuilder.AppendLine("\t\t$('#SourceTree').find('li').each(function () {");
                //jsBuilder.AppendLine("\t\t\t\tconsole.log($(this).attr('code'));");
                //jsBuilder.AppendLine("\t\t\tif ($(this).attr('code') == '4') {");
                //jsBuilder.AppendLine("\t\t\t\tvar liClass = $(this).attr('class');");
                //jsBuilder.AppendLine("\t\t\t\tconsole.log('liclass=' + liClass);");
                //jsBuilder.AppendLine("\t\t\t\tliClass = liClass.replace('jstree-unchecked', 'jstree-checked');");
                //jsBuilder.AppendLine("\t\t\t\t$(this).attr('class', liClass);");
                //jsBuilder.AppendLine("\t\t\t}");
                //jsBuilder.AppendLine("\t\t});");
                
                ////jsBuilder.AppendLine("\t\t$('#SourceTree').find('li').each(function () {");

                //string[] checkedNodes = _checkedNodes.Split(',');
                //foreach (var item in checkedNodes)
                //{
                    
                //}
            }
            jsBuilder.AppendLine("});");
            jsBuilder.AppendLine("</script>");
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
