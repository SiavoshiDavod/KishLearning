using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class UserCommnet : ParentChildEntity
    {
        public int? UserId { get; set; }

        public bool Status { get; set; }
        public int? TypeId { get; set; }
        public PageType PageTypeId { get; set; }

        [ Display(Name = "عنوان")]
        public string Title { get; set; }
        [GenericRequired, Display(Name = "نام")]
        public string Name { get; set; }
        [GenericRequired, Display(Name = "ایمیل"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Mobile { get; set; }

        [ForeignKey("UserId")]
        public learn_user learn_user { get; set; }
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
                    case PageType.OnlineClass:
                        return "<a href=\"/DetailsOnlineClass/Index?type=2&id=" + TypeId + "\">نمایش</a>";
                    case PageType.OfflineClass:
                        return "<a href=\"/DetailsCours/Index?type=1&id=" + TypeId + "\">نمایش</a>";
                    case PageType.Paper:
                        return "<a href=\"/paper/detail?id=" + TypeId + "\">نمایش</a>";
                    case PageType.Step6:
                        return "<a href=\"/Home/Step?id=" + TypeId + "\">نمایش</a>";
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
        OnlineClass,
        OfflineClass,
        Step6,
        Paper,
        News
    }
}