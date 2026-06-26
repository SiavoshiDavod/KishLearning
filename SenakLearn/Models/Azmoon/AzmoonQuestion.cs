using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Description("سوال آزمون")]
    public class AzmoonQuestion : BaseEntity
    {
        public AzmoonQuestion()
        {
            AzmoonQuestionOptions = new HashSet<AzmoonQuestionOption>();
            AzmoonAnswers = new HashSet<AzmoonAnswer>();
        }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام گروه سوال")]
        public int? AzmoonGroupQuestionId { get; set; }
        [ForeignKey("AzmoonGroupQuestionId")]
        [JsonIgnore]
        public AzmoonGroupQuestion AzmoonGroupQuestion { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "نام پرسشنامه")]
        public int AzmoonEntityId { get; set; }
        [ForeignKey("AzmoonEntityId")]
        [JsonIgnore]
        public AzmoonEntity AzmoonEntity { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "ترتیب سوال")]
        public int AzmoonOrder { get; set; }
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
        [GenericStringLength(100)]
        [System.ComponentModel.DataAnnotations.Display(Name = "عکس جواب")]
        public string AnswerImageUrl { get; set; }

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
        public ICollection<AzmoonQuestionOption> AzmoonQuestionOptions { get; set; }
        [JsonIgnore]
        public ICollection<AzmoonAnswer> AzmoonAnswers { get; set; }
        [NotMapped]
        public string QuestionTypeName
        {
            get
            {
                return SenakLearn.EnumExtention.GetDescription<QuestionEnum>(this.QuestionType);
                // return this.QuestionType.ToString();
            }
            set { }
        }

        [System.ComponentModel.DataAnnotations.Display(Name = "نمره")]
        public double Score { get; set; }
    }

    public enum AzmoonEnum
    {
        //no answer
        [Description("صفحه خوش آمدگویی")]
        Welcome,

        //one Answer that is AzmoonQuestionOptionId
        //multi question
        [Description("چند گزینه ای")]
        Option,

        [Description("چند گزینه ای تصویری")]
        ImageOption,

        //Multi Answer  that is AzmoonQuestionOptionId
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

        //one Answer that is AzmoonQuestionOptionId
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