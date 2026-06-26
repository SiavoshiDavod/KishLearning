using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.BaseInfo
{
    public class City 
    {
        [Key]
        public int Id { get; set; }
        [GenericStringLength(100)]
        [GenericRequired]
        [Display(Name ="شهر")]
        public string DropDownTitle { get; set; }
        [Display(Name = "استان")]
        public Province ProvinceId { get; set; }

        [NotMapped]
        [Display(Name = "استان")]
        public string ProvinceName => EnumExtention.GetDescription(ProvinceId);
    }
}