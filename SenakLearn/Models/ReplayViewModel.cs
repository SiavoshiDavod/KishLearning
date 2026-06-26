using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class Replayt1ViewModel
    {[Key]
        public int Id { get; set; }
        public int Idques { get; set; }
        public int IdUser { get; set; }
        public int IdClass { get; set; }
        public int IdExam { get; set; }
        [Display(Name = "سوال")]
        public string Ques { get; set; }
        [Display(Name = "پاسخ")]
        [DataType(DataType.MultilineText)]
        public string Replay { get; set; }


    }
    public class Replayt4ViewModel
    {
        [Key]
        public int Id { get; set; }

        public int Idques { get; set; }
        public int IdExam { get; set; }
        public int IdClass { get; set; }
        [Display(Name = "سوال")]
        public string Ques { get; set; }
        [Display(Name = "گزینه اول")]
        public string Switch1 { get; set; }
        [Display(Name = "گزینه دوم")]
        public string Switch2 { get; set; }
        [Display(Name = "گزینه سوم")]
        public string Switch3 { get; set; }
        [Display(Name = "گزینه چهارم")]
        public string Switch4 { get; set; }
        [Display(Name = "پاسخ")]
        [RegularExpression("[1-4]",ErrorMessage="جواب باید عدد 1 تا 4 باشد")]
        public int? SwitchReplay { get; set; }


    }
}