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
    public static class TreeView1Helper
    {
        /// <summary>
        /// Create an HTML tree from a recursive collection of items
        /// </summary>
        public static TreeView1<T> TreeView1<T>(this HtmlHelper html, IEnumerable<T> items, string TreeId, string MemberValue)
        {
            return new TreeView1<T>(html, TreeId, MemberValue, items);
        }
    }


    /// <summary>
    /// Create an HTML tree from a resursive collection of items
    /// </summary>
    public class TreeView1<T> : IHtmlString
    {
        private readonly HtmlHelper _html;
        private readonly IEnumerable<T> _items = Enumerable.Empty<T>();
        private Func<T, string> _displayProperty = item => item.ToString();
        private Func<T, IEnumerable<T>> _childrenProperty;
        private string _emptyContent = "No children";
        private bool _cShowCreate = false;
        private string cCreateFunctionName = "";
        private bool _cShowEdit = false;
        private bool _cShowGroupCheck = false;
        private bool _showContextMenu = false;
        private string cEditFunctionName = "";
        private bool _isAjax = false;
        private string _groupCheck = "";
        private bool _allItemsLoad = false;
        private string ajaxUrl = "";
        private bool _cShowRemove = false;
        private bool _cSearchable = false;
        private bool _cSearchShowLabel = false;
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

        public TreeView1(HtmlHelper html, string TreeId, string MemberValue, IEnumerable<T> items)
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
        public TreeView1<T> ItemText(Func<T, string> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            _displayProperty = selector;
            return this;
        }


        /// <summary>
        /// The template used to render each item in the tree view
        /// </summary>
        public TreeView1<T> ItemTemplate(Func<T, HelperResult> itemTemplate)
        {
            if (itemTemplate == null) throw new ArgumentNullException("itemTemplate");
            _itemTemplate = itemTemplate;
            return this;
        }
        public TreeView1<T> ShowContextMenu(bool showContextMenu)
        {
            this._showContextMenu = showContextMenu;
            return this;
        }
        public TreeView1<T> ShowCheckbox(bool showCheckbox)
        {
            this._isCheckbox = showCheckbox;
            this._isCheckboxTwoState = false;
            return this;
        }
        public TreeView1<T> ShowCheckbox(bool showCheckbox, bool isTwoState)
        {
            this._isCheckbox = showCheckbox;
            this._isCheckboxTwoState = isTwoState;
            return this;
        }
        public TreeView1<T> OpenAll(bool openAll)
        {
            this._openAll = openAll;
            return this;
        }
        public TreeView1<T> ShowCRRM(bool showCRRM)
        {
            this._showCRRM = showCRRM;
            return this;
        }
        public TreeView1<T> Sortable(bool sortable)
        {
            this._sortable = sortable;
            return this;
        }
        public TreeView1<T> ContextMenu(string contextMenu)
        {
            this.contextMenu = contextMenu;
            return this;
        }
        public TreeView1<T> showContextCreate(bool showCreate, string functionName)
        {
            this._cShowCreate = showCreate;
            this.cCreateFunctionName = functionName;
            return this;
        }
        public TreeView1<T> showContextEdit(bool showEdit, string functionName)
        {
            this._cShowEdit = showEdit;
            this.cEditFunctionName = functionName;
            return this;
        }
        public TreeView1<T> showGroupCheck(bool showGroupCheck)
        {
            this._cShowGroupCheck = showGroupCheck;
            return this;
        }
        public TreeView1<T> LanguageDirection(string langDirection)
        {
            this._langDirection = langDirection;
            return this;
        }
        public TreeView1<T> IsAjax()
        {
            this._isAjax = true;
            return this;
        }
        public TreeView1<T> setGroupCheck(string checkBoxId)
        {
            this._groupCheck = checkBoxId;
            return this;
        }
        public TreeView1<T> AjaxUrl(string AjaxUrl)
        {
            this.ajaxUrl = AjaxUrl;
            this._allItemsLoad = false;
            return this;
        }
        public TreeView1<T> AjaxUrl(string AjaxUrl, bool allItemLoad)
        {
            this.ajaxUrl = AjaxUrl;
            this._allItemsLoad = allItemLoad;
            return this;
        }
        public TreeView1<T> showContextRemove(bool showRemove, string functionName)
        {
            this._cShowRemove = showRemove;
            this.cRemoveFunctionName = functionName;
            return this;
        }
        public TreeView1<T> SetSearchable(bool searchable, bool showLabel = false)
        {
            this._cSearchable = searchable;
            this._cSearchShowLabel = showLabel;
            return this;
        }
        public TreeView1<T> OnSelect(string onSelect)
        {
            this._onSelect = onSelect;
            return this;
        }
        public TreeView1<T> CheckedNodes(string checkedNodes)
        {
            this._checkedNodes = checkedNodes;
            return this;
        }
        /// <summary>
        /// The property which returns the children items
        /// </summary>
        public TreeView1<T> Children(Func<T, IEnumerable<T>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            _childrenProperty = selector;
            return this;
        }

        /// <summary>
        /// Content displayed if the list is empty
        /// </summary>
        public TreeView1<T> EmptyContent(string emptyContent)
        {
            if (emptyContent == null) throw new ArgumentNullException("emptyContent");
            _emptyContent = emptyContent;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView1<T> HtmlAttributes(object htmlAttributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView1<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) throw new ArgumentNullException("htmlAttributes");
            _htmlAttributes = htmlAttributes;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView1<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView1<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
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
        public TreeView1<T> showContextMenu1(bool showMenu1, string functionName1, string TitleShowMenu1)
        {
            this._cShowMenu1 = showMenu1;
            this.cMenu1FunctionName = functionName1;
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
        public TreeView1<T> showContextMenu2(bool showMenu2, string functionName2, string TitleShowMenu2)
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
            jsBuilder.AppendFormat(@"
                function _Cascade{0}(node) {{
                        this._changeStateProcessActive = true;
                        var tree_instance = $(""#{0}"");
                        if (node.state.selected) {{
                            function recursivecheckChilds(node) {{
                                var childs = node.children;
                                for (var i = 0; childs != undefined && i < childs.length; i++) {{
                                    recursivecheckChilds(tree_instance.jstree().get_node(childs[i]));
                                    tree_instance.jstree('select_node', childs[i]);
                                    //tree_instance.uncheck_node(childs[i]);
                                }}
                            }}

                            recursivecheckChilds(node);
                            //recursiveUncheckParents(tree_instance._get_parent(node));
                            //tree_instance.jstree().close_node(node);
                        }} else {{
                            function recursiveUncheckChilds(node) {{
                                var childs = node.children;
                                for (var i = 0; childs != undefined && i < childs.length; i++) {{
                                    recursiveUncheckChilds(tree_instance.jstree().get_node(childs[i]));
                                    tree_instance.jstree('deselect_node', childs[i]);
                                    //tree_instance.uncheck_node(childs[i]);
                                }}
                            }}

                            recursiveUncheckChilds(node);
                        }}
                        this._changeStateProcessActive = null;
                }}", this.TreeId).AppendLine();
            jsBuilder.AppendLine("\t$(function () {");
            if (_cSearchable)
            {
                jsBuilder.AppendLine("\t\t$('#" + TreeId + "').before('<div id=\"" + TreeId + "_Before\" style=\"margin:5px;\" > </div>');");
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
                jsBuilder.AppendLine("\t\t\t,\"checkbox\": { " +
                                     "\"three_state\" : false, // to avoid that fact that checking a node also check others\n}");
            }



            if (_isAjax)
            {
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
            //jsBuilder.AppendLine("\t\t\t\t\t,\"ui\" : {");
            //jsBuilder.AppendLine("\t\t\t\t\t\t\"select_limit\" : 1,");
            //jsBuilder.AppendLine("\t\t\t\t\t\t\"selected_parent_close\" : \"select_parent\"");
            //jsBuilder.AppendLine("\t\t\t\t\t}");


            jsBuilder.AppendLine("\t\t\t\t\t,\"core\": {");
            if (_langDirection == "rtl")
            {
                jsBuilder.AppendLine("\t\t\t\t\t rtl: true ,");
            }
            if (_isAjax)
            {
                ajaxUrl += ajaxUrl.IndexOf("?") >= 0 ? "&" : "?";
                jsBuilder.AppendLine("\t\t\t\t\t 'data' : {");
                if (!_allItemsLoad)
                    jsBuilder.AppendLine("\t\t\t\t\t\t 'url' :  function (node) {return '" + ajaxUrl + "'+ 'LazyLoading=true&ParentId=' + node.id},");
                else
                    jsBuilder.AppendLine("\t\t\t\t\t\t 'url' : '" + ajaxUrl + "LazyLoading=false',");

                //jsBuilder.AppendLine("\t\t\t\t\t\t 'url' : function (node) {");
                //jsBuilder.AppendLine("\t\t\t\t\t\t\t console.log('nnodeid='+node.id); return node.id == '#' ? ");
                //jsBuilder.AppendLine("\t\t\t\t\t\t\t '" + ajaxUrl+"'");
                //jsBuilder.AppendLine("\t\t\t\t\t\t\t : '" + ajaxUrl + "'");
                //jsBuilder.AppendLine("\t\t\t\t\t\t}, ");
                jsBuilder.AppendLine("\t\t\t\t\t\t'data' : function (node) {");
                jsBuilder.AppendLine("\t\t\t\t\t\t\treturn { 'id' : (node.id == null || node.id=='#'? null:node.id) }; ");
                jsBuilder.AppendLine("\t\t\t\t\t\t} ");
                jsBuilder.AppendLine("\t\t\t\t\t} ");
            }
            jsBuilder.AppendLine("\t\t\t\t\t\t}");
            //اگر سورت فالس باشد بر اساس متن سورت شود
            //if (!_sortable)
            //{
            //    jsBuilder.AppendLine("\t\t\t\t\t,\"sort\" : function(a, b) {")
            //        .AppendLine("\t\t\t\t\t\ta1 = this.get_node(a);")
            //        .AppendLine("\t\t\t\t\t\tb1 = this.get_node(b);")
            //        .AppendLine("\t\t\t\t\t\tif (a1.icon == b1.icon)")
            //        .AppendLine("\t\t\t\t\t\t{")
            //            .AppendLine("\t\t\t\t\t\t\treturn (a1.text > b1.text) ? 1 : -1;")
            //        .AppendLine("\t\t\t\t\t\t}")
            //        .AppendLine("\t\t\t\t\t\telse")
            //        .AppendLine("\t\t\t\t\t\t{")
            //            .AppendLine("\t\t\t\t\t\t\treturn (a1.icon > b1.icon) ? 1 : -1;")
            //        .AppendLine("\t\t\t\t\t\t}")
            //    .AppendLine("\t\t\t\t\t}");
            //}

            if (_showContextMenu)
            {
                jsBuilder.AppendLine("\t\t\t\t\t,\"contextmenu\": {'select_node': false,");
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
                        jsBuilder.AppendLine("\"icon\": \"../../Images/TreeImage/add.png\",");
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
                        "\"icon\": \"../../Images/TreeImage/edit.png\",\n" +
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
                        "\"icon\": \"../../Images/TreeImage/delete.png\",\n" +
                        // The function to execute upon a click
                        "\"action\": function (obj) { " + cRemoveFunctionName + "(node); }\n" +
                     //"_disabled": function (obj) { alert("obj=" + obj); return "default" != obj.attr('rel'); }
                     "},\n");
                    }
                    if (_cShowMenu1)
                    {
                        jsBuilder.AppendLine("\"Menu1\": {");
                        jsBuilder.AppendLine(" \"label\": \"" + cMenu1Title + "\",");
                        jsBuilder.AppendLine("\"icon\": \"../../Images/TreeImage/new_root.png\",\n");
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
                    if (_cShowGroupCheck)
                    {
                        jsBuilder.AppendFormat("\"GroupCheck\": {{\n" +
                        "\"label\": \"انتخاب گروهی\",\n" +
                        @"""action"": function (obj) {{ 
                            $(""#{0}"").jstree('select_node', node.id);
                            _Cascade{0}(node);" +
                        "}}\n" +
                     "}},\n", this.TreeId).AppendLine();

                        jsBuilder.AppendFormat("\"GroupUnCheck\": {{\n" +
                        "\"label\": \"لغو انتخاب گروهی\",\n" +
                        @"""action"": function (obj) {{ 
                            $(""#{0}"").jstree('deselect_node', node.id);
                            _Cascade{0}(node);" +
                        "}}\n" +
                     //"_disabled": function (obj) { alert("obj=" + obj); return "default" != obj.attr('rel'); }
                     "}},\n", this.TreeId).AppendLine();
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
                jsBuilder.AppendLine("\t\t\tvar tree = $.jstree.reference(\"#" + TreeId + "\");\n");
                jsBuilder.AppendLine("\t\t\tvar currentNode = tree.get_node(null, false);\n");
                //jsBuilder.AppendLine("console.log('curent node = '+currentNode['code']);\n");
                if (!string.IsNullOrEmpty(_onSelect))
                {
                    jsBuilder.Append("\t\t\t")
                        .Append(_onSelect)
                        .Append("(currentNode.attr('")
                        .Append(MemberValue)
                        .AppendLine("'));");
                }
                jsBuilder.AppendLine("\t\t\ttree.refresh(currentNode);\n");
                jsBuilder.AppendLine("\t\t});\n");
            }
            else
            {
                jsBuilder.Append("$('#" + TreeId + "').on('select_node.jstree', function (e, data) { ");
                jsBuilder.Append(_onSelect + "(e,data);});");
            }
            if (_openAll)
            {
                jsBuilder.AppendLine("\t\t$('#" + TreeId + "').bind('loaded.jstree', function (event, data) {\n");
                jsBuilder.AppendLine("\t\t\t$(this).jstree('open_all'); \n");
                jsBuilder.AppendLine("\t\t});\n");
            }
            if (_cSearchable)
            {
                jsBuilder.Append("$('#").Append(TreeId).Append("_Before").Append("').prepend('");
                if (_cSearchShowLabel)
                {
                    jsBuilder.Append("<label for=\"").Append(TreeId).Append("_search").Append("\">جستجو </label>");
                }
                jsBuilder.Append("<input type=\"text\" style=\"margin-right:5px;\" class=\"searchable\" id=\"").Append(TreeId).Append("_search").AppendLine("\">');");

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
            if (!string.IsNullOrEmpty(_groupCheck))
            {
                //jsBuilder.AppendLine("\t\t$('#" + TreeId + "').on('select_node.jstree', function (event, data) {\n");
                //jsBuilder.AppendLine("if($('#" + _groupCheck + "').is(\":checked\")){");
                //jsBuilder.AppendLine("if(!data.node.state.checked){");
                //jsBuilder.AppendLine("\t\t\t$('#" + TreeId + "').jstree('check_node', data.node);" +
                //                     "for(var i=0;i < data.node.children.length;i++)" +
                //                     "$('#" + TreeId + "').jstree('check_node', data.node.children[i]);\n");
                //jsBuilder.AppendLine("\t\t}else{\n");
                //jsBuilder.AppendLine("if(!data.node.state.checked){");
                //jsBuilder.AppendLine("\t\t\t$('#" + TreeId + "').jstree('uncheck_node', data.node);" +
                //                     "for(var i=0;i < data.node.children.length;i++)" +
                //                     "$('#" + TreeId + "').jstree('uncheck_node', data.node.children[i]);\n");

                //jsBuilder.AppendLine("\t\t}}}else{");

                //jsBuilder.AppendLine("if(!data.node.state.checked){");
                //jsBuilder.AppendLine("\t\t\t$('#" + TreeId + "').jstree('check_node', data.node);");

                //jsBuilder.AppendLine("\t\t}else{\n");
                //jsBuilder.AppendLine("if(!data.node.state.checked){");
                //jsBuilder.AppendLine("\t\t\t$('#" + TreeId + "').jstree('uncheck_node', data.node);");

                //jsBuilder.AppendLine("}}}});\n");

                //jsBuilder.AppendLine("\t\t$('#" + TreeId + "').on('check_node.jstree', function (event, data) {\n");
                //jsBuilder.AppendLine("if($('#" + _groupCheck + "').is(\":checked\")){");
                //jsBuilder.AppendLine("\t\t\t$('#" + TreeId + "').jstree('check_node', data.node);" +
                //                     "for(var i=0;i < data.node.children.length;i++)" +
                //                     "$('#" + TreeId + "').jstree('check_node', data.node.children[i]);\n");
                //jsBuilder.AppendLine("\t\t}});\n");

                //jsBuilder.AppendLine("\t\t$('#" + TreeId + "').on('uncheck_node.jstree', function (event, data) {\n");
                //jsBuilder.AppendLine("if($('#" + _groupCheck + "').is(\":checked\")){");
                //jsBuilder.AppendLine("\t\t\t$('#" + TreeId + "').jstree('uncheck_node', data.node);" +
                //                     "for(var i=0;i < data.node.children.length;i++)" +
                //                     "$('#" + TreeId + "').jstree('uncheck_node', data.node.children[i]);\n");
                //jsBuilder.AppendLine("\t\t}});\n");
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
