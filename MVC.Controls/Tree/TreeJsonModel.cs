using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Controls.Tree
{
    public class TreeJsonModel
    {
        public TreeJsonModelData data { get; set; }
        //public string state { get; set; }
        public TreeParameterJsonModel attr { get; set; }

        public object children { get; set; }

        //for new jstree
        private string _id;
        public string id { get { return (_id != null ? _id : (attr != null ? attr.Code : null)); } set { this._id = value; } }
        public string icon { get; set; }
        private string _text;
        public string text { get { return _text != null ? _text : (data != null ? data.title : null); } set { this._text = value; } }        
        public object li_attr { get; set; }
        public object a_attr { get; set; }
        public TreeJsonModelState state { get; set; }
        public bool? isPost { get; set; }
    }
    public class TreeJsonModelState
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class TreeJsonModelData
    {
        public string title { get; set; }
        private string _text;
        public string text { get { return _text == null ? title : _text; } set { this._text = value; } }
        public string titleColor { get; set; }
        public TreeParameterJsonModel attr { get; set; }
        public string icon { get; set; }
    }
}