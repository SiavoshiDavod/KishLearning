using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models.Resturan
{
    public class PaymentType : BaseEntity
    {
        [GenericStringLength(500), GenericRequired, Display(Name = "توضيحات")]
        public string Desc { get; set; }

        [Display(Name = "حق عضویت سالیانه(ريال)")]
        public int Price { get; set; }

        [Display(Name = "رتبه ")]
        public byte Degree { get; set; }

        [Display(Name = "آرشيو شده")]
        public bool Archive { get; set; }

        [Display(Name = "شرح خدمات")]
        public int ResturantTypeId { get; set; }

        [ForeignKey("ResturantTypeId")]
        public ResturantType ResturantType { get; set; }
    }
}