using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace SurveyWeb.Models.CheckList
{
    [Description("چک لیست بازرسی")]
    public class ComplaintCheckList : BaseEntity
    {
        [Display(Name = "چک لیست")]
        [GenericRequired]
        public int CheckListId { get; set; }
        [ForeignKey("CheckListId")]
        public CheckList CheckList { get; set; }
        [Display(Name = "مرکز")]
        [GenericRequired]
        public int ResturantId { get; set; }
        [ForeignKey("ResturantId")]
        public Resturant Resturant { get; set; }
        [Display(Name = "تاریخ بازدید")]
        [GenericRequired]
        public string ComplaintDatePersian { get; set; }
        [Display(Name = "ساعت بازدید")]
        [GenericRequired]
        public string ComplaintTimePersian { get; set; }
        [GenericRequired]
        public DateTime? ComplaintDate { get; set; }
        [Display(Name = "کارشناس نظارت")]
        [GenericRequired]
        public int? UserComplaintId { get; set; }
        [ForeignKey("UserComplaintId")]
        public User UserComplaint { get; set; }
        [Display(Name = "توضیحات کلی")]
        public string Descript { get; set; }
        [Display(Name = "روز حل مشکلات")]
        public int? DayNumResolve { get; set; }
    }
}