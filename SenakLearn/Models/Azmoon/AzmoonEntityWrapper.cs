using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Azmoon
{
    public class AzmoonEntityWrapper
    {
        public int AzmoonEntityId { get; set; }
        public int AzmoonUserAnswerId { get; set; }
        public string AzmoonEntityName { get; set; }
        public string GroupAzmoonName { get; set; }
        public int? TimeDuration { get; set; }
        public double TotalScore { get; internal set; }
        public int UserId { get; internal set; }
        public string UserName { get; internal set; }
        public string NameFamily { get; internal set; }
        public short? TotalRank { get; internal set; }
        public DateTime AzmounDate { get; internal set; }
        public string AzmounDatePersian { get; internal set; }
        public bool? Accepted { get; internal set; }
        public DateTime? AcceptedDate { get; internal set; }
        public string AcceptedDatePersian { get; internal set; }
        public int? act { get; set; }
        public int? act2 { get; set; }
    }
}