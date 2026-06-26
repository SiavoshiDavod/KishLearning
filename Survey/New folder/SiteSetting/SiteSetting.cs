using System.ComponentModel.DataAnnotations;

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
    }
}
