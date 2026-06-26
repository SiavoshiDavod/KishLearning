using SenakLearn.Models.Person;
using SenakLearn.Models.Security;
using System;
using System.Collections.Generic;

namespace SenakLearn.Models
{
    public static class Dictionary
    {
        public static Dictionary<Type, string> ModelDictionary = new Dictionary<Type, string> {
            { typeof(Menu),"منو"},
            { typeof(DynamicForm),"زیرمنو"},
            { typeof(News),"اخبار"},
            { typeof(NewsGroup),"گروه خبر"},
            { typeof(Author),"نویسنده"},
            { typeof(learn_user),"کاربران"},
            { typeof(Role),"نقش"},
            { typeof(CompanyLogoAndLink),"لوگوی سازمانهای مرتبط"},
            { typeof(RolePermission),"دسترسی های نقش"},
            { typeof(RoleUser),"نقش های کاربران"},
            { typeof(learn_teacher),"ثبت اساتید"},
            { typeof(JoinUs),"همکاری با ما"},
            { typeof(Book),"کتاب"},
            { typeof(Paper),"مقاله"},
            { typeof(PaperTranslateQuality),"کیفیت"},
            { typeof(PaperUniversity),"دانشگاه"},
            { typeof(PaperJournal),"ژورنال"},
            { typeof(PaperPublisher),"ناشر"},
            { typeof(PaperTrend),"گرایش"},
            { typeof(PaperField),"رشته"},
            { typeof(VideoFile),"آپلود ویدیو"},
            { typeof(StudentSupport),"پشتیبانی دانشجویان"},
            { typeof(TeacherSupport),"پشتیبانی اساتید"},
            { typeof(OfflineVideo),"کلاس آفلاین"},
            { typeof(SiteSetting.SiteSetting),"تنظیمات سایت"},
            { typeof(learn_cours_group),"گروه بندی دوره ها"},
            { typeof(learn_cours),"فیلم های آموزشی"},
            { typeof(OnlineClass),"کلاس های آنلاین"},
            { typeof(OnlineClassAccoration),"توضیحات کلاسها"},
            { typeof(OnlineClassAccorationDetails),"جزئیات توضیحات کلاسها"},
            { typeof(ZarinpalPayment),"پرداخت های زرین پال"},
            { typeof(EmailSms),"ارسال پیامک و ایمیل"},
            { typeof(Group),"گروه کاربران"},
            { typeof(GroupDetail),"کاربران"},
            { typeof(UserCommnet),"نظرات کاربران"},
            { typeof(ObjCount),"مشاهده و دانلود ها"},
            { typeof(OnlineClassRequest),"درخواست باز شدن کلاس جدید"},

              { typeof(GroupSurvey),"گروه نظرسنجی"},
            { typeof(SurveyAnswer),"پاسخ های نظرسنجی"},
            { typeof(SurveyEntity),"نظرسنجی"},
            { typeof(SurveyGroupQuestion),"گروه سوال نظرسنجی"},
            { typeof(SurveyQuestion),"سوال نظرسنجی"},
            { typeof(SurveyQuestionOption),"گزینه های سوال نظرسنجی"},
            { typeof(SurveyUserAnswer),"کاربران پاسخ دهنده به نظرسنجی"},
            { typeof(SurveyPrivateGroup),"گروه اختصاصی نظرسنجی"},
            { typeof(SurveyPrivateGroupUser),"کاربران گروه اختصاصی نظرسنجی"},

             { typeof(GroupAzmoon),"گروه آزمون"},
            { typeof(AzmoonAnswer),"پاسخ های آزمون"},
            { typeof(AzmoonEntity),"آزمون"},
            { typeof(AzmoonGroupQuestion),"گروه سوال آزمون"},
            { typeof(AzmoonQuestion),"سوال آزمون"},
            { typeof(AzmoonQuestionOption),"گزینه های سوال آزمون"},
            { typeof(AzmoonUserAnswer),"کاربران پاسخ دهنده به آزمون"},
            { typeof(AzmoonPrivateGroup),"گروه اختصاصی آزمون"},
            { typeof(AzmoonPrivateGroupUser),"کاربران گروه اختصاصی آزمون"},
            { typeof(Organization),"سازمان ها"},
            { typeof(Post),"سمت"},
            { typeof(Person_Course),"دوره پرسنل"},
            { typeof(Person_Teacher),"استاد پرسنل"},
            { typeof(Person_Certificate),"مدارک پرسنل"},
        };

    }
}