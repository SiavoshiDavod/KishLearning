using System.ComponentModel;

namespace BaseModel
{
    public enum Permisstion
    {
        [Description("دسترسی های نقش")]
        RolePermissions,
        [Description("کاربران")]
        Users,
        [Description("نقش")]
        Roles,
        [Description("نقش های کاربر")]
        RoleUsers,

        [Description("منو")]
        Menus,
        [Description("زیرمنو")]
        MenuSubs,

        [Description("کارتابل")]
        Cartables,
        [Description("ارتباط کارتابل")]
        CartableRelations,
        [Description("دسترسی های کارتابل")]
        CartableUserAccesses,
        [Description("لاگ کارتابل")]
        CartableLogs,

        [Description("شکایات")]
        Complaints,
        [Description("ایده ها")]
        Ideas,
        [Description("پیشنهادات")]
        Suggestions,

        [Description("تماس با ما")]
        ContactUs,

        [Description("اخبار")]
        News,
        [Description("گروه خبر")]
        NewsGroups,
        [Description("نویسنده")]
        Authors,
        [Description("خبرنامه")]
        NewsSubscriptions,

        //[Description("مزایده/مناقضه")]
        //Tender,

        [Description("سوالات متداول")]
        Faqs,
        [Description("آیین نامه/شیوه نامه")]
        Regulations,

        [Description("نظرات کاربران")]
        UserComment,
        [Description("آشنائی با سازمان")]
        OrgIntroes,

        [Description("لوگوی سازمانهای مرتبط")]
        CompanyLogoAndLinks,

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
        [Description("کاربران پاسخ دهنده")]
        SurveyUserAnswers,
        [Description("پاسخ های نظرسنجی")]
        SurveyAnswers,
        [Description("گروه اختصاصی نظرسنجی")]
        SurveyPrivateGroups,
        [Description("کاربران گروه اختصاصی نظرسنجی")]
        SurveyPrivateGroupUsers
    }

}
