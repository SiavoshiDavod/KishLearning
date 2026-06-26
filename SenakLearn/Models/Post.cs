using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    [DisplayName("سمت")]
    public class Post : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        [GenericRequired]
        [Display(Name = "عنوان")]
        [GenericMaxLength(50)]
        public string Title { get; set; }
    }
}