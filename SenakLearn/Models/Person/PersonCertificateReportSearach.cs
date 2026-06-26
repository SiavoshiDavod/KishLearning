using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SenakLearn.Models.Person
{
    public class PersonCertificateReportSearach
    {
        public  int? PersonCertificateId { get; set; }
        public int? Person_CourseId { get; set; }
        public string CourseTitle { get; set; }
        [Description("عنوان دوره")]
        [Excel(IsColumnOut = true)]
        public string Person_Course { get; set; }

        public int? TeacherId { get; set; }
        [Description("نام استاد")]
        [Excel(IsColumnOut = true)]
        public string TeacherName { get; set; }
        [Description("ایمیل استاد")]
        [Excel(IsColumnOut = true, Title = "")]
        public string Teacher_Email { get; set; }
        [Description("موبایل استاد")]
        [Excel(IsColumnOut = true, Title = "")]
        public string Teacher_Mobile { get; set; }
        [Description("مدرک استاد")]
        [Excel(IsColumnOut = true, Title = "")]

        public string Teacher_Certificate { get; set; }
        [Description("تخصص استاد")]
        [Excel(IsColumnOut = true, Title = "تخصص استاد")]

        public string Teacher_Expertise { get; set; }
        [Description("کد دوره")]
        [Excel(IsColumnOut = true, Title = "کد دوره")]
        public string Course_Code { get; set; }

        public int? Course_Duration { get; set; }
        [Description("توضیحات دوره")]
        [Excel(IsColumnOut = true, Title = "توضیحات دوره")]
        public string Course_Description { get; set; }
        [Description("کد مدرک")]
        [Excel(IsColumnOut = true, Title = "کد مدرک")]
        public string Code { get; set; }

        public int? IssueDate { get; set; }
        [Description("تاریخ صدور")]
        [Excel(IsColumnOut = true, Title = "تاریخ صدور")]
        public string IssueDatePersian { get; set; }
        [Description("مدت دوره")]
        [Excel(IsColumnOut = true)]
        public int? Duration { get; set; }
        public string CertificateFile { get; set; }
        public string CourseName { get; set; }
        [Description("مجری دوره")]
        [Excel(IsColumnOut = true, Title = "مجری دوره")]
        public string CourseLeader { get; set; }
        
        public string UserName { get; set; }
        public int? UserId { get; set; }
        public string UserIds { get; set; }
        public string UrlCertificate { get; set; }
        [Description("شروع دوره")]
        [Excel(IsColumnOut = true, Title = "شروع دوره")]
        public string CourseFromDate { get; set; }
        [Description("پایان دوره")]
        [Excel(IsColumnOut = true, Title = "پایان دوره")]
        public string CourseToDate { get; set; }
        [Description("پرسنل")]
        [Excel(IsColumnOut = true, Title = "پرسنل")]
        public string PersonName { get; set; }
        [Description("کد پرسنل")]
        [Excel(IsColumnOut = true, Title = "کد پرسنل")]
        public string PersonCode { get; set; }
        [Description("سازمان پرسنل")]
        [Excel(IsColumnOut = true, Title = "سازمان پرسنل")]
        public string PersonOrg { get; set; }
        public int? PersonOrgId { get; set; }
        public bool ShowPersonDetail { get; set; }
        [Description("نوع دوره")]
        [Excel(IsColumnOut = true)]
        public string InOut { get; set; }
        public string act { get; set; }
    }
}