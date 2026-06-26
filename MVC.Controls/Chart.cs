using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.WebPages;

namespace MVC.Controls
{
    public static class ChartHelper
    {
        public static Chart Chart(this HtmlHelper html, string Name)
        {
            return new Chart(html, Name);
        }
    }
    public class Chart : IHtmlString
    {
        private readonly HtmlHelper _html;
        private string _title;
        private Func<string, string> _displayProperty = item => item.ToString();
        private Func<string, HelperResult> _itemTemplate;
        private string _url;
        private string _name;
        private string _lineDataUrl = "";
        private string _barDataUrl = "";
        private int _width = 400;
        private int _height = 150;
        private string[] _lable;

        public Chart(HtmlHelper html, string Name)
        {
            if (html == null) throw new Exception("Html");
            _html = html;
            _name = Name;
            // The ItemTemplate will default to rendering the DisplayProperty
            _itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }

        public Chart SetLabels(params string[] lable)
        {
            this._lable = lable;
            return this;
        }

        public Chart SetTittle(string tittle)
        {
            this._title = tittle;
            return this;
        }

        public Chart SetLineDataUrl(string Url)
        {
            this._lineDataUrl = Url;
            return this;
        }

        public Chart SetBarDataUrl(string Url)
        {
            this._barDataUrl = Url;
            return this;
        }
        public Chart SetWidth(int width)
        {
            this._width = width;
            return this;
        }
        public Chart SetHeight(int height)
        {
            this._height = height;
            return this;
        }
        public override string ToString()
        {
            //create our string builder to hold our html
            StringBuilder chartBuilder = new StringBuilder();
            //create the opener tag add the class topnav (will use for jquery chart)
            chartBuilder.AppendLine("<div id=\"" + this._name + "\" style=\"width: " + this._width.ToString() + "px; height: " + this._height.ToString() + "px\"></div>");
            chartBuilder.AppendLine("<br/>");
            chartBuilder.AppendLine("<br/>");
            chartBuilder.AppendLine("<input type=\"button\" id=\"btn" + this._name + "\" value=\"بارگزاری مجدد\" onclick=\"" + this._name + ".replot( { resetAxes: true } )\" />");

            //return the html
            chartBuilder.AppendLine("<script type=\"text/javascript\">");
            // initialise plugins
            chartBuilder.AppendLine("jQuery(function () {");
            chartBuilder.AppendLine("var LineData= ['',0];");
            chartBuilder.AppendLine("var BarData = ['',0];");
            chartBuilder.AppendLine(this._name + " = $.jqplot(\"" + this._name + "\", [BarData, LineData], {");

            // Turns on animatino for all series in this plot.
            chartBuilder.AppendLine("animate: true,");
            // Will animate plot on calls to plot1.replot({resetAxes:true})                        
            chartBuilder.AppendLine("animateReplot: true,");
            chartBuilder.AppendLine("cursor: {");
            chartBuilder.AppendLine("zoom: true,");
            chartBuilder.AppendLine("show: true,");
            chartBuilder.AppendLine("looseZoom: true,");
            chartBuilder.AppendLine("showTooltip: false");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("title: {");
            chartBuilder.AppendLine(string.Format("text: '{0}'", this._title));
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("tooltip: {");
            chartBuilder.AppendLine("formatter: function () {");
            chartBuilder.AppendLine("return this.point.name + ': ' + Math.round(this.percentage) + ' %';");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("series: [");
            //if (this._lable != null && this._lable.Count() > 0)
            //{
            //    foreach (var item in this._lable)
            //    {
            //        chartBuilder.AppendLine("{label:'" + item + "'},");
            //    }
            //}
            chartBuilder.AppendLine("{");
            chartBuilder.AppendLine("pointLabels: {");
            chartBuilder.AppendLine("show: true");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("renderer: $.jqplot.BarRenderer,");
            chartBuilder.AppendLine("showHighlight: true,");
            chartBuilder.AppendLine("yaxis: 'y2axis',");
            chartBuilder.AppendLine("rendererOptions: {");
            // Speed up the animation a little bit.
            // This is a number of milliseconds.  
            // Default for bar series is 3000. 
            chartBuilder.AppendLine("animation: {");
            chartBuilder.AppendLine("speed: 2500");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("barWidth: 15,");
            chartBuilder.AppendLine("barPadding: -15,");
            chartBuilder.AppendLine("barMargin: 0,");
            chartBuilder.AppendLine("highlightMouseOver: true");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("{");
            chartBuilder.AppendLine("rendererOptions: {");
            // speed up the animation a little bit.
            // This is a number of milliseconds.
            // Default for a line series is 2500.
            chartBuilder.AppendLine("animation: {");
            chartBuilder.AppendLine("speed: 2000");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("}],");
            chartBuilder.AppendLine("axesDefaults: {");
            chartBuilder.AppendLine("pad: 0");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("axes: {type: 'dateTime', location: 'bottom', skipEmptyMonths: true,");
            // These options will set up the x axis like a category axis.
            chartBuilder.AppendLine("xaxis: {");
            chartBuilder.AppendLine("renderer: $.jqplot.CategoryAxisRenderer,tickOptions: { angle: -30, fontSize: '10pt' },");

            chartBuilder.AppendLine("drawMajorGridlines: false,");
            chartBuilder.AppendLine("drawMinorGridlines: true,");
            chartBuilder.AppendLine("drawMajorTickMarks: false,");
            chartBuilder.AppendLine("rendererOptions: {");
            chartBuilder.AppendLine("tickInset: 0.5,");
            chartBuilder.AppendLine("minorTicks: 1");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine(" yaxis: {");
            chartBuilder.AppendLine("tickOptions: {");
            chartBuilder.AppendLine("formatString: \"%'d\"");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("rendererOptions: {");
            chartBuilder.AppendLine("forceTickAt0: true");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine(" y2axis: {");
            chartBuilder.AppendLine("tickOptions: {");
            chartBuilder.AppendLine("formatString: \"%'d\"");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("rendererOptions: {");
            // align the ticks on the y2 axis with the y axis.
            chartBuilder.AppendLine("alignTicks: true,");
            chartBuilder.AppendLine("forceTickAt0: true");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("}");
            chartBuilder.AppendLine("},");
            chartBuilder.AppendLine("highlighter: {");
            chartBuilder.AppendLine("show: true,");
            chartBuilder.AppendLine("showLabel: true,");
            chartBuilder.AppendLine("tooltipAxes: 'y',");
            chartBuilder.AppendLine("sizeAdjust: 7.5, tooltipLocation: 'ne'");
            chartBuilder.AppendLine("}");

            chartBuilder.AppendLine(" });");

            chartBuilder.AppendLine(" });");
            chartBuilder.AppendLine("</script>");

            return chartBuilder.ToString();
        }

        #region IHtmlString Members

        public string ToHtmlString()
        {
            return ToString();
        }

        #endregion
    }
}
