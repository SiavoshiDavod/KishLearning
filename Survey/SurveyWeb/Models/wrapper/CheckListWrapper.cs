using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.wrapper
{
    public class CheckListWrapper
    {
        public string Name { get; set; }
        public List<CheckListItemWrapper> CheckListItems { get; set; }
    }
}