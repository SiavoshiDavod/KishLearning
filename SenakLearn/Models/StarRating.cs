using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("ثبت امتیاز")]
    public class StarRating:BaseEntity
    {
        public int? UserId { get; set; }
        [GenericStringLength(15),GenericRequired]
        public string Ip { get; set; }
        public int TypeId { get; set; }

        public PageType PageTypeId { get; set; }
        public byte Rate { get; set; }

        //[ForeignKey("UserId")]
        //public learn_user learn_user { get; set; }

        [NotMapped]
        public string googlerecaptcha { get; set; }
        [NotMapped]
        public string PageTypeName => PageTypeId.ToString();
        [NotMapped]
        public override DateTime CreatedDate { get => base.CreatedDate; set => base.CreatedDate = value; }
        [NotMapped]
        public override DateTime? UpdateDate { get => base.UpdateDate; set => base.UpdateDate = value; }
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
}