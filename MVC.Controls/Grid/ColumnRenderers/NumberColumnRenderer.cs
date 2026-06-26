using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// The default column editing renderer
    /// </summary>
    public class NumberColumnRenderer : IColumnRenderer
    {
        public int _decimalPlaces = 0;
        public char _thousandsSeparator = ',';
        public NumberColumnRenderer(int decimalPlaces)
        {
            _decimalPlaces = decimalPlaces;
        }
        public NumberColumnRenderer(char thousandsSeparator)
        {
            _thousandsSeparator = thousandsSeparator;
        }
        public NumberColumnRenderer(int decimalPlaces, char thousandsSeparator)
        {
            _decimalPlaces = decimalPlaces;
            _thousandsSeparator = thousandsSeparator;
        }
        public NumberColumnRenderer()
        {
        }
        public string Render()
        {
           
            string dataEvents = this.Column.renderDataEvents();
            if (!string.IsNullOrEmpty(dataEvents))
            {
                dataEvents = ", editoptions: {" + dataEvents + "}";
            }

            return "edittype: 'text'" + dataEvents + ",formatter:'number',formatoptions: { thousandsSeparator:'"+_thousandsSeparator+"',decimalPlaces: " + _decimalPlaces + "}";
            //return "edittype: 'text'" + dataEvents + ",formatter:jqgridCellValueNumberFormatter";
        }
        public GridColumnModel Column { get; set; }
    }
}
