using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public class News :BaseEntity
    {
        [Display(Name = "تیتر"), GenericRequired, GenericStringLength(200)]
        public string Title { get; set; }
        [Display(Name = "گروه خبر")]
        public int NewsGroupId { get; set; }
        [Display(Name = "خلاصه"), GenericRequired, GenericStringLength(1000)]
        public string Summary { get; set; }
        [Display(Name = "کلمات کلیدی"), GenericRequired, GenericStringLength(200)]
        public string Keyword { get; set; }
        [Display(Name = "متن خبر"), GenericRequired,AllowHtml]
        public string Description { get; set; }
        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }
        [Display(Name = "نویسنده")]
        public int? AuthorId { get; set; }
        [Display(Name = "عکس اصلی"), GenericStringLength(100)]
        public string ImageUrl { get; set; }
        [NotMapped]
        public List<string> ImageUrls { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        [ForeignKey("NewsGroupId")]
        public NewsGroup NewsGroup { get; set; }
    }
}