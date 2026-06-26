namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_ticket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(maxLength: 1000),
                        SenderUserId = c.Int(nullable: false),
                        ReceiverUserId = c.Int(),
                        IsRead = c.Boolean(nullable: false),
                        Answer = c.String(maxLength: 1000),
                        File = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReceiverUserId)
                .ForeignKey("dbo.Users", t => t.SenderUserId, cascadeDelete: true)
                .Index(t => t.SenderUserId)
                .Index(t => t.ReceiverUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "SenderUserId", "dbo.Users");
            DropForeignKey("dbo.Tickets", "ReceiverUserId", "dbo.Users");
            DropIndex("dbo.Tickets", new[] { "ReceiverUserId" });
            DropIndex("dbo.Tickets", new[] { "SenderUserId" });
            DropTable("dbo.Tickets");
        }
    }
}
