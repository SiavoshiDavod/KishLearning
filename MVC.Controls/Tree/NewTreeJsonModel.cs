using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Controls.Tree
{
    public class NewTreeJsonModel
    {
        public string state {get;set;}
        public TreeParameterJsonModel attr { get; set; }

        public List<TreeJsonModel> children { get; set; }

        //for new jstree
        public string id { get; set; }
        //public string parent { get; set; }
        public string text { get; set; }
    }
}