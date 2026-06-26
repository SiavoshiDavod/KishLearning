using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("کاربران پاسخ دهنده به آزمون")]
    public class AzmoonUserAnswer : BaseEntity
    {
        public AzmoonUserAnswer()
        {
            AzmoonAnswers = new HashSet<AzmoonAnswer>();
        }

        public int? UserId { get; set; }
        public learn_user User { get; set; }

        [Display(Name = "آی پی")]
        [ GenericStringLength(20)]
        public string Ip { get; set; }
        [JsonIgnore]
        public virtual ICollection<AzmoonAnswer> AzmoonAnswers { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        public int AzmoonEntityId { get; set; }

        public int NoAnswerd { get; set; }
        public int CorrectAnswerd { get; set; }
        public int WrongAnswerd { get; set; }
        public double TotalScore { get; set; }
        [ForeignKey("AzmoonEntityId")]
        public AzmoonEntity AzmoonEntity { get; set; }

        public short? TotalRank { get; set; }

        [NotMapped]
        public int TotalCount { get; set; }
        [Display(Name = "تاریخ صدور مدرک")]
        public DateTime? AcceptedDate { get; set; }
        [Display(Name = "صادر کننده مدرک")]
        public int? AcceptedBy { get; set; }
    }
}