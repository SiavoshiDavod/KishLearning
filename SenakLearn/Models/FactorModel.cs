using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    [Table("Factor", Schema = "dbo")]
    public class FactorModel : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Display(Name = "تاریخ ایجاد"), GenericRequired]
        public DateTime CreateDate { get; set; }
        [Display(Name = "خدمات"), GenericRequired]
        public string ServiceName { get; set; }
        [Display(Name = "شناسه خدمات"), GenericRequired]
        public string IdForSale { get; set; }
        [Display(Name = "کاربر")]
        public int? UserId { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [Display(Name = "همراه")]
        public string Mobile { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "وضعیت")]
        public int StatusId { get; set; } = 1;
        [NotMapped]
        public string StatusName
        {
            get => StatusId == 1 ? "پرداخت نشده" : StatusId == 2 ? "ارسال به درگاه" : StatusId == 3 ? "پرداخت موفق" : StatusId == 4 ? "پرداخت ناموفق" : StatusId == 5 ? "ابطال شده" : "";
            set => value = null;
        }
        [Display(Name = "مبلغ")]
        public decimal Amount { get; set; }
        [Display(Name = "تخفیف")]
        public decimal? Discount { get; set; }
        [Display(Name = "کد تراکنش ")]
        public string PaymentTransaction { get; set; }
        [Display(Name = "کد رهگیری")]
        public string PaymentTrace { get; set; }
        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PaymentDatetime { get; set; }
        [Display(Name = "توضیحات")]
        public string Descript { get; set; }
        [Display(Name = "شماره فاکتور")]
        public string FactorNo { get; set; }
        [NotMapped]
        public DateTime? UpdateDate { get; set; }
        [NotMapped]
        public override DateTime CreatedDate { get { return CreateDate; }  }
        [Display(Name = "مبلغ اصلی")]
        public decimal? AmountMaster { get; set; }
        //[NotMapped]
        //public virtual string CreatedDateShamsi
        //{
        //    get => CreatedDate.ToPersianDateTime();
        //    set => CreatedDate = value.ToGregorianDate();
        //}
        //[NotMapped]
        //public string act { get; set; }
    }
    public static class FactorStatusEnum
    {
        /// <summary>
        /// پرداخت نشده
        /// </summary>
        public static readonly int Factor_Status_Create = 1;
        /// <summary>
        /// ارسال به درگاه
        /// </summary>
        public static readonly int Factor_Status_Sended = 2;
        /// <summary>
        /// دریافت شناسه
        /// </summary>
        public static readonly int Factor_Status_Authorithy = 3;
        /// <summary>
        /// پرداخت موفق
        /// </summary>
        public static readonly int Factor_Status_Success = 6;
        /// <summary>
        /// پرداخت ناموفق
        /// </summary>
        public static readonly int Factor_Status_NoPayment = 4;
        /// <summary>
        /// ابطال شده
        /// </summary>
        public static readonly int Factor_Status_Removed = 5;
    }
}