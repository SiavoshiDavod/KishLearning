using SurveyWeb.Models.BaseInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    public class ResturantPersonelEducation : BaseEntity
    {
        [ForeignKey("ResturantPersonelId")]
        public ResturantPersonel ResturantPersonel { get; set; }
        public int ResturantPersonelId { get; set; }
        [Display(Name = "مقطع تحصیلی")]
        public int EducationId { get; set; }
        [ForeignKey("EducationId")]
        public Education Education { get; set; }
        [NotMapped]
        public string EducationName => Education?.DropDownTitle;

        [Display(Name = "رشته تحصیلی")]
        [GenericStringLength(100)]
        public string EducationField { get; set; }
        [Display(Name = "محل تحصیل")]
        [GenericStringLength(100)]
        public string EducationLocation { get; set; }

        [Display(Name = "آدرس محل تحصیل")]
        [GenericStringLength(200), DataType(DataType.MultilineText)]
        public string EducationAddressStay { get; set; }

    }
    public class ResturantPersonelLanguage : BaseEntity
    {
        [ForeignKey("ResturantPersonelId")]
        public ResturantPersonel ResturantPersonel { get; set; }
        public int ResturantPersonelId { get; set; }

        [Display(Name = "نام زبان")]
        [GenericStringLength(100)]
        public string LanguageName { get; set; }

        [Display(Name = "وضعیت خواندن")]
        [GenericStringLength(10)]
        public string LanguageReading { get; set; }

        [Display(Name = "وضعیت نوشتن")]
        [GenericStringLength(10)]
        public string LanguageWriting { get; set; }

        [Display(Name = "وضعیت مکالمه")]
        [GenericStringLength(10)]
        public string LanguageSpeaking { get; set; }

    }
    public class ResturantPersonelCourse : BaseEntity
    {
        [ForeignKey("ResturantPersonelId")]
        public ResturantPersonel ResturantPersonel { get; set; }
        public int ResturantPersonelId { get; set; }

        [Display(Name = "نام دوره")]
        [GenericStringLength(100)]
        public string CourseName1 { get; set; }

        [Display(Name = "مدت دوره")]
        [GenericStringLength(10)]
        public string CourseDuration1 { get; set; }

        [Display(Name = "محل گذراندن دوره")]
        [GenericStringLength(100)]
        public string CourseLocation1 { get; set; }

    }
    public class ResturantPersonelJob : BaseEntity
    {
        [ForeignKey("ResturantPersonelId")]
        public ResturantPersonel ResturantPersonel { get; set; }
        public int ResturantPersonelId { get; set; }

        [Display(Name = "عنوان شغل گذشته")]
        [GenericStringLength(100)]
        public string LastJobPosition1 { get; set; }

        [Display(Name = "نام محل کار گذشته")]
        [GenericStringLength(100)]
        public string LastJobName1 { get; set; }

        [Display(Name = "تاریخ شروع به کار")]
        [GenericStringLength(10)]
        public string LastStartDate1 { get; set; }

        [Display(Name = "تاریخ خاتمه کار")]
        [GenericStringLength(10)]
        public string LastEndDate1 { get; set; }
    }

    [Description("ثبت اطلاعات پرسنل شاغل در آن مرکز")]
    public class ResturantPersonel : BaseEntity
    {
        public ICollection<ResturantPersonelJob> ResturantPersonelJob { get; set; }
        public ICollection<ResturantPersonelCourse> ResturantPersonelCourse { get; set; }
        public ICollection<ResturantPersonelLanguage> ResturantPersonelLanguage { get; set; }
        public ICollection<ResturantPersonelEducation> ResturantPersonelEducation { get; set; }
        public int ResturantId { get; set; }
        [ForeignKey("ResturantId")]
        public Resturant Resturant { get; set; }

        [GenericStringLength(100)]
        [DisplayName(" عکس")]
        public string ImageUrl { get; set; }

        [Display(Name = "نام ")]
        [GenericStringLength(100)]
        public string Name { get; set; }
        [GenericRequired]
        [Display(Name = "نام خانوادگی")]
        [GenericStringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "نام پدر ")]
        [GenericStringLength(100)]
        public string FatherName { get; set; }

        [NotMapped]
        public string BirthdayShamsi
        {
            get { return Birthday?.ToPersianDate(); }
            set { Birthday = value.ToGregorianDate(); }
        }

        [Display(Name = " تاریخ تولد")]
        public DateTime? Birthday { get; set; }

        [GenericStringLength(20)]
        [Display(Name = " محل تولد")]
        public string BirthdayLocation { get; set; }

        [GenericStringLength(10)]
        [Display(Name = "شماره شناسنامه ")]
        public string CodeNumber { get; set; }

        [GenericStringLength(10)]
        [Display(Name = "شماره ملی ")]
        public string NationalCode { get; set; }

        [GenericStringLength(10)]
        [Display(Name = "شماره گذرنامه ")]
        public string PassportNumber { get; set; }

        [GenericStringLength(10)]
        [Display(Name = " تابعیت ")]
        public string Nationality { get; set; }

        [Display(Name = " جنسیت ")]
        public bool IsMan { get; set; }

        [Display(Name = " وضعیت تاهل ")]
        public bool IsMarried { get; set; }

        ///////////////////////////////////////////////////////////

        [Display(Name = "آدرس محل سکونت")]
        [GenericStringLength(500), DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "شهر محل اقامت")]
        [GenericStringLength(20)]
        public string CityStay { get; set; }

        [Display(Name = "آدرس محل اقامت")]
        [GenericStringLength(500), DataType(DataType.MultilineText)]
        public string AddressStay { get; set; }

        [GenericStringLength(10)]
        [Display(Name = "کد پستی")]
        public string PostalCode { get; set; }

        [Display(Name = "تلفن ثابت")]
        [GenericStringLength(20)]
        public string Tel { get; set; }

        [Display(Name = "تلفن همراه ")]
        [GenericRequired]
        [GenericStringLength(20)]
        public string Mobile { get; set; }


        ////////////////////////////سوابق تحصیلی///////////////////////////////

        [Display(Name = "آخرین مقطع تحصیلی")]
        public int? EducationId { get; set; }
        [ForeignKey("EducationId")]
        public Education Education { get; set; }
        [NotMapped]
        public string EducationName => Education?.DropDownTitle;

        [Display(Name = "رشته تحصیلی")]
        [GenericStringLength(100)]
        public string EducationField { get; set; }
        [Display(Name = "محل تحصیل")]
        [GenericStringLength(100)]
        public string EducationLocation { get; set; }

        [Display(Name = "آدرس محل تحصیل")]
        [GenericStringLength(200), DataType(DataType.MultilineText)]
        public string EducationAddressStay { get; set; }

        ////////////////////////////سوابق شغلی///////////////////////////////


        [Display(Name = "سمت شغلی فعلی در این محل کار")]
        [GenericStringLength(100)]
        public string JobPosition { get; set; }


        [Display(Name = "عنوان شغل گذشته")]
        [GenericStringLength(100)]
        public string LastJobPosition { get; set; }

        [Display(Name = "نام محل کار گذشته")]
        [GenericStringLength(100)]
        public string LastJobName { get; set; }

        [Display(Name = "تاریخ شروع به کار")]
        [GenericStringLength(10)]
        public string LastStartDate { get; set; }

        [Display(Name = "تاریخ خاتمه کار")]
        [GenericStringLength(10)]
        public string LastEndDate { get; set; }
        ////////////////////////////سوابق دوره آموزشی///////////////////////////////


        [Display(Name = "نام دوره")]
        [GenericStringLength(100)]
        public string CourseName { get; set; }

        [Display(Name = "مدت دوره")]
        [GenericStringLength(10)]
        public string CourseDuration { get; set; }

        [Display(Name = "محل گذراندن دوره")]
        [GenericStringLength(100)]
        public string CourseLocation { get; set; }
        ////////////////////////////آشنایی با زبان خارجه///////////////////////////////


        [Display(Name = "نام زبان")]
        [GenericStringLength(100)]
        public string LanguageName { get; set; }

        [Display(Name = "وضعیت خواندن")]
        [GenericStringLength(10)]
        public string LanguageReading { get; set; }

        [Display(Name = "وضعیت نوشتن")]
        [GenericStringLength(10)]
        public string LanguageWriting { get; set; }

        [Display(Name = "وضعیت مکالمه")]
        [GenericStringLength(10)]
        public string LanguageSpeaking { get; set; }

        [Display(Name = "اگر در کشور دیگری اقامت داشته اید، ذکر کنید")]
        [GenericStringLength(100)]
        public string OtherCountry { get; set; }

    }
}