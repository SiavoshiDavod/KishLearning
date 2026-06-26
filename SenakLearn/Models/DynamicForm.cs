using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Models
{
    public class DynamicForm
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "ترتیب")]
        public int Order { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "منو")]
        public int MenuId { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "تیتر")]
        public string Title { get; set; }
        public string Url { get; set; }
        [Display(Name = "ویدیو")]
        public Guid? InterviewPathVideo { get; set; }
        [Display(Name = "عکس")]
        public string Image { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "محتوا")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
        [ForeignKey("MenuId")]
        [Display(Name = "منو")]
        public Menu Menu { get; set; }
    }
}