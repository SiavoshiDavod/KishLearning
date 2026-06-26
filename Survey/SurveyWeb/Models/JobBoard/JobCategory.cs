using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class JobCategory : BaseEntity
    {
        [Display(Name ="عنوان")]
        [Required(ErrorMessage ="لطفا این فیلد را پر کنید")]
        public string Title { get; set; }

        public int UserID { get; set; }
    }
}