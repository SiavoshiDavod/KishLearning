using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class Slider:BaseEntity
    {
        [GenericRequired]
        public string DropDownTitle { get; set; }
        [GenericRequired]
        public string IconPath { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string PreTitle { get; set; }
        public bool Archive { get; set; }
        [NotMapped]
        public string ImageForGrid => "<img src='/images/slider/" + this.IconPath+"'  width='150px' class='thumbnail' />";
    }
}