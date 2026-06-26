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
        public string Name { get; set; } = "KishLearning";
        [StringLength(200)]
        public string NameFa { get; set; } = "آموزش مجازی کیش";
        [StringLength(800)]
        public string Description { get; set; } = "اين سایت با در اختيار داشتن متخصصين علوم مختلف مانند مديريت، كسب و كار، فناوري و ... سعي در طراحي دوره هاي آموزشي مختلف نموده كه اميد است شما را در رسيدن به اهداف و برنامه هاي مختلفتان كمك و ياري نمايد. ";
        [StringLength(200)]
        public string Keywords { get; set; } = "آموزش ، مجازی ،کلاس آنلاین ، مقاله ، جزوه ، کتاب ، ویدیو";
        [StringLength(200)]
        public string SiteUrl { get; set; } = "https://kishlearning.com/";
        [StringLength(200)]
        public string EmailServer { get; set; } = "kishlearning.com";
        [StringLength(200)]
        public string EmailUser { get; set; } = "AlertForClass";
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
        public string DefaultVideoUrl { get; set; } = "/files/KishLearningPreview.mp4";
        [StringLength(200)]
        public string LogoUrl { get; set; } = @"\images\logo.png";
        [StringLength(200)]
        public string JwtTokenSecretKey { get; set; } = "KishLearningUserBizApiTokenController";
        [StringLength(200)]
        public string ZarinPalMerchantID { get; set; } = "92137998-efa5-11e8-a7bc-005056a205be";
        public bool isPecPayment { get; set; } = true;
        [StringLength(200)]
        public string GoogleSecretKey { get; set; } = "6LdlcpMUAAAAANWoRryyWDU9TGSkPI5aE9Yw87jg";
        [StringLength(200)]
        public string GoogleSiteKey { get; set; } = "6LdlcpMUAAAAAP60SvjHIbVPGrxvbEt5pvSWe_WF";
        [StringLength(200)]
        public string Address { get; set; } = "کیش";
        [StringLength(200)]
        public string PostalCode { get; set; }
        [StringLength(200)]
        public string Mobile { get; set; } = "09335386978-09216353148";
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
