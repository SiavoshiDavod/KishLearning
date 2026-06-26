using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("سوال نظرسنجی")]
    public class SurveyQuestion : BaseEntity
    {
        public SurveyQuestion()
        {
            SurveyQuestionOptions = new HashSet<SurveyQuestionOption>();
            SurveyAnswers = new HashSet<SurveyAnswer>();
        }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام گروه سوال")]
        public int? SurveyGroupQuestionId { get; set; }
        [ForeignKey("SurveyGroupQuestionId")]
        [JsonIgnore]
        public SurveyGroupQuestion SurveyGroupQuestion { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        public int SurveyEntityId { get; set; }
        [ForeignKey("SurveyEntityId")]
        [JsonIgnore]
        public SurveyEntity SurveyEntity { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "ترتیب سوال")]
        public int SurveyOrder { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "عنوان سوال")]
        [GenericRequired, GenericStringLength(1000)]
        public string Question { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نوع سوال")]
        public QuestionEnum QuestionType { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "اجباری بودن سوال")]
        public bool required { get; set; }

        [GenericStringLength(100)]
        [System.ComponentModel.DataAnnotations.Display(Name = "عکس سوال")]
        public string QuestionImageUrl { get; set; }

        [Display(Name = "عرض عکس ")]
        public Int16 Width { get; set; }
        [Display(Name = "ارتفاع عکس ")]
        public Int16 Height { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "حداقل مقدار سوال")]
        public int MinType { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "حداکثر مقدار سوال")]
        public int MaxType { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نوع مقدار سوال")]
        public string StringType { get; set; }
        [JsonIgnore]
        public ICollection<SurveyQuestionOption> SurveyQuestionOptions { get; set; }
        [JsonIgnore]
        public ICollection<SurveyAnswer> SurveyAnswers { get; set; }
        [NotMapped]
        public string QuestionTypeName
        {
            get
            {
                return SurveyWeb.EnumExtention.GetDescription<QuestionEnum>(this.QuestionType);
                // return this.QuestionType.ToString();
            }
            set { }
        }
    }

    public enum QuestionEnum
    {
        //no answer
        [Description("صفحه خوش آمدگویی")]
        Welcome,

        //one Answer that is SurveyQuestionOptionId
        //multi question
        [Description("چند گزینه ای")]
        Option,

        [Description("چند گزینه ای تصویری")]
        ImageOption,

        //Multi Answer  that is SurveyQuestionOptionId
        [Description("چند گزینه ای،چند انتخابی")]
        MultiOptions,

        [Description("چند گزینه ای تصویری،چند انتخابی")]
        ImageMultiOptions,

        //one Answer
        //input type
        [Description("متنی با پاسخ کوتاه")]
        text,//maxlength

        [Description("متنی با پاسخ بلند")]
        textArea,//maxlength

        //one Answer that is SurveyQuestionOptionId
        //multi question
        [Description("لیست کشویی")]
        DropDownOption,

        //one Answer
        //input type
        [Description("عددی")]
        number,//min,max

        [Description("رنگ")]
        color,

        [Description("ایمیل")]
        email,

        [Description("درجه بندی")]
        range,//min,max //rate:heart,like,star //step:interval

        [Description("لینک/وب سایت")]
        url,

        //no answer
        [Description("متنی بدون پاسخ")]
        TextOnly,

        [Description("آپلود فایل")]
        file,//volume //type:image,pdf,video

        //no answer
        [Description("صفحه تشکر")]
        Goodbye,




    }
}