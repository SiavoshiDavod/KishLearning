using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models.BaseInfo
{
    public class CompanyType
    {
        [Key]
        public int Id { get; set; }
        [GenericStringLength(100)]
        [GenericRequired]
        [Display(Name = "مدرک")]
        public string DropDownTitle { get; set; }
    }
}