using System.ComponentModel.DataAnnotations;

namespace SenakLearn.Models
{
    public class NewsGroup : BaseEntity
    {
        [Display(Name = "تیتر"), GenericRequired, GenericStringLength(200)]
        public string Title { get; set; }
    }
}