using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteSetting
{
    public class SiteSetting
    {
        [DataType(DataType.MultilineText)]
        public string Contract { get; set; } = "قرارداد همکاری با ما";
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = "Kish e government";
        [StringLength(200)]
        public string NameFa { get; set; } = "دولت الکترونیک کیش";
        [StringLength(160)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = "منطقه آزاد تجاری - صنعتی کیش یکی از مناطق هفتگانه آزاد ایران است که در استان هرمزگان قرار دارد. منطقه آزاد کیش شامل جزایر کیش، هندرابی، فارور بزرگ و فارور کوچک است و اداره امور آن برعهده سازمانی است که مسئولیت‌هایش از سوی شورای عالی مناطق آزاد تجاری - صنعتی کشور تهیه و تصویب شده‌است.";
        [StringLength(200)]
        public string Keywords { get; set; } = "دولت ,الکترونیک, نظرسنجی , کیش";
        [StringLength(200)]
        public string SiteUrl { get; set; } = "http://kishpoll.com/";
        [StringLength(200)]
        public string EmailServer { get; set; } = "kishpoll.com";
        [StringLength(200)]
        public string EmailUser { get; set; } = "info";
        [StringLength(200)]
        public string EmailInfo { get; set; } = "info";
        [StringLength(200)]
        public string EmailPass { get; set; } = "123!@#qwe";
        [StringLength(200)]
        public string FaraPayamakUser { get; set; } = "fereidoonrezaei";
        [StringLength(200)]
        public string FaraPayamakPass { get; set; } = "55674694";
        [StringLength(200)]
        public string FaraPayamakNumber { get; set; } = "50004000504110";
        [StringLength(200)]
        public string AdobeServerUrl { get; set; } = "http://46.209.20.165/api/xml";
        [StringLength(200)]
        public string AdobeAdminUser { get; set; } = "rahmatymahdi@gmail.com";
        [StringLength(200)]
        public string AdobeAdminPass { get; set; } = "123456";
        [StringLength(200)]
        public string DefaultVideoUrl { get; set; } = "/files/SurveyPreview.mp4";
        [StringLength(200)]
        public string LogoUrl { get; set; } = @"\images\logo.png";
        [StringLength(200)]
        public string JwtTokenSecretKey { get; set; } = "favabandarabbasUserBizApiTokenController";
        [StringLength(200)]
        public string ZarinPalMerchantID { get; set; } = "92137998-efa5-11e8-a7bc-005056a205be";
        public bool isPecPayment { get; set; } = true;
        [StringLength(200)]
        public string GoogleSecretKey { get; set; } = "6LdlcpMUAAAAANWoRryyWDU9TGSkPI5aE9Yw87jg";
        [StringLength(200)]
        public string GoogleSiteKey { get; set; } = "6LdlcpMUAAAAAP60SvjHIbVPGrxvbEt5pvSWe_WF";
        [StringLength(200)]
        public string Address { get; set; } = "	ایران، استان هرمزگان، جزیره زیبای کیش";
        [StringLength(200)]
        public string PostalCode { get; set; }
        [StringLength(200)]
        public string Mobile { get; set; } = " 07644422141";// "09335386978-09216353148";
        [StringLength(200)]
        public string tel { get; set; }
        [StringLength(200)]
        public string Telegram { get; set; } = "https://t.me/kishlearning";
        [StringLength(200)]
        public string Instagram { get; set; }
        [StringLength(200)]
        public string Twitter { get; set; }
        [StringLength(200)]
        public string WhatsUp { get; set; }
        [StringLength(200)]
        public string Skype { get; set; }

        [StringLength(400)]
        public string side_content_Desc { get; set; } = "کلیه سیستمهای الکترونیکی را در خدمت شما قرار می دهد. به راحتی می توانید کلیه امورتان را از طریق این پلتفرم انجام دهید.";
        
        [StringLength(100)]
        public string side_content_Title { get; set; } = "جامعه مراکز پذایرایی کیش";
        
        [StringLength(100)]
        public string side_content_SubTitle { get; set; } = "منطقه آزاد صنعتی تجاری و گردشگری";
        
        [StringLength(200)]
        public string Poshtibani247 { get; set; } = "پشتیبانی آنلاین 24x7";
        
        [StringLength(200)]
        public string WorkingHours { get; set; } = "شنبه-پنجشنبه 8 صبح-8شب";
        [StringLength(200)]
        public string Footer_AboutUs { get; set; } = "درباره ما";
        [StringLength(200)]
        public string Footer_AboutUsLink { get; set; } = "#";
        [StringLength(200)]
        public string Footer_ServiceLink { get; set; } = "#";
        [StringLength(200)]
        public string Footer_WebLogLink { get; set; } = "#";
        [StringLength(200)]
        public string Footer_LegalLink { get; set; } = "#";
        [StringLength(200)]
        public string Footer_PoliticsLink { get; set; } = "#";
        [StringLength(200)]
        public string Footer_MobileTel { get; set; } = "#";
        [StringLength(200)]
        public string Footer_CopyRight { get; set; } = "کپی رایت 1398. تمام حقوق برای ما محفوظ است";

        [StringLength(200)]
        public string Banner_Title1 { get; set; } = "به درگاه ما خوش آمدید.";

        [StringLength(500)]
        public string Banner_Description1 { get; set; } = "";

        [StringLength(200)]
        public string Banner_Title2 { get; set; } = "7 روز هفته 24 ساعته در خدمت شماست.";

        [StringLength(500)]
        public string Banner_Description2 { get; set; } = "";

        [StringLength(200)]
        public string Banner_Title3 { get; set; } = "در چشم انداز1400.";

        [StringLength(500)]
        public string Banner_Description3 { get; set; } = "";

        [StringLength(200)]
        public string Banner_Btn1 { get; set; } = "پیشنهادات";

        [StringLength(200)]
        public string Banner_LinkBtn1 { get; set; } = "/home/suggestion";

        [StringLength(200)]
        public string Banner_Btn2 { get; set; } = "شکایات";

        [StringLength(200)]
        public string Banner_LinkBtn2 { get; set; } = "/home/Complaint";

        [StringLength(200)]
        public string Banner_Btn3 { get; set; } = "نظرسنجی";

        [StringLength(200)]
        public string Banner_LinkBtn3 { get; set; } = "/home/AllActiveSurvey";

        [StringLength(200)]
        public string Banner_Btn4 { get; set; } = "ثبت ایده ها";

        [StringLength(200)]
        public string Banner_LinkBtn4 { get; set; } = "/home/idea";

        [StringLength(200)]
        public string Banner_Btn5 { get; set; } = "آئین نامه وشیوه نامه";

        [StringLength(200)]
        public string Banner_LinkBtn5 { get; set; } = "/home/Regulation";

        [StringLength(200)]
        public string Banner_Btn6 { get; set; } = "آخرین اخبار";

        [StringLength(200)]
        public string Banner_LinkBtn6 { get; set; } = "/home/Allnews";

    }
}
