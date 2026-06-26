namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_sitesetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contract = c.String(),
                        Name = c.String(maxLength: 200),
                        NameFa = c.String(maxLength: 200),
                        Description = c.String(maxLength: 160),
                        Keywords = c.String(maxLength: 200),
                        SiteUrl = c.String(maxLength: 200),
                        EmailServer = c.String(maxLength: 200),
                        EmailUser = c.String(maxLength: 200),
                        EmailInfo = c.String(maxLength: 200),
                        EmailPass = c.String(maxLength: 200),
                        FaraPayamakUser = c.String(maxLength: 200),
                        FaraPayamakPass = c.String(maxLength: 200),
                        FaraPayamakNumber = c.String(maxLength: 200),
                        AdobeServerUrl = c.String(maxLength: 200),
                        AdobeAdminUser = c.String(maxLength: 200),
                        AdobeAdminPass = c.String(maxLength: 200),
                        DefaultVideoUrl = c.String(maxLength: 200),
                        LogoUrl = c.String(maxLength: 200),
                        JwtTokenSecretKey = c.String(maxLength: 200),
                        ZarinPalMerchantID = c.String(maxLength: 200),
                        isPecPayment = c.Boolean(nullable: false),
                        GoogleSecretKey = c.String(maxLength: 200),
                        GoogleSiteKey = c.String(maxLength: 200),
                        Address = c.String(maxLength: 200),
                        PostalCode = c.String(maxLength: 200),
                        Mobile = c.String(maxLength: 200),
                        tel = c.String(maxLength: 200),
                        Telegram = c.String(maxLength: 200),
                        Instagram = c.String(maxLength: 200),
                        Twitter = c.String(maxLength: 200),
                        WhatsUp = c.String(maxLength: 200),
                        Skype = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SiteSettings");
        }
    }
}
