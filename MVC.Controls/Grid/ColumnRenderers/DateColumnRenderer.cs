using MVC.Controls.Grid;

namespace Housing.MVC.Controls.Grid.ColumnRenderers
{
    /// <summary>
    /// The default column editing renderer
    /// </summary>
    public class DateColumnRenderer : IColumnRenderer
    {
        public DateColumnRenderer()
        {
           
        }
        public string Render()
        {
           
            string dataEvents = this.Column.renderDataEvents();
            if (!string.IsNullOrEmpty(dataEvents))
            {
                dataEvents = ", editoptions: {" + dataEvents + "}";
            }

            return "edittype: 'text'" + dataEvents + ",formatter:'date',formatoptions: {srcformat: 'd/m/Y', newformat: 'd/m/Y'}";
        }
        public GridColumnModel Column { get; set; }
    }
}
