using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("گزینه های سوال آزمون")]
    public class AzmoonQuestionOption : BaseEntity
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
        public int AzmoonQuestionId { get; set; }
        [ForeignKey("AzmoonQuestionId")]
        [JsonIgnore]
        public AzmoonQuestion AzmoonQuestion { get; set; }

        [Display(Name = "گزینه صحیح")]
        public bool IsCorrect { get; set; }
    }
}