using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("پاسخ های نظرسنجی")]
    public class SurveyAnswer : BaseEntity
    {

        [System.ComponentModel.DataAnnotations.Display(Name = "پاسخ دهنده")]
        public int SurveyUserAnswerId { get; set; }
        [ForeignKey("SurveyUserAnswerId")]
        [JsonIgnore]
        public SurveyUserAnswer SurveyUserAnswer { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام سوال")]
        public int SurveyQuestionId { get; set; }
        [ForeignKey("SurveyQuestionId")]
        [JsonIgnore]
        public SurveyQuestion SurveyQuestion { get; set; }

        [GenericRequired, GenericStringLength(1000)]
        [System.ComponentModel.DataAnnotations.Display(Name = "جواب سوال")]
        public string Result { get; set; }

        [GenericStringLength(100)]
        [System.ComponentModel.DataAnnotations.Display(Name = "جواب سوالات چند گزینه ای")]
        public string SurveyQuestionOptionId { get; set; }
        [NotMapped]
        public int SurveyEntityId { get; set; }
    }
}