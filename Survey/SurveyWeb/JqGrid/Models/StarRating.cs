using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{
    [Description("ثبت امتیاز")]
    public class StarRating : BaseEntity
    {
        public int? UserId { get; set; }
        [GenericStringLength(15), GenericRequired]
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
        public override DateTime CreatedDate { get; set; }
        [NotMapped]
        public override DateTime? UpdateDate { get; set; }
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
                        return "<a href=\"/Home/news/" + TypeId + "\">نمایش</a>";
                    case PageType.Advertising:
                        return "<a href=\"/Home/Advertising/" + TypeId + "\">نمایش</a>";
                    default:
                        break;
                }
                return "";
            }
        }
    }
}