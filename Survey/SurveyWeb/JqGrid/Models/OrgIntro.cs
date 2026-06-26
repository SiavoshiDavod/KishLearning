using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public class OrgIntro : BaseEntity
    {
        [Display(Name = "نام"), GenericRequired, GenericStringLength(200)]
        public string Name { get; set; }
        [Display(Name = "عکس"), GenericStringLength(200)]
        public string ImageUrl { get; set; }

        [Display(Name = "خلاصه"), GenericRequired, AllowHtml, GenericStringLength(2000)]
        public string Summery { get; set; }
        [Display(Name = "توضیحات "), GenericRequired, AllowHtml]
        public string Description { get; set; }
        [Display(Name = "عکس در سمت چپ نشان داده شود؟")]
        public bool IsImageDirectionLeft { get; set; }
        [NotMapped]
        public string Divid => "OrgIntroDiv" + this.Id;
    }
}
