using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    [Description("کاربران پاسخ دهنده به نظرسنجی")]
    public class SurveyUserAnswer : BaseEntity
    {
        public SurveyUserAnswer()
        {
            SurveyAnswers = new HashSet<SurveyAnswer>();
        }

        public int? UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "آی پی")]
        [ GenericStringLength(20)]
        public string Ip { get; set; }
        [JsonIgnore]
        public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        public int SurveyEntityId { get; set; }
        //[ForeignKey("SurveyEntityId")]
        //public SurveyEntity SurveyEntity { get; set; }
    }
}