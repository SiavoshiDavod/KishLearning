using System;
using System.ComponentModel.DataAnnotations;

namespace SenakLearn.Models
{
    public class StudentSupport:ParentChildEntity
    {
        [Display(Name = "ویدیو")]
        public Guid VideoId { get; set; }
    }
}