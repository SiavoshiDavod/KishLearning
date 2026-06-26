using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace SenakLearn.Models
{
    [Description("گروه نظرسنجی")]
    public class GroupSurvey:BaseEntity
    {
        public GroupSurvey()
        {
            SurveyEntities = new HashSet<SurveyEntity>(); ;
        }
        [GenericRequired,GenericStringLength(20)]
        [System.ComponentModel.DataAnnotations.Display(Name = "نام گروه")]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<SurveyEntity> SurveyEntities { get; set; }
    }
}