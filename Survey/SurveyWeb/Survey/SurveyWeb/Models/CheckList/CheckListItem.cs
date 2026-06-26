using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.CheckList
{
    [Description("آیتم های چک لیست")]
    public class CheckListItem : BaseEntity
    {
        [GenericRequired, GenericStringLength(50)]
        [Display(Name = "عنوان")]
        public string Name { get; set; }
        [ForeignKey("CheckListGroupId")]
        public CheckListGroup CheckListGroup { get; set; }
        [Display(Name = "گروه بندی")]
        [GenericRequired]
        public int CheckListGroupId { get; set; }
        [Display(Name = "نوع چک لیست ")]
        public CheckListItemTypeEnum CheckListItemType { get; set; }
        [ForeignKey("CheckListId")]
        public CheckList CheckList { get; set; }
        [Display(Name = "چک لیست")]
        [GenericRequired]
        public int CheckListId { get; set; }
        [NotMapped]
        public string CheckListName { get; set; }
        //public static readonly int 
    }
    public enum CheckListItemTypeEnum
    {
        [Display(Name = "بله خیر")]
        YesNo =1,
        [Display(Name = "خوب متوسط بد")]
        GoodMediumBad =2,
        [Display(Name = "دارد ندارد")]
        HasItDontHave =3
    }
}