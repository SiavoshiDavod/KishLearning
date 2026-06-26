using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.JqGrid
{
    [ModelBinder(typeof(GridModelBinder))]
    public class GridSettings
    {
        public bool IsSearch { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SelectedColumnNames { get; set; }
        public Filter Where { get; set; }
        public bool LoadAll { get { return _loadAll; } set { this._loadAll = value; } }
        private bool _loadAll = false;

        public static DataTable CreateFilterDataTable(GridSettings gridSetting)
        {
            var ListRules = new List<JqGrid.Rule>();

            if (gridSetting.Where != null && gridSetting.Where.rules != null)
                ListRules = gridSetting.Where.rules.ToList();

            DataTable table = new DataTable("Filter1");
            table.Columns.Add("field", typeof(string));
            table.Columns.Add("op", typeof(string));
            table.Columns.Add("data", typeof(string));
            ListRules.ForEach(i =>
            {if(IsInjectionSql(i.data))
                    throw new Exception("فیلتر شامل کاراکتر یا کلمه غیر مجاز است");
            else
                table.Rows.Add(i.field, i.op, i.data);
            });

            return table;
        }

        public static bool IsInjectionSql(string data)
        {
            if (data.Contains("="))
                return true;
            else if (data.Contains("<"))
                return true;
            else if (data.Contains(">"))
                return true;
            else if (data.ToLower().Contains("delete"))
                return true;
            else if (data.ToLower().Contains("update"))
                return true;
            else if (data.ToLower().Contains("select"))
                return true;
            else if (data.ToLower().Contains("insert"))
                return true;
            else if (data.ToLower().Contains("and"))
                return true;
            else if (data.ToLower().Contains("or"))
                return true;
            else if (data.ToLower().Contains("not"))
                return true;
            return false;
        }
        public static DataTable CreateOrderDataTable(GridSettings gridSetting)
        {
            DataTable table = new DataTable("ListOrder1");
            table.Columns.Add("ColumnName", typeof(string));
            table.Columns.Add("OrderType", typeof(string));

            if (gridSetting != null && !string.IsNullOrEmpty(gridSetting.SortColumn))
            {
                if(IsInjectionSql(gridSetting.SortColumn))
                    throw new Exception("مرتب سازی شامل کاراکتر یا کلمه غیر مجاز است");
                if (IsInjectionSql(gridSetting.SortOrder))
                    throw new Exception("مرتب سازی شامل کاراکتر یا کلمه غیر مجاز است");
                if (gridSetting.SortColumn.Contains(','))
                {
                    foreach (var sortColumn in gridSetting.SortColumn.Split(','))
                    {
                        var sortColumnArr = sortColumn.Trim().Split(' ');
                        table.Rows.Add(sortColumnArr[0], sortColumnArr[1]);
                    }
                }else
                    table.Rows.Add(gridSetting.SortColumn, gridSetting.SortOrder ?? "asc");
            }


            return table;
        }
    }
}