using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.CheckList
{
    [Description("آیتم های چک لیست بازرسی")]
    public class ComplaintCheckListItem : BaseEntity
    {
        [GenericRequired]
        public int ComplaintCheckListId { get; set; }
        [ForeignKey("ComplaintCheckListId")]
        public ComplaintCheckList ComplaintCheckList { get; set; }
        [GenericRequired]
        public int CheckListItemId { get; set; }
        [ForeignKey("CheckListItemId")]
        public CheckListItem CheckListItem { get; set; }
        [Display(Name = "بله یا خیر")]
        public bool? IsYesNo { get; set; }
        [Display(Name = "خوب بد متوسط")]
        public int? IsGoodMidBad { get; set; }
        [Display(Name = "دارد یا ندارد")]
        public int? IsHasItDontHave { get; set; }
        public int? ValueItem { get; set; }
        [Display(Name = "بازدید")]
        [NotMapped]
        public String CheckListName { get; set; }
        [Display(Name = "مرکز")]
        [NotMapped]
        public String ResturantName { get; set; }
    }
}