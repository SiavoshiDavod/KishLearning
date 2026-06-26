using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class ExamedViewModel
    {
        public ExamedViewModel()
        {
        }
        [Key]
        public int id { get; set; }

        [Display(Name = "عنوان آزمون")]
        public string name { get; set; }
        [Display(Name = "گروه")]
        public string Group { get; set; }
        [Display(Name = "دوره")]
        public string Cours { get; set; }
        [Display(Name = "تاریخ شرکت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public System.DateTime createDate { get; set; }
        [Display(Name = "نتیجه آزمون")]
        public string status { get; set; }
    }
}