using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    public class Menu:BaseEntity
    {
        public Menu()
        {
            MenuSubs = new List<MenuSub>();
        }
        [Display(Name = "ترتیب")]
        public int Order { get; set; }
        [Display(Name = "تیتر"), GenericRequired, GenericStringLength(200)]
        public string Title { get; set; }
        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        public ICollection<MenuSub> MenuSubs { get; set; }
    }
}