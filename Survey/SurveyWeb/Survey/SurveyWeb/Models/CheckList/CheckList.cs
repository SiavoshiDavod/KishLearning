using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.CheckList
{
    [Description("چک لیست")]
    public class CheckList : BaseEntity
    {
        [GenericRequired, GenericStringLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "فعال")]
        [NotMapped]
        public bool  IsActive { get; set; }
        [InverseProperty("CheckList")]
        public ICollection<CheckListItem> CheckListItems { get; set; }

        }

}