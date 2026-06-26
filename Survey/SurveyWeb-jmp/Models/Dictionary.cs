using SurveyWeb.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SurveyWeb.Models
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
        SurveyPrivateGroupUsers,
        [Description("تنظیمات سایت")]
        SiteSetting,

        [Description("مراکزپذیرایی")]
        Resturants,
        [Description("نوع مراکزپذیرایی")]
        ResturantTypes,
        [Description("نوع مدارک پیوست")]
        CheckListTypes,
    }

    public static class Dictionary
    {
        public static Dictionary<Type, string> ModelDictionary = new Dictionary<Type, string> {
            { typeof(Menu),"منو"},
            { typeof(MenuSub),"زیرمنو"},
            { typeof(Cartable),"کارتابل"},
            { typeof(CartableLog),"لاگ کارتابل"},
            { typeof(CartableRelation),"ارتباطات کارتابل"},
            { typeof(CartableUserAccess),"دسترسی کارتابل"},
            { typeof(Complaint),"شکایات"},
            { typeof(ContactUs),"تماس با ما"},
            { typeof(Faq),"سوالات متداول"},
            { typeof(GroupSurvey),"گروه نظرسنجی"},
            { typeof(Idea),"ایده ها"},
            { typeof(OrgIntro),"آشنائی با سازمان"},
            { typeof(News),"اخبار"},
            { typeof(NewsGroup),"گروه خبر"},
            { typeof(Author),"نویسنده"},
            { typeof(NewsSubscription),"خبرنامه"},
            { typeof(Regulation),"آئین نامه وشیوه نامه"},
            { typeof(Suggestion),"پیشنهادات"},
            { typeof(SurveyAnswer),"پاسخ های نظرسنجی"},
            { typeof(SurveyEntity),"نظرسنجی"},
            { typeof(SurveyGroupQuestion),"گروه سوال نظرسنجی"},
            { typeof(SurveyQuestion),"سوال نظرسنجی"},
            { typeof(SurveyQuestionOption),"گزینه های سوال نظرسنجی"},
            { typeof(SurveyUserAnswer),"کاربران پاسخ دهنده به نظرسنجی"},
            { typeof(User),"کاربران"},
            { typeof(Role),"نقش"},
            { typeof(CompanyLogoAndLink),"لوگوی سازمانهای مرتبط"},
            { typeof(RolePermission),"دسترسی های نقش"},
            { typeof(RoleUser),"نقش های کاربران"},
            { typeof(SurveyPrivateGroup),"گروه اختصاصی نظرسنجی"},
            { typeof(SurveyPrivateGroupUser),"کاربران گروه اختصاصی نظرسنجی"},
            { typeof(UserComment),"نظرات کاربران"},
             { typeof(SiteSetting.SiteSetting),"تنظیمات سایت"},
             { typeof(Resturant),"مراکزپذیرایی"},
             { typeof(ResturantType),"نوع مراکزپذیرایی"},
             { typeof(CheckListType),"نوع مدارک پیوست"},
        };

    }
}