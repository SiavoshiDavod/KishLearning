using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.JobBoard
{
    public class JobRequestWrapper : BaseEntity
    {
        public int UserID { get; set; }
        
        [Display(Name ="نام کاربری")]
        public string UserName { get; set; }
        
        [Display(Name ="نام")]
        public string FirstName { get; set; }
        
        [Display(Name ="نام خانوادگی")]
        public string LastName { get; set; }
        
        [Display(Name ="تلفن")]
        public string Phone { get; set; }
        
        [Display(Name ="شرکت")]
        public string Company { get; set; }
                        
        public int JobPositionID { get; set; }
        
        [Display(Name ="عنوان موقعیت شغلی")]
        public string JobPositionTitle { get; set; }
        
        [Display(Name ="فایل رزومه")]
        public byte[] Resume { get; set; }

        public StatusType Status { get; set; }
        
        [Display(Name ="وضعیت")]
        public string StatusName { get; set; }

        public enum StatusType
        {
            [Display(Name = "در حال انتظار")]
            Waiting,

            [Display(Name = "تایید شده")]
            Confirmed,

            [Display(Name = "لفو شده")]
            Rejected

        }
    }
}