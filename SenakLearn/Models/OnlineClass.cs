using SenakLearn.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SenakLearn.Models
{
    public class OnlineClass : BaseEntity
    {
        [Required, Display(Name = "عنوان کلاس")]
        public string name { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "گروه دوره کلاس")]
        public int id_learn_cours_group { get; set; }
        [Display(Name = "کاربر ثبت کننده")]
        public int? id_user_register { get; set; }
        //[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        //[Display(Name = "تاریخ ثبت")]
        //public string createDate { get; set; }
        [ForeignKey("id_learn_cours_group")]
        public virtual learn_cours_group learn_cours_group { get; set; }
        [ForeignKey("id_learn_teacher")]
        public virtual learn_teacher learn_teacher { get; set; }
        [Display(Name = "استاد")]
        public int id_learn_teacher { get; set; }
        [Display(Name = "هزینه دوره(ریال)")]
        public int Amount { get; set; }
        [Display(Name = "مدت دوره")]
        public int Duration { get; set; }
        [Display(Name = "تعداد جلسات")]
        public int SessionCount { get; set; }
        [Display(Name = "جزئیات دوره/توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "چه چیزی داخل این دوره می آموزید")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string doc2 { get; set; }
        [Display(Name = "ساعت برگزاری کلاس")]
        public byte Time { get; set; }
        [Display(Name = "تاریخ شروع برگزاری کلاس")]
        public override DateTime CreatedDate { get; set; }
        [Display(Name = "تاریخ پایان برگزاری کلاس")]
        public override DateTime? UpdateDate { get; set; }
        [Display(Name = "ظرفیت")]
        public int Capacity { get; set; }
        [Display(Name = "محبوب است(نمایش در صفحه اصلی سایت)")]
        public bool IsFavorite { get; set; }
        [Display(Name = "ویدیو معرفی دوره")]
        public Guid? InterviewPathVideo { get; set; }
        [Display(Name = "عکس معرفی دوره")]
        public string InterviewPathImage { get; set; }
        [Display(Name = "اطلاعات مرتبط با این کلاس")]
        public int? OnlineClassAccorationId { get; set; }
        [ForeignKey("OnlineClassAccorationId")]
        public OnlineClassAccoration OnlineClassAccoration { get; set; }
        //[ForeignKey("InterviewPathVideo")]
        //public learn_file InterviewPathVideoFile { get; set; }
        [Display(Name = "روزهای هفته")]
        public string Days { get; set; }
        [Display(Name = "روزهای هفته")]
        public string[] DaysArr
        {
            get => Days.ToArrayForMultiDropDown();
            set => Days = string.Join(",", value);
        }
        [Display(Name = " ورود به ادوبی")]
        public string AdobeMeeting { get; set; }
        public long? AdobeScoId { get; set; }
        public string GoToAdobe => string.IsNullOrEmpty(AdobeMeeting) || AdobeScoId == null ? "" : "<a href=\"/AdobeConnectTest/GoToAdobe?url=" + AdobeMeeting + "&scoId=" + AdobeScoId + "&classId=" + Id + "\">ورود</a>";
        [Display(Name = "وضعیت")]
        //[Column(TypeName = "tinyint")]
        //[DefaultValue((byte)0)]
        public virtual OnlineClassType ClassType { get; set; }

        [Display(Name = "وضعیت اتوماتیک باشد؟")]
        public bool IsAutoClassType { get; set; }

        [NotMapped]
        public string ClassTypeString => EnumExtention.GetDescription(ClassType);
        public string IsFavoriteS { get { return IsFavorite ? "محبوب" : ""; } }
        public string AmountFarsi => Amount.ToString("N0").ToPersianNumber();
        [Display(Name = "دوره مرتبط")]
        public int? CourseRelated { get; set; }
        [Display(Name = "کتاب مرتبط")]
        public int? BookRelated { get; set; }
        [Display(Name = "جزوه مرتبط")]
        public int? BookletRelated { get; set; }

        [NotMapped]
        public bool Clone { get; set; }
    }
}