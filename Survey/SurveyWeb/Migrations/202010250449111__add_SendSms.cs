namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_SendSms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailSms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        To = c.String(nullable: false, maxLength: 100),
                        From = c.String(nullable: false, maxLength: 100),
                        Body = c.String(nullable: false, maxLength: 1000),
                        Subject = c.String(nullable: false, maxLength: 100),
                        IsSend = c.Boolean(nullable: false),
                        SendResult = c.String(nullable: false, maxLength: 200),
                        EmailSmsType = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailSms");
        }
    }
}
