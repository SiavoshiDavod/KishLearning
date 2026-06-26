using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    [Description("سوالات متداول")]
    public class Faq:BaseEntity
    {
        [DataType(DataType.MultilineText)]
        [Display(Name ="سوال"),GenericRequired]
        public string Question { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name ="پاسخ"), GenericRequired]
        public string Answer { get; set; }
    }
}