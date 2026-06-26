using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public class BoardDirector:BaseEntity
    {
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
        [Display(Name = "مدرک تحصیلی")]
        public string education { get; set; }
        [Display(Name = "وضعیت")]
        public bool status { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "سمت")]
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
        public string FullName => name + " " + family;
    }
}