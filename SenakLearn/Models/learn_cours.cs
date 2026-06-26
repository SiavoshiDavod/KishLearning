using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System;

namespace SenakLearn.Models
{
    //[DisplayPluralName("دوره ها")]
    public class learn_cours
    {
        //public learn_coursMetadata()
        //{

        //}
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "نام دوره")]
        [MaxLength(50, ErrorMessage = "حداکثر طول {0} تعداد {1} می باشد")]
        public string name { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "گروه بندی دوره")]
        public int id_group { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "مدت دوره")]
        public int time { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "جزئیات دوره/توضیحات")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string doc { get; set; }
        [Display(Name = "چه چیزی داخل این دوره می آموزید")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string doc2 { get; set; }
        [Display(Name = "مدرس دوره")]
        public int? id_teacher { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "وضعیت")]
        public bool status { get; set; }
        [Display(Name = "تعداد جلسات")]
        public int num_present { get; set; }

        [Display(Name = "تصویر")]
        public string image { get; set; }

        [Display(Name = "هزینه دوره(ریال)")]
        public int? Monetary { get; set; }

        public int? TypeCours { get; set; }
        public string AmountFarsi => PriceAfterDiscount != null ? PriceAfterDiscount?.ToString("N0").ToPersianNumber() : Monetary?.ToString("N0").ToPersianNumber();
        //[JsonIgnore]
        //public virtual ICollection<learn_class> learn_class { get; set; }
        [ForeignKey("id_group"), JsonIgnore]
        public virtual learn_cours_group learn_cours_group { get; set; }
        //[ForeignKey("id_type")]
        //public virtual learn_TypeCenter learn_TypeCenter { get; set; }
        //[ForeignKey("id_internship")]
        //public virtual learn_internship learn_internship { get; set; }
        [ForeignKey("id_teacher"), JsonIgnore]
        public virtual learn_teacher learn_teacher { get; set; }
        //[ForeignKey("id_workshop")]
        //public virtual learn_workshop learn_workshop { get; set; }
        //public virtual ICollection<learn_exam> learn_exam { get; set; }
        // public virtual ICollection<learn_file> learn_file { get; set; }
        [JsonIgnore]
        public virtual ICollection<OfflineVideo> OfflineVideo { get; set; }
        //public virtual ICollection<learn_internship> learn_internship1 { get; set; }
        //public virtual ICollection<learn_workshop> learn_workshop1 { get; set; }
        [Display(Name = "ویدیو معرفی دوره")]
        public Guid? InterviewPathVideo { get; set; }
        [NotMapped]
        public string InterviewPathVideoName { get; set; }
        [Display(Name = "محبوب است(نمایش در صفحه اصلی سایت)")]
        public bool IsFavorite { get; set; }
        public string IsFavoriteS { get { return IsFavorite ? "محبوب" : ""; } }
        [Display(Name = "کلاس آنلاین مرتبط")]
        public int? OnlineRelated { get; set; }
        [Display(Name = "کتاب مرتبط")]
        public int? BookRelated { get; set; }
        [Display(Name = "جزوه مرتبط")]
        public int? BookletRelated { get; set; }
        [Display(Name = "هزینه بعد از تخفیف")]
        public int? PriceAfterDiscount { get; set; }
    }
}
