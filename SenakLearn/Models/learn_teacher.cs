using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    //[DisplayPluralName("اساتید")]
    public class learn_teacher
    {
        //public learn_teacherMetadata()
        //{
        //    this.learn_cours = new HashSet<learn_cours>();
        //}
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "نام")]
        public string name { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "نام خانوادگی")]
        public string family { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "کد ملی")]
        [MaxLength(10, ErrorMessage = "حداکثر طول {0} تعداد {1} می باشد")]
        [MinLength(10, ErrorMessage = "حداقل طول {0} تعداد {1} می باشد")]
        public string meli { get; set; }
        [Display(Name = "تلفن")]
        [DataType(DataType.PhoneNumber)]
        public string tel { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "همراه")]
        [DataType(DataType.PhoneNumber)]
        public string mobile { get; set; }
        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string address { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "تاریخ ثبت")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime date_register { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "مدرک تحصیلی")]
        public string education { get; set; }
        [Display(Name = "وضعیت")]
        public bool status { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "کد استاد")]
        public string code { get; set; }
        [Display(Name = "تصویر")]
        public string image { get; set; }
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "رزومه")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Resume { get; set; }
        public long? PrincipalId { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public learn_user learnUser { get; set; }
        public virtual ICollection<learn_cours> learn_cours { get; set; }
        // public virtual ICollection<OnlineClass> OnlineClass { get; set; }
        [NotMapped]
        public string FullName => name + " " + family;
        [NotMapped]
        public string UserName => learnUser != null ? learnUser.user_name : "";
        [Display(Name = "محبوب است(نمایش در صفحه اصلی سایت)")]
        public bool IsFavorite { get; set; }
        public string IsFavoriteS { get { return IsFavorite ? "محبوب" : ""; } }
        [Display(Name = "تعداد دوره")]
        public int CourseCount { get; set; }
    }
}
