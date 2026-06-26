using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models.Resturan
{
    public enum PaymentTypeEnum : byte
    {
        [Description("حق پرداخت سالیانه بر اساس رتبه و نوع مرکز")]
        YearlyByDegree,
        [Description("حق پرداخت سالیانه بر اساس متراژ")]
        YearlyByMeter,
        [Description("کاریابی")]
        Karyabi,
        [Description("تبلیغات")]
        Tablighat
    }

    public class PaymentType : BaseEntity
    {
        [GenericStringLength(500), GenericRequired, Display(Name = "توضيحات")]
        public string Desc { get; set; }

        [Display(Name = "حق عضویت سالیانه(ريال)")]
        public int Price { get; set; }

        [Display(Name = "آرشيو شده")]
        public bool Archive { get; set; }




        [Display(Name = "رتبه ")]
        public byte Degree { get; set; }

        [Display(Name = "شرح خدمات")]
        public int? ResturantTypeId { get; set; }

        [ForeignKey("ResturantTypeId")]
        public ResturantType ResturantType { get; set; }




        [GenericStringLength(100), GenericRequired, Display(Name = "شرح خدمات")]
        public string Title { get; set; }



        [Display(Name = "نوع ")]
        public PaymentTypeEnum PaymentTypeEnumId { get; set; }

        [NotMapped]
        public string PaymentTypeEnumName => EnumExtention.GetDescription(PaymentTypeEnumId);
        [NotMapped]
        public string ResturantTypeName => PaymentTypeEnumId == PaymentTypeEnum.YearlyByDegree ? (ResturantType?.DropDownTitle+" "+Degree+" ستاره" ): Title;

    }
}