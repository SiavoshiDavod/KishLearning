using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class OnlineClassRequest:BaseEntity
    {
        [Display(Name = "کاربر")]
        public int UserId { get; set; }
        [Display(Name = "کلاس آنلاین")]
        public int OnlineClassId { get; set; }
        [Display(Name = "اطلاع رسانی شده است")]
        public bool Notices { get; set; }
        [ForeignKey("UserId")]
        public virtual learn_user learn_user { get; set; }
        [ForeignKey("OnlineClassId")]
        public virtual OnlineClass OnlineClass { get; set; }
    }
}