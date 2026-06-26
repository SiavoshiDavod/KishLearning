using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SenakLearn.Models
{
    [Table("Book", Schema = "dbo")]
    public class Book : BaseEntity
    {
        [Display(Name = "نام انگلیسی کتاب"),GenericRequired]
        public string Title { get; set; }

        [Display(Name = "نام فارسی کتاب"),GenericRequired]
        public string TitleF { get; set; }

       
        
        [Display(Name = "موضوع")]
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public learn_cours_group Group { get; set; }

        [Display(Name = "ناشر")]
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public PaperPublisher Publisher { get; set; }

        [Display(Name = "تعداد صفحات "), Range(1, 2000)]
        public int PageCount { get; set; }

        [Display(Name = "سال انتشار"), Range(1990, 2050)]
        public int Year { get; set; }


        [Display(Name = "کلمات کلیدی")]
        public string Keyword { get; set; }

       
        [Display(Name = "فهرست کتاب")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Indicator { get; set; }

        [Display(Name = "توضیحات/خلاصه کتاب ")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Abstract { get; set; }
        
        [Display(Name = "مبلغ(ریال)")]
        public int Price { get; set; }

        [Display(Name = "فایل پی دی اف")]
        public string FileId { get; set; }

        [Display(Name = "تصویری از کتاب ")]
        public string ScreenShotId { get; set; }

        [Display(Name = "نویسندگان/مولف"),GenericRequired,GenericMaxLength(200)]
        public string Author { get; set; }

        [Display(Name = "مترجمان"), GenericMaxLength(200)]
        public string Translator { get; set; }

        [Display(Name = "شابک"),GenericMaxLength(50)]
        public string Shabak { get; set; }

        [Display(Name = "رده بندی کنگره"), GenericMaxLength(50)]
        public string Ranking1 { get; set; }

        [Display(Name = "رده بندی دیویی"), GenericMaxLength(50)]
        public string Ranking2 { get; set; }

        [Display(Name = "شماره کتابشناسی ملی"), GenericMaxLength(50)]
        public string MelliBookingCode { get; set; }
        [Display(Name = "نمایش در سلایدر")]
        public bool? IsSlider { get; set; }
        [Display(Name = "تصویر سلایدر")]
        public string SlideImg { get; set; }
    }
}