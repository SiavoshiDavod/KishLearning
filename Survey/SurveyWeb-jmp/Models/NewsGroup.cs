using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    public class NewsGroup:BaseEntity
    {
        [Display(Name = "تیتر"), GenericRequired, GenericStringLength(200)]
        public string Title { get; set; }
    }
}