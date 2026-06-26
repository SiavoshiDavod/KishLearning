using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseModel
{
    [Description("نظرسنجی")]
    public class SurveyEntity:BaseEntity
    {
        public SurveyEntity()
        {
           // SurveyAnswers = new HashSet<SurveyAnswer>();
            SurveyQuestions = new HashSet<SurveyQuestion>();
            SurveyGroupQuestion = new HashSet<SurveyGroupQuestion>();
        }

        [Display(Name = "نام گروه")]
        public int GroupSurveyId { get; set; }
        [ForeignKey("GroupSurveyId")]
        [System.ComponentModel.DataAnnotations.Display(Name = "نام گروه")]
        [JsonIgnore]
        public GroupSurvey GroupSurvey { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        [GenericRequired, GenericStringLength(500)]
        public String Name { get; set; }

        [Display(Name = "عکس پرسشنامه")]
        [ GenericStringLength(100)]
        public string SurveyImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "تعداد پاسخنامه")]
        public int AnswerCount { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "تعداد سوالات")]
        public int QuestionCount { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "وضعیت پرسشنامه")]
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
        [Display(Name = "موضوع پرسشنامه")]
        public string Title { get; set; }

        [GenericStringLength(2000)]
        [Display(Name = "توضیحات پرسشنامه")]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = " اختصاصی?")]
        public bool IsPrivate { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "گروه اختصاصی")]
        public int? SurveyPrivateGroupId { get; set; }

        [ForeignKey("SurveyPrivateGroupId")]
        public SurveyPrivateGroup SurveyPrivateGroup { get; set; }

        //public ICollection<SurveyAnswer> SurveyAnswers { get; set; }
        [JsonIgnore]
        public ICollection<SurveyQuestion> SurveyQuestions { get; set; }
        [JsonIgnore]
        public ICollection<SurveyGroupQuestion> SurveyGroupQuestion { get; set; }
        

        [NotMapped]
        public string StatusName => Status ? "فعال" : "غیرفعال";

    }

}