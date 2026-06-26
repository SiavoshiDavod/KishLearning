namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class menu_Sub : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuSubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        MenuId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Url = c.String(maxLength: 1000),
                        Image = c.String(maxLength: 100),
                        Content = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuSubs", "MenuId", "dbo.Menus");
            DropIndex("dbo.MenuSubs", new[] { "MenuId" });
            DropTable("dbo.MenuSubs");
            DropTable("dbo.Menus");
        }
    }
}
