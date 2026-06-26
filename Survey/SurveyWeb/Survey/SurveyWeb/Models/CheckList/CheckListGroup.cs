using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.CheckList
{
    [Description("عنوان ارزیابی")]
    public class CheckListGroup : BaseEntity
    {
        [GenericRequired, GenericStringLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }
    }
}