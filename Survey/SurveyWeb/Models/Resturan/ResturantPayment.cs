using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models.Resturan
{
    public class ResturantPayment:BaseEntity
    {
        [Display(Name = "نوع و مبلغ پرداخت")]
        public int PaymentTypeId { get; set; }
        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "نوع ")]
        public PaymentTypeEnum PaymentTypeEnumId { get; set; }

        [NotMapped]
        public string PaymentTypeEnumName => EnumExtention.GetDescription(PaymentTypeEnumId);

        [Display(Name = "مبلغ")]
        public int Price { get; set; }

        [Display(Name = "پرداخت آنلاين ")]
        public bool IsOnlinePayment { get; set; }

        [Display(Name = " کاربر ")]
        public int UserId { get; set; }


        [Display(Name = " مرکزپذیرایی ")]
        public int ResturantId { get; set; }
        [ForeignKey("ResturantId")]
        public Resturant Resturant { get; set; }

        [Display(Name = " تصوير فيش پرداختي ")]
        [GenericStringLength(100)]
        public string FishPic { get; set; }

        [Display(Name = " تاريخ فیش پرداخت ")]
        public DateTime PaymentDate { get; set; }

        [NotMapped][GenericRequired]
        public string PaymentDateShamsi
        {
            get { return PaymentDate.ToPersianDate(); }
            set { PaymentDate = value.ToGregorianDate().Value; }
        }

        [Display(Name = " واريز کننده ")]
        [GenericStringLength(100)]
        public string VarizKonande { get; set; }
        [Display(Name = "شماره فیش/کد رهگیری")]
        public long RefId { get; set; }
        [Display(Name = " تاييد شده ")]
        public bool IsAccepted { get; set; }

        [DisplayName(" توضیحات تایید پرداخت")]
        [GenericStringLength(500)]
        public string AdminDescription { get; set; }

    }
}