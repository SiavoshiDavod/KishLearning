using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.BaseInfo
{
    public class Education 
    {
        [Key]
        public int Id { get; set; }
        [GenericStringLength(100)]
        [GenericRequired]
        [Display(Name = "مدرک")]
        public string DropDownTitle { get; set; }
    }
}