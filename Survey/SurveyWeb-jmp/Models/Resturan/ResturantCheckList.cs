using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{

    [Description("آپلود اسناد مورد نیاز")]
    public class ResturantCheckList : BaseEntity
    {
        public int ResturantId { get; set; }
        [ForeignKey("ResturantId")]
        public Resturant Resturant { get; set; }

        [GenericStringLength(100)]
        [DisplayName(" بارگزاری فایل")]
        public string ImageUrl { get; set; }

        [DisplayName("نوع مدرک")]
        public int CheckListId { get; set; }
        [ForeignKey("CheckListId")]
        [DisplayName("نوع مدرک")]
        public CheckListType CheckListType { get; set; }

        [DisplayName("نام مدرک")]
        public string Name { get; set; }

        [DisplayName("تاریخ صدور")]
        public DateTime? IssueDate { get; set; }

        [DisplayName("تاریخ انقضا")]
        public DateTime? ExpireDate { get; set; }

        [NotMapped]
        public string ExpireDateShamsi
        {
            get { return ExpireDate?.ToPersianDate(); }
            set { ExpireDate = value.ToGregorianDate(); }
        }


        [NotMapped]
        public string IssueDateShamsi
        {
            get { return IssueDate?.ToPersianDate(); }
            set { IssueDate = value.ToGregorianDate(); }
        }

        [NotMapped]
        public string CheckListName => CheckListType?.DropDownTitle;

    }
}