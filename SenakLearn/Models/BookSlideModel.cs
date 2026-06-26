using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    [Table("BookSlide")]
    public class BookSlideModel: BaseEntity
    {
        [Display(Name = "لینک")]
        public string Url { get; set; }=string.Empty;
        [Display(Name = "تصویر اسلایدر"), GenericRequired]
        public string Img { get; set; }
        [Display(Name = "وضعیت"), GenericRequired]
        public bool IsActive { get; set; } = true;
        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}