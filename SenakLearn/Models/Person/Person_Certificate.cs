using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Person
{
    [DisplayName("مدرک پرسنل")]
    public class Person_Certificate : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        [Display(Name = "دوره")]
        public int Person_CourseId { get; set; }

        [NotMapped]
        public string Person_Course { get; set; }
        [Display(Name = "مدرس دوره")]
        public int Person_TeacherId { get; set; }
        [Display(Name = "مدرس دوره")]
        [NotMapped]
        public string Person_Teacher { get; set; }
        [Display(Name = "ایمیل استاد")]
        [NotMapped]
        public string Teacher_Email { get; set; }
        [Display(Name = "همراه استاد")]
        [NotMapped]
        public string Teacher_Mobile { get; set; }
        [Display(Name = "مدرک استاد")]
        [NotMapped]
        public string Teacher_Certificate { get; set; }
        [Display(Name = "تخصص استاد")]
        [NotMapped]
        public string Teacher_Expertise { get; set; }
        [Display(Name = "کد دوره")]
        [NotMapped]
        public string Course_Code { get; set; }
        [Display(Name = "مدت دوره")]
        [NotMapped]
        public int? Course_Duration { get; set; }
        [Display(Name = "توضیحات دوره")]
        [NotMapped]
        public string Course_Description { get; set; }
        public string Code { get; set; }
        [Display(Name = "تاریخ صدور مدرک")]
        public string IssueDate { get; set; }
        [Display(Name = "مدت دوره")]
        public int Duration { get; set; }
        public string CertificateFile { get; set; }
        [Display(Name = "دوره")]
        [NotMapped]
        public string CourseName { get; set; }
        [Display(Name = "مجری دوره")]
        [NotMapped]
        public string CourseLeader { get; set; }
        public int UserId { get; set; }
        [Display(Name = "کاربر")]
        [NotMapped]
        public string UserName { get; set; }
        //[Display(Name = "تصویر مدرک")]
        public string UrlCertificate { get; set; }
        [NotMapped]
        public string FromDate { get; set; }
        [NotMapped]
        public string ToDate { get; set; }

        [Display(Name = "نوع مجری دوره")]
        public bool InOut { get; set; }
        [NotMapped]
        public string InOutTitle { get; set; }
        [NotMapped]
        public string TypeCourse { get; set; }
    }
}