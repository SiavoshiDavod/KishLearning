using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class OnlineClassAccoration : BaseEntity
    {
        [Required, Display(Name = "عنوان")]
        public string Name { get; set; }
        public virtual ICollection<OnlineClassAccorationDetails> Details { get; set; }
        [NotMapped]
        public override DateTime CreatedDate { get => base.CreatedDate; set => base.CreatedDate = value; }
        [NotMapped]
        public override DateTime? UpdateDate { get => base.UpdateDate; set => base.UpdateDate = value; }
    }
}