using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SenakLearn.Models
{
    public class Paper : BaseEntity
    {
        [Display(Name = "گروه")]
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public learn_cours_group Group { get; set; }
        [Display(Name = "رشته مرتبط با این مقاله")]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public PaperField Field { get; set; }
        [Display(Name = "گرایش های مرتبط با این مقاله")]
        public string TrendIds { get; set; }
        [Display(Name = "گرایش های مرتبط با این مقاله")]
        public string[] TrendArr
        {
            get => TrendIds.ToArrayForMultiDropDown();
            set => TrendIds = string.Join(",", value);
        }
        [Display(Name = "نشریه")]
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public PaperPublisher Publisher { get; set; }
        [Display(Name = "چاپ شده در مجله (ژورنال)")]
        public int? JournalId { get; set; }
        [ForeignKey("JournalId")]
        public PaperJournal Journal { get; set; }
        [Display(Name = "عنوان انگلیسی مقاله")]
        public string Title { get; set; }
        [Display(Name = "عنوان فارسی مقاله")]
        public string TitleF { get; set; }
        [Display(Name = "تعداد صفحات انگلیسی"), Range(1, 2000)]
        public int PageCount { get; set; }
        [Display(Name = "سال انتشار"), Range(1990, 2050)]
        public int Year { get; set; }
        [Display(Name = "کلمات کلیدی")]
        public string Keyword { get; set; }
        [Display(Name = "ارائه شده از دانشگاه")]
        public int? UniversityId { get; set; }
        [ForeignKey("UniversityId")]
        public PaperUniversity University { get; set; }
        [Display(Name = " رفرنس دارد")]
        public bool HasReference { get; set; }
        [Display(Name = "وضعیت ترجمه")]
        public bool HasTranslate { get; set; }
        [Display(Name = "تعداد صفحات ترجمه")]
        public int? PageCountF { get; set; }
        [Display(Name = " ترجمه عناوین تصاویر و جداول")]
        public bool IsTranslateImageTableTitle { get; set; }
        [Display(Name = " ترجمه متون داخل تصاویر")]
        public bool IsTranslateImage { get; set; }
        [Display(Name = " ترجمه متون داخل جداول")]
        public bool IsTranslateTable { get; set; }
        [Display(Name = "درج تصاویر در فایل ترجمه")]
        public bool IsImagesInTranslate { get; set; }
        [Display(Name = "درج جداول در فایل ترجمه")]
        public bool IsTablesInTranslate { get; set; }
        [Display(Name = "درج فرمولها و محاسبات در فایل ترجمه")]
        public bool IsFormulaInTranslate { get; set; }
        [Display(Name = "منابع داخل متن")]
        public bool IsReferenceInTranslate { get; set; }
        [Display(Name = "کیفیت ترجمه")]
        public int TranslateQualityId { get; set; }
        [ForeignKey("TranslateQualityId")]
        public PaperTranslateQuality TranslateQuality { get; set; }
        [Display(Name = "فهرست مطالب")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Indicator { get; set; }
        [Display(Name = "چکیده انگلیسی")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Abstract { get; set; }
        [Display(Name = "چکیده فارسی")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string AbstractF { get; set; }
        [Display(Name = "مبلغ(ریال)")]
        public int Price { get; set; }
        [Display(Name = "فایل اصلی")]
        public string FileId { get; set; }
        [Display(Name = "تصویری از مقاله ترجمه و تایپ شده در نرم افزار ورد")]
        public string ScreenShotId { get; set; }
        [JsonIgnore]
        [Display(Name = "فایل ورد ترجمه")]
        public string TranslateWordId { get; set; }
        [JsonIgnore]
        [Display(Name = "فایل  پی دی اف ترجمه")]
        public string TranslatePdfId { get; set; }
        [NotMapped]
        public IEnumerable<string> TrendNames { get; set; }
    }
}