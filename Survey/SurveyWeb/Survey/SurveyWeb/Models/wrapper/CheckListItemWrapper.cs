using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveyWeb.Models.CheckList;

namespace SurveyWeb.Models.wrapper
{
    public class CheckListItemWrapper
    {
        public int Id { get; set; }
        public int CheckListId { get; set; }
        public string Name { get; set; }
        public CheckListGroup CheckListGroup { get; set; }
        public int CheckListGroupId { get; set; }
        public string CheckListGroupName { get; set; }
        public CheckListItemTypeEnum CheckListItemType { get; set; }
        public string CheckListItemTypeName { get; set; }
        public string CheckListName { get; set; }
        public string act { get; set; }
    }
}