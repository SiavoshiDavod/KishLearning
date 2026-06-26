using System;

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
        public string SurveyEntity { get; set; }
        public string User { get; set; }
        public string UserName { get; set; }
        public int NoAnswerd { get; set; }
        public int CorrectAnswerd { get; set; }
        public int WrongAnswerd { get; set; }
        public double TotalScore { get; set; }
        public short? TotalRank { get; set; }

        public double TotalCorrectScore { get; set; }
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