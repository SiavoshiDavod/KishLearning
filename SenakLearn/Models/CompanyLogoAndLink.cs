using System.ComponentModel.DataAnnotations;

namespace SenakLearn.Models
{
    public class CompanyLogoAndLink : BaseEntity
    {
        [Display(Name = "نام "), GenericStringLength(200)]
        public string Name { get; set; }
        [Display(Name = "عکس "), GenericStringLength(200)]
        public string ImageUrl { get; set; }
        [Display(Name = "لینک "), GenericStringLength(900)]
        public string Link { get; set; }
    }
}