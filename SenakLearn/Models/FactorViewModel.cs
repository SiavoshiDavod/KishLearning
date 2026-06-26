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
        public string ServiceName { get; set; }
        public string IdForSale { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; } = 1;
        public string StatusName
        {
            get => StatusId == 1 ? "پرداخت نشده" : StatusId == 2 ? "ارسال به درگاه" : StatusId == 3 ? "پرداخت موفق" : StatusId == 4 ? "پرداخت ناموفق" : StatusId == 5 ? "ابطال شده" : "";
            set => value = null;
        }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public string PaymentTransaction { get; set; }
        public string PaymentTrace { get; set; }
        public DateTime? PaymentDatetime { get; set; }
        public string Descript { get; set; }
        public string FactorNo { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedDateShamsi
        {
            get => CreatedDate.ToPersianDateTime();
            set => CreatedDate = value.ToGregorianDate();
        }
        public string act { get; set; }
    }
}