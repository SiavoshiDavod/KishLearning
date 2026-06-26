using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Models.JobBoard
{
    public class JobPosition: BaseEntity
    {
        public int UserID { get; set; }
        
        [Display(Name ="دسته بندی")]
        [Required(ErrorMessage ="لطفا این فیلد را پر کنید.")]
        public int JobCategoryID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")] 
        public string Title { get; set; }

        [Display(Name = "نام شرکت")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public string Companyname { get; set; }

        [Display(Name = "نوع همکاری")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")] 
        public EnumCooperationType CooperationType { get; set; }

        [AllowHtml]
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")] 
        public string Description { get; set; }

        [Display(Name = "مهارت های لازم")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")] 
        public string RequiredSkills { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public EnumGenderType Gender { get; set; }

        [Display(Name = "وضعیت نظام وظیفه")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public EnumMilitaryServiceType MilitaryServiceStatus { get; set; }

        [Display(Name = "سابقه کار")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public EnumWorkExperienceType WorkExperience { get; set; }

        [Display(Name = "موقعیت مکانی")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public string Location { get; set; }
        
        [Display(Name = "حقوق از")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public string SalaryFrom { get; set; }

        [Display(Name = " حقوق تا")]
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید.")]
        public string SalaryTo { get; set; }

        [Display(Name = "انتشار عمومی")]
        public bool IsPublic { get; set; }

        public bool IsVerified { get; set; }

        public enum EnumGenderType
        {
            [Display(Name = "مهم نیست")]
            DontCare,
            [Display(Name ="مرد")]
            Male,
            [Display(Name ="زن")]
            Female
        }

        public enum EnumCooperationType
        {
            [Display(Name = "پاره وقت")]
            PartTime,
            [Display(Name = "تمام وقت")]
            FullTime
        }

        public enum EnumMilitaryServiceType
        {
            [Display(Name = "مهم نیست")]
            DontCare,
            [Display(Name = "پایان خدمت")]
            Ended,
            [Display(Name = "معافیت تحصیلی")]
            EducationPardon
        }

        public enum EnumWorkExperienceType
        {
            [Display(Name = "کم تر از یک سال")]
            LessThanOneYear,
            [Display(Name = "بین سه تا پنج سال")]
            BetweenOneAndThreeYear,
            [Display(Name = "کم تر از پنچ سال")]
            LessThanFiveYear
        }
    }
}