using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseModel
{
    [Description("گروه اختصاصی نظرسنجی")]
    public class SurveyPrivateGroup : BaseEntity
    {
        [GenericStringLength(100)]
        [Display(Name = "نام  ")]
        [GenericRequired]
        public string Name { get; set; }
    }

}
