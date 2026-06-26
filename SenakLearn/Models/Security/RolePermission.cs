using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models.Security
{
    [Description("دسترسی های نقش")]
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Permisstion Permisstion { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [NotMapped]
        public string act { get; set; }
        [NotMapped]
        public string PermisstionName
        {
            get
            {
                return SenakLearn.EnumExtention.GetDescription<Permisstion>(this.Permisstion);
                // return this.QuestionType.ToString();
            }
            set { }
        }
    }
    public enum Permisstion
    {
        [Description("دسترسی های نقش")]
        RolePermissions,
        [Description("کاربران")]
        UsersAdmin,///Admin/UsersAdmin
        [Description("نقش")]
        Roles,

        [Description("نقش های کاربر")]
        RoleUsers,

        [Description("منو")]
        Menus,
        [Description("زیرمنو")]
        DynamicForms,

        [Description("اخبار")]
        News,
        [Description("گروه خبر")]
        NewsGroups,
        [Description("نویسنده")]
        Authors,

        [Description("لوگوی سازمانهای مرتبط")]
        CompanyLogoAndLinks,

        [Description("ثبت اساتید")]
        Teacher,///Admin/Teacher/Index
        [Description("همکاری با ما")]
        JoinUs,
        [Description("کتاب")]
        Book,
        [Description("مقاله")]
        Papers,
        [Description("کیفیت")]
        PaperTranslateQuality,
        [Description("دانشگاه")]
        PaperUniversity,
        [Description("ژورنال")]
        PaperJournal,
        [Description("ناشر")]
        PaperPublisher,
        [Description("گرایش")]
        PaperTrend,
        [Description("رشته")]
        PaperField,

        [Description("آپلود ویدیو")]
        VideoFile,
        [Description("پشتیبانی دانشجویان")]
        StudentSupport,
        [Description("پشتیبانی اساتید")]
        TeacherSupport,
        [Description("کلاس آفلاین")]
        OfflineVideo,

        [Description("تنظیمات سایت")]
        SiteSetting,

        [Description("گروه بندی دوره ها")]
        Group,///Admin/Group/Index
        [Description("فیلم های آموزشی")]
        Cours,///Admin/Cours/Index
        [Description("پادکست")]
        Podcast,
        [Description("کلاس های آنلاین")]
        OnlineClasses,
        [Description("توضیحات کلاسها")]
        OnlineClassAccorations,
        [Description("جزئیات توضیحات کلاسها")]
        OnlineClassAccorationDetails,

        [Description("پرداخت های زرین پال")]
        ZarinpalPayments,

        [Description("ارسال پیامک و ایمیل")]
        SendSms,
        [Description("گروه کاربران")]
        GroupUser,
        [Description("کاربران")]
        GroupDetail,
        [Description("نظرات کاربران")]
        UserComments,
        [Description("درخواست باز شدن کلاس جدید")]
        OnlineClassRequests,
        [Description("مشاهده و دانلود ها")]
        ObjCount,


        [Description("گروه نظرسنجی")]
        GroupSurveys,
        [Description("نظرسنجی")]
        SurveyEntitys,
        [Description("گروه سوال نظرسنجی")]
        SurveyGroupQuestions,
        [Description("سوالات نظرسنجی")]
        SurveyQuestions,
        [Description("گزینه های سوالات نظرسنجی")]
        SurveyQuestionOptions,
        [Description("کاربران پاسخ دهنده نظرسنجی")]
        SurveyUserAnswers,
        [Description("پاسخ های نظرسنجی")]
        SurveyAnswers,
        [Description("گروه اختصاصی نظرسنجی")]
        SurveyPrivateGroups,
        [Description("کاربران گروه اختصاصی نظرسنجی")]
        SurveyPrivateGroupUsers,


        [Description("گروه آزمون")]
        GroupAzmoons,
        [Description("آزمون")]
        AzmoonEntitys,
        [Description("گروه سوال آزمون")]
        AzmoonGroupQuestions,
        [Description("سوالات آزمون")]
        AzmoonQuestions,
        [Description("گزینه های سوالات آزمون")]
        AzmoonQuestionOptions,
        [Description("کاربران پاسخ دهنده آزمون")]
        AzmoonUserAnswers,
        [Description("پاسخ های آزمون")]
        AzmoonAnswers,
        [Description("گروه اختصاصی آزمون")]
        AzmoonPrivateGroups,
        [Description("کاربران گروه اختصاصی آزمون")]
        AzmoonPrivateGroupUsers,
        [Description("سازمان ها")]
        Org,
        [Description("سمت")]
        Post,
        [Description("دوره پرسنل")]
        PersonCourse,
        [Description("استاد پرسنل")]
        PersonTeacher,
    }
}