using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    public class ResturantType
    {
        [Key]
        public int Id { get; set; }

        [GenericStringLength(100)]
        [GenericRequired]
        public string DropDownTitle { get; set; }
    }
    //public enum ResturantType:byte
    //{
    //    [Description("رستوران")]
    //    Resturant,
    //    [Description("فست فود")]
    //    FastFood,
    //    [Description("کافی شاپ")]
    //    CofeeShop
    //}


}