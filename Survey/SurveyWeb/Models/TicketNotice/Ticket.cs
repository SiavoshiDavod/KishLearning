using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models.TicketNotice
{
    public class Ticket : BaseEntity
    {
        public int? ParentId { get; set; } 

        [Display(Name = "عنوان")]
        [GenericStringLength(200), GenericRequired]
        public string Title { get; set; }

        [GenericStringLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "پیام")]
        public string Content { get; set; }

        [Display(Name = "ثبت کننده")]
        public int SenderUserId { get; set; }

        [Display(Name = "پاسخ دهنده")]
        public int? ReceiverUserId { get; set; }

        public bool IsRead { get; set; } = false;

        [DataType(DataType.MultilineText)]
        [Display(Name = "پاسخ")]
        [GenericStringLength(1000)]
        public string Answer { get; set; }

        [GenericStringLength(200)]
        [Display(Name = "پیوست")]
        public string File { get; set; }

        [ForeignKey(nameof(SenderUserId))]
        public User SenderUser { get; set; }
        [ForeignKey(nameof(ReceiverUserId))]
        public User ReceiverUser { get; set; }
    }
}