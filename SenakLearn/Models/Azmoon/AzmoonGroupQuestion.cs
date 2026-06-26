using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("گروه سوال آزمون")]
    public class AzmoonGroupQuestion: BaseEntity
    {
        public AzmoonGroupQuestion()
        {
            AzmoonQuestion = new HashSet<AzmoonQuestion>();
        }
        //[System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        //public int AzmoonEntityId { get; set; }
        //[ForeignKey("AzmoonEntityId")]
        //[JsonIgnore]
        //public AzmoonEntity AzmoonEntity { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "عنوان گروه سوال")]
        [GenericRequired, GenericStringLength(100)]
        public string AzmoonGroupQuestionTitle { get; set; }

        [JsonIgnore]
        public ICollection<AzmoonQuestion> AzmoonQuestion { get; set; }
    }
}