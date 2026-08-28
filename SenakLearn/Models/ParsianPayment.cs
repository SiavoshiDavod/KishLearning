using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class ParsianPayment
    {
        public int Id { get; set; }
        public string RefId { get; set; }
        public string Token { get; set; }
        public int Amount { get; set; }
        public short? Status { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public string Descript { get; set; }

    }
    public enum ParsianPaymentEnum
    {
        
    }
}