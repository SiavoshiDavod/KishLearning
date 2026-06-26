using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SenakLearn.Models.Security
{
    [Description("گروه اختصاصی نظرسنجی")]
    public class SurveyPrivateGroup:BaseEntity
    {
        [GenericStringLength(100)]
        [Display(Name = "نام  ")]
        [GenericRequired]
        public string Name { get; set; }
    }
}