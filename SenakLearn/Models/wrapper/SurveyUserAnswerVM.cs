using System;
using System.ComponentModel;

namespace SenakLearn.Models.wrapper
{
    public class SurveyUserAnswerVM
    {

        public int Id { get; set; }
        public int AzmoonEntityId { get; set; }
        public string Ip { get; set; }
        public string act { get; set; }
        public string Question { get; set; }
        public bool IsRequired { get; set; }
        public bool Answered { get; set; }
        [Description("آزمون")]
        [Excel(IsColumnOut = true, Title = "")]
        public string SurveyEntity { get; set; }
        [Description("نام کاربر")]
        [Excel(IsColumnOut = true, Title = "")]
        public string User { get; set; }
        [Description("نام کاربري")]
        [Excel(IsColumnOut = true, Title = "")]
        public string UserName { get; set; }
        [Description("پاسخ داده نشده")]
        [Excel(IsColumnOut = true, Title = "")]
        public int NoAnswerd { get; set; }
        [Description("پاسخ صحيح")]
        [Excel(IsColumnOut = true, Title = "")]
        public int CorrectAnswerd { get; set; }
        [Description("پاسخ غلط")]
        [Excel(IsColumnOut = true, Title = "")]
        public int WrongAnswerd { get; set; }
        [Description("نمره")]
        [Excel(IsColumnOut = true, Title = "")]
        public double TotalScore { get; set; }
        [Description("رتبه")]
        [Excel(IsColumnOut = true, Title = "")]
        public short? TotalRank { get; set; }
        [Description("جمع کل نمرات")]
        [Excel(IsColumnOut = true, Title = "")]
        public double TotalCorrectScore { get; set; }
        [Description("حداکثر نمره")]
        [Excel(IsColumnOut = true, Title = "")]
        public double maxScore { get; set; }
        public double minScore { get; set; }
        public double zaribManfi { get; set; }
        public DateTime? AzmounDate { get; set; }
        public bool Accepted { get; internal set; }
        public DateTime? AcceptedDate { get; internal set; }
        public byte TimeDuration { get; internal set; }
        public string AcceptedDatePersian { get; internal set; }
        public string AzmounDatePersian { get; internal set; }
        public string act2 { get; internal set; }
        public string FromDateCourse { get; internal set; }
        public DateTime? FromDate { get; internal set; }
        public string ToDateCourse { get; internal set; }
        public DateTime? ToDate { get; internal set; }
    }
}