using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SenakLearn.Models
{
    [Description("پاسخ های آزمون")]
    public class AzmoonAnswer : BaseEntity
    {

        [System.ComponentModel.DataAnnotations.Display(Name = "پاسخ دهنده")]
        public int AzmoonUserAnswerId { get; set; }
        [ForeignKey("AzmoonUserAnswerId")]
        [JsonIgnore]
        public AzmoonUserAnswer AzmoonUserAnswer { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام سوال")]
        public int AzmoonQuestionId { get; set; }
        [ForeignKey("AzmoonQuestionId")]
        [JsonIgnore]
        public AzmoonQuestion AzmoonQuestion { get; set; }

        [GenericRequired, GenericStringLength(1000)]
        [System.ComponentModel.DataAnnotations.Display(Name = "جواب سوال")]
        public string Result { get; set; }

        [GenericStringLength(100)]
        [System.ComponentModel.DataAnnotations.Display(Name = "جواب سوالات چند گزینه ای")]
        public string AzmoonQuestionOptionId { get; set; }
        [NotMapped]
        public int AzmoonEntityId { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نمره دریافت شده")]
        public double Score { get; set; }

        [NotMapped]
        public List<int> AzmoonAnswerQuestionOptionId => string.IsNullOrEmpty(AzmoonQuestionOptionId) ? new List<int>() : AzmoonQuestionOptionId.Split(',').Select(Int32.Parse).ToList();
    }
}