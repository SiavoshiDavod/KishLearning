using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public class MenuSub : BaseEntity
    {
        [Display(Name = "ترتیب"), GenericRequired]
        public int Order { get; set; }
        [Display(Name = "وضعیت"), GenericRequired]
        public bool Status { get; set; }
        [Display(Name = "منو"), GenericRequired]
        public int MenuId { get; set; }
        [Display(Name = "تیتر"), GenericRequired, GenericStringLength(200)]
        public string Title { get; set; }
        [ GenericStringLength(1000)]
        public string Url { get; set; }
        //[Display(Name = "ویدیو")]
        //public Guid? InterviewPathVideo { get; set; }
        [Display(Name = "عکس"), GenericStringLength(100)]
        public string Image { get; set; }
        [Display(Name = "محتوا")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
        [ForeignKey("MenuId")]
        [Display(Name = "منو")]
        public Menu Menu { get; set; }
        [NotMapped]
        public bool isMenuId { get; set; }
    }
}