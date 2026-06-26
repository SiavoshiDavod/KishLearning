using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("آزمون")]
    public class AzmoonEntity:BaseEntity
    {
        public AzmoonEntity()
        {
           // AzmoonAnswers = new HashSet<AzmoonAnswer>();
            AzmoonQuestions = new HashSet<AzmoonQuestion>();
            //AzmoonGroupQuestion = new HashSet<AzmoonGroupQuestion>();
        }
		
		 [System.ComponentModel.DataAnnotations.Display(Name = "نمایش سوالات در یک صفحه")]
		public bool IsShowInSinglePage { get; set; }


        [Display(Name = "نام گروه")]
        public int GroupAzmoonId { get; set; }
        [ForeignKey("GroupAzmoonId")]
        [System.ComponentModel.DataAnnotations.Display(Name = "نام گروه")]
        [JsonIgnore]
        public GroupAzmoon GroupAzmoon { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام آزمون")]
        [GenericRequired, GenericStringLength(500)]
        public String Name { get; set; }

        [Display(Name = "عکس آزمون")]
        [ GenericStringLength(100)]
        public string AzmoonImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "تعداد پاسخنامه")]
        public int AnswerCount { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "تعداد سوالات")]
        public int QuestionCount { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "وضعیت آزمون")]
        public bool Status { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "محدویت آی پی")]
        public bool IsIpRestriction { get; set; }

        [Display(Name = "آیا کاربر باید در سیستم لاگین کرده باشد")]
        public bool IsUserMustBeLogin { get; set; }

        [Display(Name = "محبوب")]
        public bool IsFavorite { get; set; }

        [Display(Name = "مهم")]
        public bool IsImportant { get; set; }

        [GenericStringLength(200)]
        [Display(Name = "موضوع آزمون")]
        public string Title { get; set; }
        [NotMapped]
        [Display(Name = "تاریخ شروع")]
        public string FromDate_l { get; set; }
        public DateTime? FromDate { get; set; }

        [NotMapped]
        [Display(Name = "تاریخ پایان")]
        public string ToDate_l { get; set; }
        public DateTime? ToDate { get; set; }

        [GenericStringLength(2000)]
        [Display(Name = "توضیحات آزمون")]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = " اختصاصی?")]
        public bool IsPrivate { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "گروه اختصاصی")]
        public int? AzmoonPrivateGroupId { get; set; }

        [ForeignKey("AzmoonPrivateGroupId")]
        public Security.AzmoonPrivateGroup AzmoonPrivateGroup { get; set; }

        //public ICollection<AzmoonAnswer> AzmoonAnswers { get; set; }
        [JsonIgnore]
        public ICollection<AzmoonQuestion> AzmoonQuestions { get; set; }
        //[JsonIgnore]
        //public ICollection<AzmoonGroupQuestion> AzmoonGroupQuestion { get; set; }
        

        [NotMapped]
        public string StatusName => Status ? "فعال" : "غیرفعال";

        [Display(Name = "حداقل نمره قبولی")]
        public int MinScore { get; set; }
        [Display(Name = "حداکثر نمره")]

        public int MaxScore { get; set; }
        [Display(Name = "جمع کل نمرات")]

        public int TotalScore { get; set; }

        [Display(Name = "ضريب نمره منفي")]
        public double ZaribManfi { get; set; }

        [Display(Name = "فقط سوالات تستی جهت محاسبه نمره")]
        public bool IsJustOption { get; set; }

        [Display(Name = "داشتن رتبه بندی")]
        public bool IsRanking { get; set; }

        [Display(Name = "نوع نمونه سوالات")]
        public AzmoonEntityType AzmoonEntityType { get; set; }

        [Display(Name = "مدت آزمون(دقیقه)")]

        public byte TimeDuration { get; set; }
        [NotMapped]
        public int UserIdCurrent { get; set; }


        [Display(Name = "تصویر مدرک")]
        [GenericStringLength(100)]
        public string AzmoonCerImageUrl { get; set; }
    }
    public enum AzmoonEntityType:byte
    {
        Amozeshi,
        Konkor,
        Estekhdami
    }
}