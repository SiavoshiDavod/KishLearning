using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("کارتابل")]
    public class Cartable : BaseEntity
    {
        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام کارتابل ")]
        public string Name { get; set; }

        [Display(Name = "نوع کارتابل ")]
        public CartableType CartableType { get; set; }

        [Display(Name = "وضعیت اولیه است؟")]
        public bool IsFirstState { get; set; }

        [Display(Name = "آخرین وضعیت است؟")]
        public bool IsLastState { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        [InverseProperty("FromCartable")]
        public ICollection<CartableRelation> From { get; set; }

        //[InverseProperty("ToCartable")]
        //public ICollection<CartableRelation> To { get; set; }
        public ICollection<CartableUserAccess> Users { get; set; }
        [NotMapped]
        public string CartableTypeName { get { return CartableType.ToString(); } set { } }
    }
    public enum CartableType
    {
        Suggestion,
        Idea,
        ContactUs,
        Complaint,
        Resturant
    }
}