using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class EmployeeProfile : BaseEntity
    {

        public int UserID { get; set; }

        [Display(Name = "درباره من")]
        [MaxLength(200, ErrorMessage = "حداکثر طول مجاز 200 کاراکتر است")]
        public string AboutMe { get; set; }

        [Display(Name = "پست الکترونیک")]
        [DataType(DataType.EmailAddress, ErrorMessage = "پست الکترونیک معتبر نیست")]
        public string Email { get; set; }

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "سال تولد")]
        public string BirthYear { get; set; }

        [Display(Name = "جنسیت")]
        public EnumEmployeeGenderType Gender { get; set; }

        [Display(Name = "وضعیت سربازی")]
        public EnumEmployeeMilitaryServiceType MilitaryStatus { get; set; }

        [Display(Name = "وضعیت تاهل")]
        public EnumEmployeeMaritalStatusType MaritalStatus { get; set; }

        [Display(Name = "استان سکونت")]
        public string ProvinceOfResidence { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "تخصص")]
        public string Specialty { get; set; }

        [Display(Name = "مهارت ها")]
        public string Skills { get; set; }

        [Display(Name = "زبان های مسلط")]
        public string Languages { get; set; }

        public string ProfileImageURI { get; set; }

        public byte[] ResumeFile { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsVerified { get; set; }

        [NotMapped]
        [Display(Name = "وضعیت سربازی")]
        public string MilitaryStatusTitle { get; set; }

        [NotMapped]
        [Display(Name = "جنیست")]
        public string GenderTitle { get; set; }

        [NotMapped]
        [Display(Name = "وضعیت تاهل")]
        public string MaritalStatusTitle { get; set; }

        [NotMapped]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        public enum EnumEmployeeGenderType
        {
            [Display(Name = "مرد")]
            Male,
            [Display(Name = "زن")]
            Female
        }

        public enum EnumEmployeeMilitaryServiceType
        {
            [Display(Name = "پایان خدمت")]
            Ended = 1,
            [Display(Name = "معافیت تحصیلی")]
            EducationPardon
        }

        public enum EnumEmployeeMaritalStatusType
        {
            [Display(Name = "مجرد")]
            Single = 1,
            [Display(Name = "متاهل")]
            Married
        }
    }
}