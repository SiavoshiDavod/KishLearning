using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class ZarinpalPaymentResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; }
        public int ZarinpalStatus { get; set; }
        public ZarinpalPayment Payment { get; set; }
        public string Authority { get; set; }
        public string BankUrl { get; set; }
        public DateTime CreateDate { get; set; }
    }
}