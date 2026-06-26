using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class JobPositionWrapper
    {
        public int ID { get; set; }
        
        public int UserID { get; set; }
        
        public string CompanyName { get; set; }
        
        public string UserName { get; set; }
        
        [Display(Name ="دسته بندی")]
        public int JobCategoryID { get; set; }

        [Display(Name = "دسته بندی")]
        public string JobCategoryName { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "نوع همکاری")]
        public EnumCooperationType CooperationType { get; set; }

        [Display(Name = "نوع همکاری")]
        public string CooperationTypeName { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "مهارت های لازم")]
        public string RequiredSkills { get; set; }

        [Display(Name = "جنسیت")]
        public EnumGenderType Gender { get; set; }

        [Display(Name = "جنسیت")]
        public string GenderName { get; set; }

        [Display(Name = "وضعیت نظام وظیفه")]
        public EnumMilitaryServiceType MilitaryServiceStatus { get; set; }

        [Display(Name = "وضعیت نظام وظیفه")]
        public string MilitaryServiceStatusName { get; set; }

        [Display(Name = "سابقه کار")]
        public EnumWorkExperienceType WorkExperience { get; set; }

        [Display(Name = "سابقه کار")]
        public string WorkExperienceName { get; set; }

        [Display(Name = "موقعیت مکانی")]
        public string Location { get; set; }
        
        [Display(Name = "حقوق از")]
        public string SalaryFrom { get; set; }

        [Display(Name = " حقوق تا")]
        public string SalaryTo { get; set; }
        
        public byte[] Resume { get; set; }

        public JobRequest.StatusType RequestStatus { get; set; }
        
        public string RequestStatusName { get; set; }

        [Display(Name = "انتشار عمومی")]
        public bool IsPublic { get; set; }

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