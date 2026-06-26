using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveyWeb.Models.CheckList;

namespace SurveyWeb.Models.wrapper
{
    public class ComplaintCheckListItemWrapper
    {
        public string Id { get; set; }
        public int? ComplaintCheckListItemId { get; set; }
        public string act { get; set; }
        public int ComplaintCheckListId { get; set; }
        public ComplaintCheckList ComplaintCheckList { get; set; }
        public int CheckListItemId { get; set; }
        public string CheckListItemName { get; set; }
        public string CheckListItemGroupName { get; set; }
        public CheckListItem CheckListItem { get; set; }
        public bool? IsYesNo { get; set; }
        public int? IsGoodMidBad { get; set; }
        public int? IsHasItDontHave { get; set; }
        public string StatusName { get; set; }
        public int? ValueItem { get; set; }
    }
}