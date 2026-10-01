using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Table("FactorePaymentRequest", Schema = "dbo")]
    public class FactorePaymentRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long FactorId { get; set; }

        public int? UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 1: Initiated, 2: TokenGenerated, 6: Succeeded, 4: Failed, -1: SystemError
        /// </summary>
        public int StatusId { get; set; }

        [MaxLength(100)]
        public string Token { get; set; }

        public short? TerminalStatus { get; set; }

        [MaxLength(50)]
        public string RRN { get; set; }

        [MaxLength(50)]
        public string TraceNo { get; set; }

        [MaxLength(50)]
        public string CardNumberMasked { get; set; }

        public DateTime? CallbackDateTime { get; set; }

        public DateTime? VerifyDateTime { get; set; }

        public short? VerifyStatus { get; set; }

        [MaxLength(500)]
        public string VerifyMessage { get; set; }

        public string RawCallbackData { get; set; }

        public string RawRequestData { get; set; }

        [MaxLength(50)]
        public string IpAddress { get; set; }

        public string ResponseContentRequest { get; set; }

        [ForeignKey("FactorId")]
        public virtual FactorModel Factor { get; set; }
    }

    public static class PaymentRequestStatusEnum
    {
        public const int Initiated = 1;
        public const int TokenReceived = 2;
        public const int Failed = 4;
        public const int Succeeded = 6;
        public const int ErrorOnRequest = -1;
    }
}
