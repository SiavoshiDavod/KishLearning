using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Person
{
    [DisplayName("دوره پرسنل")]
    public class Person_Course : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        [GenericRequired]
        [Display(Name = "عنوان")]
        [GenericMaxLength(50)]
        [Excel(IsColumnOut = true)]
        public string Title { get; set; }

        [Display(Name = "مجری دوره")]
        [GenericMaxLength(50)]
        [Excel(IsColumnOut = true)]
        public string CourseLeader { get; set; }
        [Display(Name = "کد دوره")]
        [Excel(IsColumnOut = true)]
        public string Code { get; set; }
        [Display(Name = "مدت دوره")]
        [Excel(IsColumnOut = true)]
        public int? Duration { get; set; }
        [Display(Name = "توضیحات دوره")]
        [Excel(IsColumnOut = true)]
        public string Description { get; set; }
        [Display(Name = "مدرک دوره")]
        [Excel(IsColumnOut = true)]
        public string CertificateFile { get; set; }
        [Display(Name = "شروع دوره")]
        [Excel(IsColumnOut = true)]
        public string FromDate { get; set; }
        [Display(Name = "پایان دوره")]
        [Excel(IsColumnOut = true)]
        public string ToDate { get; set; }
        [Display(Name = "تاریخ صدور مدرک")]
        [Excel(IsColumnOut = true)]
        public string CertificateDate { get; set; }

    }
}