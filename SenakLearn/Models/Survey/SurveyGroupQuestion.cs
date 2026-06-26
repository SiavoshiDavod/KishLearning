using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("گروه سوال نظرسنجی")]
    public class SurveyGroupQuestion: BaseEntity
    {
        public SurveyGroupQuestion()
        {
            SurveyQuestion = new HashSet<SurveyQuestion>();
        }
        [System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        public int SurveyEntityId { get; set; }
        [ForeignKey("SurveyEntityId")]
        [JsonIgnore]
        public SurveyEntity SurveyEntity { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "عنوان گروه سوال")]
        [GenericRequired, GenericStringLength(100)]
        public string SurveyGroupQuestionTitle { get; set; }

        [JsonIgnore]
        public ICollection<SurveyQuestion> SurveyQuestion { get; set; }
    }
}