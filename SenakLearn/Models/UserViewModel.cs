using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int Id { get; set; }
        [GenericRequired]
        [Display(Name = "کد ملی")]
        [GenericMaxLength(10)]
        [GenericMinLength(10)]  
        ////[RegularExpression(@"\d",ErrorMessage = "کد ملی را صحیح وارد کنید")]
        public string NationaCode { get; set; }
        [GenericRequired]
        [Display(Name = "نام کاربری")]
        [GenericMaxLength(20)]
        public string UserName { get; set; }
        [Display(Name = "نام")]
        [GenericRequired]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        [GenericRequired]
        public string Family { get; set; }
        [GenericRequired]
        [Display(Name = "کلمه عبور")]
        [GenericMaxLength(20)]
        [GenericMinLength(3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [GenericRequired]
        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "تکرار کلمه عبور با کلمه عبور مطابقت ندارد")]
        public string RePassword { get; set; }
        [Display(Name = "تاریخ ثبت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public string DateRegister { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool Status { get; set; }
        [Display(Name = "نقش کاربر")]
        public int IdRole { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "ایمیل معتبر نمی باشد")]
        [Display(Name = "ایمیل"), DataType(DataType.EmailAddress), GenericRequired]
        public string Email { get; set; }
        [GenericRequired]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "همراه معتبر نمی باشد")]
        [Display(Name = "همراه")]
        public string Mobile { get; set; }
        [Display(Name = "آدرس"), GenericStringLength(500)]
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string PassAdobe { get; set; }
        public string googlerecaptchaRegister { get; set; }

        [Display(Name = "استان")]
        public Province Province { get; set; }

        [Display(Name = "شهر"), GenericStringLength(50)]
        public string City { get; set; }

        [Display(Name = "مدرک تحصیلی"), GenericStringLength(50)]
        public string Education { get; set; }

        [Display(Name = "تخصص"), GenericStringLength(50)]
        public string Expertise { get; set; }

        [Display(Name = "نام پدر"), GenericStringLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "محل تولد"), GenericStringLength(50)]
        public string BirthLocation { get; set; }

        [Display(Name = "شماره ثابت"), GenericStringLength(15)]
        public string Tel { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDay { get; set; }

        public virtual string BirthDayShamsi
        {
            get => BirthDay==null?"": BirthDay.Value.ToPersianDate();
            set => BirthDay = value.ToGregorianDate();
        }

        [Display(Name = "شماره شناسنامه"), GenericStringLength(15)]
        public string Shenasname { get; set; }
        public int? TypeUser { get; set; }
        [NotMapped]
        [Display(Name = "نوع کاربر"), GenericStringLength(15)]
        public string TypeUserName { get; set; }

        public int? PostId { get; set; }
        [NotMapped]
        [Display(Name = "پست"), GenericStringLength(15)]
        public string PostName { get; set; }
        public int? OrgId { get; set; }
        [NotMapped]
        [Display(Name = "سازمان"), GenericStringLength(15)]
        public string OrgName { get; set; }
    }

    public class LoginViewModel
    {
        [GenericRequired]
        [Display(Name = "نام کاربری")]
        public string userNameLogin { get; set; }
        [GenericRequired]
        [GenericMaxLength(20)]
        [GenericMinLength(3)]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
        [GenericRequired]
        [Display(Name = "مرا به خاطر بسپار")]
        public bool Remember { get; set; }
        public string googlerecaptchaLogin { get; set; }
    }

    public class ChangePassViewModel
    {
        [GenericRequired]
        [Display(Name = "پسورد قبلی")]
        [DataType(DataType.Password)]
        public string OldPass { get; set; }
        [GenericRequired]
        [Display(Name = "پسورد جدید")]
        [DataType(DataType.Password)]
        [GenericMaxLength(20)]
        [GenericMinLength(3)]
        public string Pass { get; set; }
        [GenericRequired]
        [Display(Name = "تکرار پسورد جدید")]
        [DataType(DataType.Password)]
        [Compare("Pass", ErrorMessage = "تکرار کلمه عبور با کلمه عبور مطابقت ندارد")]
        public string rePass { get; set; }
        public bool IsAdobe { get; set; }
        public string googlerecaptcha { get; set; }
    }
    public class reCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}