using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SurveyWeb.Models.TicketNotice
{
    public class EmailSms : BaseEntity
    {
        [GenericStringLength(100), GenericRequired]
        public string To { get; set; }
        [GenericStringLength(100), GenericRequired]
        public string From { get; set; }
        [AllowHtml]
        [GenericStringLength(1000), GenericRequired]
        public string Body { get; set; }
        [GenericStringLength(100), GenericRequired]
        public string Subject { get; set; }//RecId in Sms
        public bool IsSend { get; set; }
        [GenericStringLength(200), GenericRequired]
        public string SendResult { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        [NotMapped]
        public string EmailSmsTypeName { get { return EmailSmsType.ToString(); } set { } }
        [NotMapped]
        public Roles RolesId { get; set; }
    }
}