using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid.ColumnRenderers
{
    public class ButtonColumnRenderer : IColumnRenderer
    {
        string Width = "";
        string Value = "";
        string ClickEvent = "";
        public ButtonColumnRenderer()
        {
        }
        public ButtonColumnRenderer(string Value,string EventName)
        {
            this.Value = Value;
            this.ClickEvent = EventName;
        }
        public ButtonColumnRenderer(string Width)
        {
            this.Width = Width;
        }
        public ButtonColumnRenderer(string Value, string Width, string EventName)
        {
            this.Value = Value;
            this.Width = Width;
            this.ClickEvent = EventName;
        }
        public string Render()
        {
            string dataEvents = this.Column.renderDataEvents();
            if (!string.IsNullOrEmpty(dataEvents))
            {
                dataEvents = ", editoptions: {" + dataEvents;
            }
            if (!string.IsNullOrEmpty(Width))
            {
                if (string.IsNullOrEmpty(dataEvents))
                    dataEvents = ", editoptions: {" + "width:'" + this.Width + "px'";
                else
                    dataEvents += ", width:'" + this.Width + "px'";
            }
            if (!String.IsNullOrEmpty(Value))
            {
                if (string.IsNullOrEmpty(dataEvents))
                    dataEvents = ", editoptions: {" + "value:'" + this.Value + "'";
                else
                    dataEvents += ", value:'" + this.Value + "'";
            }
            if (!String.IsNullOrEmpty(this.ClickEvent))
            {
                if (string.IsNullOrEmpty(dataEvents))
                    dataEvents = ", editoptions: {" + "onClick:'" + this.ClickEvent + "'";
                else
                    dataEvents += ", onClick:'" + this.ClickEvent + "'";
            }
            return "edittype: 'button'" + dataEvents + "}";
        }
        public GridColumnModel Column { get; set; }
    }
}
