using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class WorkExperience : BaseEntity
    {
        public int UserID { get; set; }
        
        [Display(Name ="از تاریخ")]
        public string FromDate { get; set; }
        
        [Display(Name ="تا تاریخ")]
        public string ToDate { get; set; }
               
        [Display(Name ="نام شرکت")]
        public string CompanyName { get; set; }
        
        [Display(Name ="سمت")]
        public string Position { get; set; }

        [NotMapped]
        public string Username { get; set; }

    }
}