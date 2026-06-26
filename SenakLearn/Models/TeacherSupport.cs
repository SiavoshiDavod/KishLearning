using System;
using System.ComponentModel.DataAnnotations;

namespace SenakLearn.Models
{
    public class TeacherSupport : ParentChildEntity
    {
        [Display(Name = "ویدیو")]
        public Guid VideoId { get; set; }
        
    }
}