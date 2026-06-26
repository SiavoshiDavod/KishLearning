using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "ترتیب")]
        public int Order { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "تیتر")]
        public string Title { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        public ICollection<DynamicForm> DynamicForms { get; set; }
    }
}