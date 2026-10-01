using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Parsian
{
    public class ParsianPaymentResponse
    {
        public string BankUrl { get; set; }
        public string Token { get; set; }
        public short Status { get; set; }
        public string Message { get; set; }
        public string ResponseContent { get; set; }

        /// <summary>
        /// جهت تطابق کامل با کدهای جاوااسکریپت موجود در ویو (Index.cshtml)
        /// اگر وضعیت پارسیان 0 (موفق) باشد، مقدار 100 برمی‌گرداند.
        /// </summary>
        public int ZarinpalStatus
        {
            get => Status == 0 ? 100 : (int)Status;
            set { /* Readonly proxy */ }
        }

        public bool IsSuccess => Status == 0 && !string.IsNullOrEmpty(Token);
    }
}