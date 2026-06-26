using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class ZarinpalPayment : BaseEntity
    {
        [Display(Name = "کاربر")]
        public int UserId { get; set; }
        [Display(Name = "ویدیو آموزشی")]
        public int? CourseId { get; set; }
        [Display(Name = "کلاس آنلاین")]
        public int? OnlineClassId { get; set; }
        public string Autohority { get; set; }
        [Display(Name = "کد رهگیری")]
        public long? RefId { get; set; }
        [Display(Name = "مبلغ")]
        public int Amount { get; set; }
        [Display(Name = "وضعیت پرداخت")]
        public int? Status { get; set; }
        [Display(Name = "فاکتور")]
        public long? FactorId { get; set; }
        [NotMapped]
        public string StatusS { get { return Status == null ? "ناموفق" : Payment.StatusZarinPal.Dic[Status.Value]; } set { StatusS = value; } }
        [ForeignKey("UserId")]
        public virtual learn_user learn_user { get; set; }
        [ForeignKey("OnlineClassId")]
        public virtual OnlineClass OnlineClass { get; set; }
        [ForeignKey("CourseId")]
        public virtual learn_cours learn_cours { get; set; }
    }
}