using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SenakLearn.Models
{
    [Table("EmailSms", Schema = "dbo")]
    public class EmailSms : BaseEntity
    {
        [GenericRequired]
        public string To { get; set; }
        [GenericRequired]
        public string From { get; set; }
        [GenericRequired]
        [AllowHtml]
        public string Body { get; set; }
        [GenericRequired]
        public string Subject { get; set; }//RecId in Sms
        public bool IsSend { get; set; }
        public string SendResult { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        [NotMapped]
        public string EmailSmsTypeName { get { return EmailSmsType.ToString(); } set { } }
        [NotMapped]
        public int GroupId { get; set; }
    }
    public enum EmailSmsType
{
        Email,
        Sms
    }
}