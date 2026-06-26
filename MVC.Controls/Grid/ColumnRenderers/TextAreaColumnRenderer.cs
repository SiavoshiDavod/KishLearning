using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class TextAreaColumnRenderer : IColumnRenderer
    {
        public TextAreaColumnRenderer(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }

        public string Render()
        {
            string editoptions = "dataInit: function (elem) { $(elem).focus(function () { this.select(); }) },\n"
        + "dataEvents: [\n"
                            + "{\n"
                               + " type: 'keydown',\n"

                               + " fn: function (e) {\n"
                                    + "var key = e.charCode || e.keyCode;\n"
                                    + "if (key == 13)\n"
                                    + "{\n"
                                        + "var collen = $(\"#grid\").jqGrid('getGridParam', 'colModel').length;\n"
                                        + "var rowlen = $(\"#grid\").jqGrid('getGridParam', 'records');\n"
                                        + "if (selICol == collen - 1) {\n"
                                            + "if (selIRow == rowlen) {\n"
                                                + "gridAddRow(\"grid\"); \n"
                                            + "}\n"
                                            + "else {\n"
                                                + "selICol = 1;\n"
                                                + "setTimeout(\"jQuery('#grid').editCell(\" + selIRow + \" + 1, \" + selICol + \", true);\", 100);\n"
                                            + "}\n"

                                        + "}\n"
                                        + "else\n"
                                            + "setTimeout(\"jQuery('#grid').editCell(\" + selIRow + \" , \" + selICol + \" + 1, true);\", 100);\n"
                                    + "} else {\n"
                                    + "}\n"
                                + "}\n"
                            + "}\n"
                        + "]";
            string dataEvents = this.Column.renderDataEvents();
            string rowcol = string.Format("rows:\"{0}\", cols:\"{1}\"", Rows, Cols);
            //if (!string.IsNullOrEmpty(dataEvents))
            //{
                dataEvents = ", editoptions: {" + editoptions + "," + rowcol + "}";
            //}
            //else
            //{
            //    dataEvents = ", editoptions: {" + rowcol + "}";
            //}


            return "edittype: 'textarea'" + dataEvents;
        }
        public GridColumnModel Column { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
    }
}
