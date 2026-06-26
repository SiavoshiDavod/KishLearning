using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models.Security
{
    [Description("نقش")]
    public class Role: BaseEntity
    {
        public ICollection<Permisstion> Permisstions { get; set; }

        [GenericStringLength(100)]
        [Display(Name = "نام  ")]
        [GenericRequired]
        public string Name { get; set; }
    }
   
}