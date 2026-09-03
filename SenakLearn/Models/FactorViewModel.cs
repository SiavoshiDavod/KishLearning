using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
	public class FactorViewModel
	{
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        [Excel(IsColumnOut = true, Title = "خرید")]
        public string ServiceName { get; set; }
        public string IdForSale { get; set; }
        [Excel(IsColumnOut = true, Title = "عنوان")]
        public string TitleForSale { get; set; }
        public int? UserId { get; set; }
        [Excel(IsColumnOut = true, Title = "خریدار")]
        public string UserName { get; set; }
        [Excel(IsColumnOut = true, Title = "همراه")]
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; } = 1;
        [Excel(IsColumnOut = true, Title = "وضعیت")]
        public string StatusName
        {
            get => StatusId == FactorStatusEnum.Factor_Status_Create ? "پرداخت نشده" : StatusId == FactorStatusEnum.Factor_Status_Sended ? "ارسال به درگاه" : StatusId == FactorStatusEnum.Factor_Status_Success
                ? "پرداخت موفق" : StatusId == FactorStatusEnum.Factor_Status_NoPayment ? "پرداخت ناموفق" : StatusId == FactorStatusEnum.Factor_Status_Removed ? "ابطال شده" : "";
            set => value = null;
        }
        [Excel(IsColumnOut = true, Title = "مبلغ")]
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        [Excel(IsColumnOut = true, Title = "تراکنش")]
        public string PaymentTransaction { get; set; }
        [Excel(IsColumnOut = true, Title = "رهگیری")]
        public string PaymentTrace { get; set; }
        public DateTime? PaymentDatetime { get; set; }
        [Excel(IsColumnOut = true, Title = "تاریخ تراکنش")]
        public string PaymentDatetimePer { get; set; }
        [Excel(IsColumnOut = true, Title = "شرح")]
        public string Descript { get; set; }
        public string FactorNo { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        [Excel(IsColumnOut = true, Title = "تاریخ")]
        public virtual string CreatedDateShamsi
        {
            get => CreatedDate.ToPersianDateTime();
            set => CreatedDate = value.ToGregorianDate();
        }
        public string act { get; set; }
    }
}