using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Controls.Grid
{
    public class FilterToolbar
    {
        private bool _searchOnEnter = true;
        private bool _enableClear = true;
        private bool _ignoreCase = true;
        private string _defaultSearch = "cn";
        private bool _stringResult = true;
        public FilterToolbar()
        {

        }
        public FilterToolbar(bool searchOnEnter, bool enableClear, bool ignoreCase, string defaultSearch, bool stringResult)
        {
            this._searchOnEnter = searchOnEnter;
            this._enableClear = enableClear;
            this._ignoreCase = ignoreCase;
            this._defaultSearch = defaultSearch;
            this._stringResult = stringResult;
        }

        public bool SearchOnEnter { get{return _searchOnEnter;} set{this._searchOnEnter=value;} }
        public bool EnableClear { get { return _enableClear; } set { this._enableClear = value; } }
        public bool IgnoreCase { get { return _ignoreCase; } set { this._ignoreCase = value; } }
        public string DefaultSearch { get { return _defaultSearch; } set { this._defaultSearch = value; } }
        public bool StringResult { get { return _stringResult; } set { this._stringResult = value; } }
        public string RenderScript()
        {
            return "";
        }
    }
}
