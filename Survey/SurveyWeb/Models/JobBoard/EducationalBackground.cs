using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class EducationalBackground : BaseEntity
    {
        public int UserID { get; set; }
        
        [Display(Name ="از تاریخ")]
        public string FromDate { get; set; }
        
        [Display(Name ="تا تاریخ")]
        public string ToDate { get; set; }
               
        [Display(Name ="موسسه")]
        public string InstituteName { get; set; }
        
        [Display(Name ="رشته")]
        public string Field { get; set; }

        [NotMapped]
        public string Username { get; set; }

    }
}