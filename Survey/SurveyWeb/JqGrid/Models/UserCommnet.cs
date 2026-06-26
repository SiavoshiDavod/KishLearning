using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    public class UserComment : ParentChildEntity
    {
        public int? UserId { get; set; }

        public bool Status { get; set; }
        public int? TypeId { get; set; }
        public PageType PageTypeId { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [GenericRequired, Display(Name = "نام")]
        public string Name { get; set; }
        [GenericRequired, Display(Name = "ایمیل"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Mobile { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
        [NotMapped]
        public override int Order { get; set; }
        [NotMapped]
        public string googlerecaptcha { get; set; }
        [NotMapped]
        public string PageTypeName => PageTypeId.ToString();
        [NotMapped]
        public string PageTypeLinkId
        {
            get
            {
                switch (PageTypeId)
                {
                    case PageType.None:
                        break;
                    case PageType.News:
                        return "<a href=\"/Home/News?id=" + TypeId + "\">نمایش</a>";
                    case PageType.Advertising:
                        return "<a href=\"/Home/Advertising?id=" + TypeId + "\">نمایش</a>";
                    default:
                        break;
                }
                return "";
            }
        }
    }
    public enum PageType
    {
        None,
        News,
        Advertising
    }
}