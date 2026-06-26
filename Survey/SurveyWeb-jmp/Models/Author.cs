using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public class Author:BaseEntity
    {
        [Display(Name = "تاریخ تولد ")]
        public DateTime? BirthDay { get; set; }
        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام ", Description = "نام شناسنامه خود را وارد کنید")]
        public string Name { get; set; }

        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام خانوادگی ")]
        public string Family { get; set; }

        [GenericStringLength(100)]
        [Display(Name = "مدرک و رشته تحصیلی")]
        public string Education { get; set; }

        [Display(Name = "ایمیل", Description = "لطفا ایمیل خود را وارد کنید")]
        [GenericRequired, GenericStringLength(50), DataType(DataType.EmailAddress, ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "تلفن همراه")]
        [GenericStringLength(11)]
        public string Mobile { get; set; }

        [Display(Name = "شماره تماس")]
        [GenericStringLength(20)]
        public string Tel { get; set; }
        [Display(Name = "عکس "), GenericStringLength(100)]
        public string ImageUrl { get; set; }
        [GenericStringLength(2000)]
        [Display(Name = "توضیحات"), AllowHtml]
        public string Description { get; set; }

        [NotMapped]
        public string BirthDayShamsi
        {
            get { return BirthDay?.ToPersianDate(); }
            set { BirthDay = value.ToGregorianDate(); }
        }
        [NotMapped]
        public string FullName => Name + " " + Family;
    }
}