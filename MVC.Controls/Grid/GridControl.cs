using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Reflection;

namespace MVC.Controls.Grid
{
    public class GridControl
    {
        #region Constructor
        public GridControl()
        {
            this.isSubGrid = false;
            this.Pager = new GridPagerControl() { GridName = this.Name };
        }
        #endregion

        #region Property

        private string _listUrl, _editUrl, _caption;
        private string _listParams = null;
        private string _attributes = null;
        private string _onSelectedRow = null;
        private string _onSelectAllRow = null;
        private string _onSortCol = null;
        private string _onDblClickRow = null;
        private string _height = null, _width = null;
        private string _sortname = null;
        private bool _frozen = false;
        private string _sortorder = "asc";
        private bool _multisort = false;
        private string _onGridComplete = null;
        private string _onLoadComplete = null;
        private string _onAfterLoadComplete = null;
        private bool _altRows = true, _shrinkToFit, _forceFit = false;
        private string _altRowsClass = "jqgrid_alt_rows_class";
        private bool _multiSelect = false;
        private string _groupingFormat = null;
        private bool _editable = false;
        private bool _rowNumber = false;
        private string _rowNumberWidth = null;
        private bool _footerRow = false;
        private bool _loadonce = false;
        private bool _keepSelectedRowsOnPaging = false;
        private string _keepSelectedRowIds = null;
        // Azadi
        private int[] _rowList;
        private int? _rowNum;
        public bool IsFilterToolbar { get; set; }
        public FilterToolbar FilterToolbar { get; set; }


        //private bool _showCheckBox = false;
        private bool _multiboxonly = false;
        private int _pageSize;
        private bool? _pgButtons;
        private bool _isAutoSize, _isRowNumber;
        private string _languageDirection = "rtl";
        private string _languageCode = "fa";
        private GridControl _subGrid = null;
        private object _gridDataSource = null;
        private bool _renderEntireObject = true;
        private HttpVerbs _httpVerb = HttpVerbs.Get;
        private bool isSubGrid { get; set; }
        public string Name { get; set; }
        public IGridPagerControl Pager { get; set; }

        #endregion


        #region  #region Set Methods
        /// <summary>
        /// If grouping has been set, configures how the grouping is formatted
        /// </summary>
        /// <param name="groupColumnShow">Whether or not to show the grouped column. Default to yes</param>
        /// <param name="groupText">The text title of the group. e.g. {0}-{1} items</param>
        /// <param name="groupCollapse">Whether or not the groups are collapsed by default. Default to no</param>
        /// <param name="groupOrder">The group order. Default to asc</param>
        /// <returns></returns>
        public GridControl SetGroupFormatting(bool groupColumnShow = true, string groupText = "", bool groupCollapse = false, string groupOrder = "asc")
        {
            _groupingFormat = "groupColumnShow:[" + groupColumnShow.ToString().ToLower() + "]" +
                               ", groupText: [\"" + groupText + "\"], groupCollapse: " + groupCollapse.ToString().ToLower() +
                               ", groupOrder: ['" + groupOrder + "']";

            //_groupColumnShow = groupColumnShow;
            //_groupCollpse = groupCollapse;
            //_groupText = groupText;
            //_groupOrder = groupOrder;
            return this;
        }

        private List<GridColumnModel> _columns = new List<GridColumnModel>();

        /// <summary>
        /// Allows you to supply a function that determines which method is the primary key of the underlying object.
        /// This is used when determining if a columns should be used as a primary key. If you use another attribute
        /// than KeyAttribute, overriding this method will help you.
        /// </summary>
        public static Func<MemberInfo, bool> IsPrimaryKeyFunc { get; set; }

        public GridControl SetWidth(string val) { _width = val; return this; }
        public GridControl SetHeight(string val) { _height = val; return this; }
        public GridControl SetSortName(string val) { _sortname = val; return this; }
        public GridControl SetFrozen(bool val) { _frozen = val; return this; }
        public GridControl SetSortOrder(string val) { _sortorder = val; return this; }
        public GridControl SetMultiSort() { _multisort = true; return this; }
        /// <summary>
        /// Instead of setting a ListUrl with a controller url that will fetch the grid's data
        /// It is possible to give the grid it's data source statically thus reducing the amount of requests to the server
        /// </summary>
        /// <param name="gridData"></param>
        /// <param name="renderEntireObject">[Not yet fully supported] Whether or not to render the entire object, or only properties bound to a column</param>
        /// <returns></returns>
        public GridControl SetDataSource(object gridData, bool renderEntireObject = true)
        {
            _gridDataSource = gridData;
            _renderEntireObject = renderEntireObject;
            return this;
        }

        /// <summary>
        /// Sets a javascript function name
        /// That will raise when a row is selected
        /// </summary>
        /// Azadi
        public GridControl SetRowList(int[] rowList)
        {
            this._rowList = rowList;
            return this;
        }

        public GridControl SetRowNum(int rowNum)
        {
            this._rowNum = rowNum;
            return this;
        }
        public GridControl SetIsFilterToolbar(bool isFilterToolbar) { this.IsFilterToolbar = isFilterToolbar; return this; }
        public GridControl SetFilterToolbar(FilterToolbar filterToolbar)
        {
            if (filterToolbar != null)
            {
                this.IsFilterToolbar = true; this.FilterToolbar = filterToolbar;
            } return this;
        }

        public GridControl SetPgButton(bool pgButtons)
        {
            this._pgButtons = pgButtons;
            return this;
        }

        /// <summary>
        /// Set wether to show row numbers
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public GridControl SetIsRowNumber(bool rowNumber) { _isRowNumber = rowNumber; return this; }

        public GridControl SetOnSelectedRowEvent(string val) { _onSelectedRow = val; return this; }
        public GridControl SetOnSelectAllRowEvent(string val) { _onSelectAllRow = val; return this; }
        public GridControl SetOnDblClickRowEvent(string val) { _onDblClickRow = val; return this; }
        public GridControl SetOnGridCompleteEvent(string val) { _onGridComplete = val; return this; }
        public GridControl SetOnLoadCompleteEvent(string val) { _onLoadComplete = val; return this; }
        public GridControl SetOnAfterLoadCompleteEvent(string val) { _onAfterLoadComplete = val; return this; }
        //public GridControl SetOnLoadKeepSelectedRows(string val) { _loadKeepSelectedRows = val; return this; }

        /// <summary>
        /// Sets the title of the grid
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public GridControl SetCaption(string caption) { _caption = caption; return this; }
        /// <summary>
        /// Set shrink to fit
        /// </summary>
        /// <param name="shrink"></param>
        /// <returns></returns>
        public GridControl SetShrinkToFit(bool shrink) { _shrinkToFit = shrink; return this; }
        public GridControl SetForceFit(bool forceFit) { _forceFit = forceFit; return this; }
        /// <summary>
        /// Set edit Cell
        /// </summary>
        /// <param name="edit"></param>
        /// <returns></returns>
        public GridControl SetEditRecord(bool edit) { _editable = edit; return this; }
        /// <summary>
        /// Set Row Number
        /// </summary>
        /// <returns></returns>
        public GridControl SetRowNumber(string rowNumWidth = null) { _rowNumber = true; if (rowNumWidth != null) _rowNumberWidth = rowNumWidth; return this; }
        /// <summary>
        /// set footer row
        /// </summary>
        /// <param name="footerRow"></param>
        /// <returns></returns>
        public GridControl SetFooterRow(bool footerRow) { _footerRow = footerRow; return this; }
        /// <summary>
        /// set Load Once
        /// </summary>
        /// <returns></returns>
        public GridControl SetLoadOnce() { _loadonce = true; return this; }
        public GridControl SetKeepSelectedRows(bool? keepSelectedRowsOnPaging=null) { _keepSelectedRowsOnPaging =(keepSelectedRowsOnPaging!=null? keepSelectedRowsOnPaging.Value: true); return this; }
        public GridControl SetKeepSelectedRowIds(string keepSelectedRowIds)
        {
            _keepSelectedRowsOnPaging = true;
            _keepSelectedRowIds = keepSelectedRowIds;
            return this;
        }
        public bool GetKeepSelectedRows() { return this._keepSelectedRowsOnPaging; }
        public string GetKeepSelectedRowIds() { return this._keepSelectedRowIds; }
        /// <summary>
        /// Show CheckBox For Multi Select Rows 
        /// </summary>
        /// <param name="checkBox"></param>
        /// <returns></returns>
        //public GridControl ShowCheckBox(bool checkBox) { _showCheckBox = checkBox; return this; }
        public GridControl SetMultiboxOnly(bool multiboxonly) { _multiboxonly = multiboxonly; return this; }
        /// <summary>
        /// set multi select
        /// </summary>
        /// <param name="multiSelect"></param>
        /// <returns></returns>
        public GridControl SetMultiSelect(bool multiSelect) { _multiSelect = multiSelect; return this; }


        public GridControl SetHttpVerb(HttpVerbs verb) { _httpVerb = verb; return this; }

        /// <summary>
        /// The name of the div that will contain the grid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GridControl SetName(string name) { this.Name = name; return this; }

        /// <summary>
        /// The name of the div that will contain the pager
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public GridControl SetPager(IGridPagerControl pager) { this.Pager = pager; return this; }

        /// <summary>
        /// Sets the property Id to use to fetch the sub-grid's data
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GridControl SetListQueryParams(string url) { _listParams = url; return this; }

        /// <summary>
        /// The url to the command that will return the list data of the grid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GridControl SetListUrl(string url) { _listUrl = url; return this; }

        /// <summary>
        /// The url to the command that will update the edited row
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GridControl SetEditUrl(string url) { _editUrl = url; return this; }

        /// <summary>
        /// The page size
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GridControl SetPageSize(int pageSize) { _pageSize = pageSize; return this; }

        /// <summary>
        /// Set wether the grid columns will autosize themself
        /// </summary>
        /// <param name="autoSize"></param>
        /// <returns></returns>
        public GridControl SetIsAutoSize(bool autoSize) { _isAutoSize = autoSize; return this; }



        public GridControl SetAltRows(bool altRows) { _altRows = altRows; return this; }

        /// <summary>
        /// Add a column mapping
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public GridControl AddColumn(GridColumnModel column) { _columns.Add(column); return this; }

        public GridControl UpdateDefaultPager(Action<GridPagerControl> action)
        {
            if (Pager is GridPagerControl == false)
            {
                Pager = new GridPagerControl();
            }
            action((GridPagerControl)Pager);
            return this;
        }

        //public GridControl SetColumns<T>(Action<List<GridColumnModel<T>>> columns) { return this; }

        public GridControl SetColumns<T>(Action<GridColumnModelList<T>> initCols) where T : class
        {
            GridColumnModelList<T> cols = new GridColumnModelList<T>();
            initCols(cols);
            _columns.AddRange(cols.Items);
            return this;
        }

        public GridControl UseColumns<T>(GridColumnModelList<T> columns) where T : class
        {
            UseColumns(columns.Items);
            return this;
        }

        public GridControl UseColumns(IEnumerable<GridColumnModel> items)
        {
            _columns.AddRange(items);
            return this;
        }

        /// <summary>
        /// Creates a sub grid
        /// </summary>
        /// <param name="subGrid"></param>
        /// <returns></returns>
        public GridControl CreateSubGrid(GridControl subGrid) { _subGrid = subGrid; return this; }


        /// <summary>
        /// Renderes the grid as RTL
        /// </summary>
        /// <returns></returns>
        public GridControl IsRTL()
        {

            _languageDirection = "rtl";
            return this;
        }
        public GridControl IsLTR()
        {

            _languageDirection = "ltr";
            return this;
        }
        public GridControl SetLanguageCode(string languageCode)
        {
            _languageCode = languageCode;
            return this;
        }
        public GridControl SetDirection(string direction)
        {
            _languageDirection = direction;
            return this;
        }
        public GridControl LanguageDirection(string languageDirection) { _languageDirection = languageDirection; return this; }

        /// <summary>
        /// Add additional custom parameters to the Grid
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public GridControl SetAdditionalAttributes(string attributes) { _attributes = attributes; return this; }


        private string _scriptTemplate = "<script language=\"javascript\" type=\"text/javascript\">{0}</script>";

        public string RequiredData()
        {
            string dataBuilder = "";

            if (_gridDataSource != null)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                dataBuilder = this.Name + "_dataSource = " + ser.Serialize(_gridDataSource) + ";";
            }

            if (_subGrid != null)
                dataBuilder += _subGrid.RequiredData();

            if ((this.isSubGrid) || (string.IsNullOrEmpty(dataBuilder)))
                return dataBuilder;
            else
                return string.Format(_scriptTemplate, dataBuilder);
        }

        public GridControl UseController<TControllerType>(object parent = null)
           where TControllerType : IGridController, new()
        {
            IGridController controller = Activator.CreateInstance<TControllerType>();

            if (parent == null)
            {
                SetListUrl(controller.GetListUrl());
                SetEditUrl(controller.GetEditUrl());
                UseColumns(controller.GetRawColumns());
            }
            else
            {
                SetListUrl(controller.GetListUrl(parent));
                SetEditUrl(controller.GetEditUrl(parent));
                UseColumns(controller.GetRawColumns());
            }

            return this;
        }


        #endregion

        public string Render()
        {
            if (this.Pager != null) this.Pager.GridName = this.Name;

            StringBuilder sb = new StringBuilder();
            if (this._editable)
                sb.AppendLine("var selICol; //iCol of selected cell\n" +
                              "var selIRow; //iRow of selected cell");
            //if (this._keepSelectedRowsOnPaging)
            //    sb.AppendLine("var ").Append(this.Name).Append("_SelectedRowIds=[];");
            if (!this.isSubGrid)
            {
                sb.AppendLine("var " + this.Name + "=$(\"#" + this.Name + "\").jqGrid({ajaxGridOptions: {cache: false},");
                sb.AppendFormat("url: \"{0}\",\r\n", _listUrl);
            }
            else
            {
                sb.AppendLine("jQuery(\"#\" + subgrid_table_id).jqGrid({");
                sb.AppendFormat("url: \"{0},\r\n", _listUrl);
            }


            if (!string.IsNullOrEmpty(_editUrl)) sb.AppendFormat("editurl: \"{0}\",\r\n", _editUrl);

            if (_gridDataSource != null)
            {
                sb.AppendFormat("data: " + this.Name + "_dataSource,\r\n");
                sb.AppendFormat("datatype: \"local\",\r\n");
            }
            else
            {
                sb.AppendFormat("mtype: \"" + _httpVerb.ToString().ToLower() + "\",\r\n");
                if (!string.IsNullOrEmpty(_listUrl))
                {
                    sb.AppendFormat("datatype: \"json\",\r\n");
                }
                else
                {
                    sb.AppendFormat("datatype: \"local\",\r\n");
                }
            }

            sb.AppendFormat("colNames: [{0}],\r\n", renderColumnNames());
            sb.AppendFormat("colModel: [{0}],\r\n", renderColumnsModel());
            sb.AppendFormat("rowNum: {0},\r\n", _pageSize);

            if (this._rowNumber)
            {
                sb.AppendFormat("rownumbers: true,\r\n");
                if (this._rowNumberWidth != null)
                {
                    sb.AppendFormat("rownumWidth:{0},\r\n", this._rowNumberWidth);
                }
            }
            if (this._loadonce)
                sb.AppendFormat("loadonce: true,\r\n");
            // sb.AppendFormat("loadonce:true,\r\n");
            // Makes sure that the Delete method redurns the correct name for the id-field
            var primaryKey = getKeyColumnName();
            if (!string.IsNullOrEmpty(primaryKey))
                sb.AppendFormat("prmNames: {{id: \"{0}\"}},\r\n", primaryKey);

            if (!this.isSubGrid)
            {
                string pagerInit = "";
                if (this.Pager != null) pagerInit = this.Pager.OnGridLoad();

                string completeEvent = _onGridComplete != null ? _onGridComplete + "();" : "";
                sb.AppendLine("gridComplete: function (){" + pagerInit + completeEvent + "updateButtonState($('#" + this.Name + "'));},");
            }


            sb.AppendLine("loadComplete: function(data){this.grid.hDiv.scrollLeft = this.grid.bDiv.scrollLeft;");
            if (_keepSelectedRowsOnPaging)
            {
                sb.Append("var grid = $('#").Append(this.Name).AppendLine("');");
                sb.AppendLine("var ids = grid.jqGrid('getDataIDs');");
                sb.AppendLine("for (var i = 0; i < ids.length; i++){");
                sb.Append("\tif (ids[i] in ").Append(this.Name + "_SelectedRowIds").AppendLine("){");
                sb.AppendLine("\t\tgrid.setSelection(ids[i], false);");
                sb.AppendLine("\t}");
                sb.AppendLine("}");
            } 
            if (!string.IsNullOrEmpty(_onLoadComplete))
            {
                sb.AppendLine(_onLoadComplete + "(data);");
            }
            if (!string.IsNullOrEmpty(_onAfterLoadComplete))
            {
                sb.AppendLine(_onAfterLoadComplete + "(data);");
            }
            sb.AppendLine("},");
            if (_languageDirection == "rtl")
                sb.AppendLine("direction: \"rtl\",");
            sb.Append("regional : '").Append(_languageCode).AppendLine("',");

            if (!string.IsNullOrEmpty(_height))
                sb.AppendFormat("height: {0},\r\n", _height);

            if (!string.IsNullOrEmpty(_width))
            {
                sb.AppendFormat("width: {0},\r\n", _width);
            }
            if (_isRowNumber)
            {
                sb.AppendFormat("rownumbers: {0},\r\n", "true");
            }


            if (this._rowList != null)
                sb.AppendFormat("rowList: [{0}],", string.Join(",", ((from p in this._rowList select p.ToString()).ToArray()))).AppendLine();

            if (this._rowNum.HasValue) sb.AppendFormat("rowNum:{0},", this._rowNum.Value).AppendLine();
            if (this._pgButtons.HasValue) sb.AppendFormat("pgbuttons:{0},", this._pgButtons.Value.ToString().ToLower()).AppendLine();

            if (this._editable)
                sb.AppendLine("rowList: '',");

            if (this._editable)
                sb.AppendLine("cellEdit: true,");
            if (!this.isSubGrid)
                sb.Append("pager: $(\"#" + this.Name + "Pager\"),");

            if (_sortname == null)
            {
                _sortname = _columns[0].Name != "act" ? _columns[0].Name : (_columns.Count > 1 ? _columns[1].Name : "");
            }
            sb.AppendFormat("sortname: \"{0}\",\r\n", _sortname);

            sb.AppendFormat("sortorder: \"{0}\",\r\n", _sortorder);
            if (_footerRow)
            {
                sb.AppendFormat("footerrow: {0},\r\n", "true");
            }
            else
            {
                sb.AppendFormat("footerrow: {0},\r\n", "false");
            }
            if (_multisort)
            {
                sb.AppendFormat("multisort: \"{0}\",\r\n", true);
            }


            sb.AppendFormat("autowidth: {0},\r\n", _isAutoSize.ToString().ToLower());


            sb.AppendLine("onSelectRow: function(id, isSelected){");
            if (_keepSelectedRowsOnPaging)
            {
                
                sb.AppendLine("var contains = (Object.keys(" + this.Name + "_SelectedRowIds).indexOf(id)>=0);");
                sb.AppendLine("if (!isSelected && contains) {");
                sb.AppendLine("delete " + this.Name + "_SelectedRowIds[id]");
                sb.AppendLine("}");
                sb.AppendLine("else if (!contains) {");
                sb.AppendLine("if($('#" + this.Name + "').jqGrid('getGridParam', 'multiselect')==false){");
                sb.AppendLine("" + this.Name + "_SelectedRowIds[id] = [];");
                sb.AppendLine("}");
                sb.AppendLine("" + this.Name + "_SelectedRowIds[id] = jQuery('#" + this.Name + "').jqGrid('getRowData', id);");
                
                sb.AppendLine("}");
            }            
            if (!string.IsNullOrEmpty(_onSelectedRow))
            {                               
                sb.AppendLine(_onSelectedRow + "($(\"#" + this.Name + "\").getRowData(id)); updateButtonState($('#" + this.Name + "'));");
            }
            sb.Append("},");

            sb.AppendLine("onSelectAll:function(ids,isSelected){");

            if (_keepSelectedRowsOnPaging)
            {
                sb.AppendLine("for(i=0;i<ids.length;i++){");
                sb.AppendLine("var contains = (Object.keys(" + this.Name + "_SelectedRowIds).indexOf(ids[i])>=0);");
                sb.AppendLine("if (!isSelected && contains) {");
                sb.AppendLine("delete " + this.Name + "_SelectedRowIds[ids[i]]");
                sb.AppendLine("}");
                sb.AppendLine("else if (!contains) {");
                sb.AppendLine("" + this.Name + "_SelectedRowIds[ids[i]] = jQuery('#" + this.Name + "').jqGrid('getRowData', ids[i]);");
                sb.AppendLine("}");
                sb.AppendLine("}");
            }
            if (!string.IsNullOrEmpty(_onSelectAllRow))
            {
                sb.AppendLine(_onSelectAllRow + "(ids,isSelected);");
            }
            sb.AppendLine("},");

            if (!string.IsNullOrEmpty(_onDblClickRow))
            {
                sb.AppendLine(
                    "ondblClickRow: function(id){" +
                    _onDblClickRow + "($(\"#" + this.Name + "\").getRowData(id));},");
            }
            if (this._editable)
            {
                sb.AppendLine("beforeEditCell: function (rowid, cellname, value, irow, icol) {\n" +
                    "console.log('rowid='+rowid+', cellname='+cellname+', value = '+value+', irow = '+irow+', icol'+icol);" +
                "selICol = icol;\n" +
                "selIRow = irow;\n" +
                 "},");
            }

            foreach (GridColumnModel col in _columns)
            {
                if (col.AsGroup)
                {
                    sb.Append("grouping:true, groupingView:{groupField:['" + col.Name + "']");

                    if (!string.IsNullOrEmpty(_groupingFormat))
                        sb.Append(", " + _groupingFormat);

                    sb.Append("},");

                    break;
                }
            }

            sb.AppendFormat("caption: \"{0}\"", _caption);
            sb.AppendLine(",");
            if (_multiSelect)
            {
                sb.AppendFormat("multiselect: true");
                sb.AppendLine(",");
            }
            if (_altRows)
            {
                sb.AppendLine("altRows:true,");
                sb.Append("altclass:'").Append(_altRowsClass).AppendLine("',");
            }
            if (_multiboxonly)
            {
                sb.AppendFormat("multiboxonly: true");
                sb.AppendLine(",");
            }
            else
            {
                sb.AppendFormat("multiboxonly: false");
                sb.AppendLine(",");
            }
            if (_forceFit)
            {
                sb.AppendFormat("forceFit:true");
                sb.AppendLine(",");
            }

            if (_shrinkToFit)
            {
                sb.AppendFormat("shrinkToFit:true");
            }
            else
            {
                sb.AppendFormat("shrinkToFit:false");
            }

            if (!string.IsNullOrEmpty(_attributes)) sb.AppendLine(_attributes);

            if (_subGrid != null)
            {
                sb.AppendLine(",");
                sb.AppendLine("subGrid: true,");
                sb.AppendLine("subGridRowExpanded: function(subgrid_id, row_id) {");
                sb.AppendLine("var subgrid_table_id;");
                sb.AppendLine("subgrid_table_id = subgrid_id+\"_t\";");
                sb.AppendLine("$(\"#\"+subgrid_id).html(\"<table id='\"+subgrid_table_id+\"' class='scroll'></table>\");");

                _subGrid.isSubGrid = true;
                _subGrid._listUrl += "\" + $(\"#" + this.Name + "\").getRowData(row_id)." + getKeyColumnName();
                _subGrid._height = "\"100%\"";
                sb.AppendFormat("{0}{1}\r\n", _subGrid.Render(), "}");
            }

            sb.Append("});");
           
            if (IsFilterToolbar)
            {
                if (FilterToolbar == null)
                    FilterToolbar = new FilterToolbar();

                sb.Append("$('#").Append(this.Name).Append("').jqGrid('filterToolbar', { stringResult: ").Append(this.FilterToolbar.StringResult ? "true" : "false").
                    Append(", searchOnEnter: ").Append(this.FilterToolbar.SearchOnEnter ? "true" : "false").Append(", defaultSearch: '").Append(this.FilterToolbar.DefaultSearch).
                    Append("', ignoreCase: ").Append(this.FilterToolbar.IgnoreCase ? "true" : "false").AppendLine(" });");
            }
            if (_frozen)
            {
                sb.Append("jQuery('#" + this.Name + "').jqGrid('setFrozenColumns');");
            }
            //sb.AppendLine("$('#" + this.Name + "').navGrid('#" + this.Name + "Pager').navButtonAdd('#" + this.Name + "Pager', { edit: false, add: false, del: false });");
            

            return sb.ToString();
        }

        private string getKeyColumnName()
        {
            // First, use any specific primary key
            foreach (GridColumnModel col in _columns)
                if (col.IsPrimaryKey) return col.Name;

            // Secondly, use any implicit primary key (set through DataAnnotations)
            foreach (GridColumnModel col in _columns)
                if (col.IsImplicitPrimaryKey) return col.Name;
            return "";
            //throw new Exception("Grid Renderer Failed: Please choose a column as a primary key");
        }

        private string renderColumnNames()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _columns.Count; i++)
            {
                sb.Append("\"");
                sb.Append(_columns[i].GetColumnCaption());
                sb.Append("\"");
                if (i < _columns.Count - 1) sb.Append(",");
            }
            return sb.ToString();
        }

        private string renderColumnsModel()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _columns.Count; i++)
            {
                sb.Append(_columns[i].Render());
                if (i < _columns.Count - 1) sb.AppendLine(", ");
            }

            return sb.ToString();
        }
    }
}
