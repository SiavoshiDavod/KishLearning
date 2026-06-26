using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SenakLearn.Models.Person
{
    [DisplayName("اساتید پرسنل")]
    public class Person_Teacher : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        [GenericRequired]
        [Display(Name = "عنوان")]
        [GenericMaxLength(50)]
        public string TeacherName { get; set; }
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "همراه معتبر نمی باشد")]
        [Display(Name = "همراه")]
        public string Mobile { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "ایمیل معتبر نمی باشد")]
        [Display(Name = "ایمیل"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "مدرک تحصیلی")]
        [NotMapped]
        public string CertificateName { get; set; }
        public int? CertificateId { get; set; }
        [Display(Name = "تخصص")]
        public string Expertise { get; set; }

    }
}