using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class JobRequest : BaseEntity
    {
        public int UserID { get; set; }
        
        public int JobPositionId { get; set; }
               
        public StatusType Status { get; set; }

        public enum StatusType
        {
            [Display(Name ="در حال انتظار")]
            Waiting,
            
            [Display(Name ="تایید شده")]
            Accepted,
            
            [Display(Name ="لغو شده")]
            Rejected

        }
    }
}