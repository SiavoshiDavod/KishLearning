using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("گزینه های سوال نظرسنجی")]
    public class SurveyQuestionOption : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "نام گزینه")]
        [GenericRequired, GenericStringLength(1000)]
        public string QuestionOption { get; set; }

        [ GenericStringLength(100)]
        [Display(Name = "نام عکس ")]
        public string QuestionOptionUrl { get; set; }
        [Display(Name = "عرض عکس ")]
        public Int16 Width { get; set; }
        [Display(Name = "طول عکس ")]
        public Int16 Height { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام سوال")]
        public int SurveyQuestionId { get; set; }
        [ForeignKey("SurveyQuestionId")]
        [JsonIgnore]
        public SurveyQuestion SurveyQuestion { get; set; }
    }
}