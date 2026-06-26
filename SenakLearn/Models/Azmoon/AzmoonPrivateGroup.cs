using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SenakLearn.Models.Security
{
    [Description("گروه اختصاصی آزمون")]
    public class AzmoonPrivateGroup:BaseEntity
    {
        [GenericStringLength(100)]
        [Display(Name = "نام  ")]
        [GenericRequired]
        public string Name { get; set; }
    }
}