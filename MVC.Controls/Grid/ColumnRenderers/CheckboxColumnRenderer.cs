using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// The default column editing renderer
    /// </summary>
    public class CheckboxColumnRenderer : IColumnRenderer
    {
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
                                                 + "selIRow = selIRow + 1;\n"
                                                + " grid.jqGrid('restoreRow', \" + selIRow + \");"
                //+ "$(\"#grid\").setSelection(\" + selIRow + \" + 1, true);\n"
                                               // + "selIRow = selIRow + 1;\n"
                                               // + "$(\"#grid\").jqGrid('setSelection', \" + selIRow + \");\n"
                                                + "selICol = 1;\n"
                                                + "setTimeout(\"jQuery('#grid').editCell(\" + selIRow + \" + 1, \" + selICol + \", true);\", 100);\n"
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
            string value = string.Format("value:\"true\"");
            if (!string.IsNullOrEmpty(dataEvents))
            {
            dataEvents = ", editoptions: {" + value + "}";
            }
            else
            {
                dataEvents = ", editoptions: {" + value + "},formatter:'checkbox'"; 
            }
            return " edittype: 'checkbox'" + dataEvents;
        }
        public GridColumnModel Column { get; set; }
    }
}
