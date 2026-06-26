namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_News : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BirthDay = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        ImageUrl = c.String(maxLength: 100),
                        Description = c.String(maxLength: 2000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        NewsGroupId = c.Int(nullable: false),
                        Summary = c.String(nullable: false, maxLength: 1000),
                        Keyword = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        VisitCount = c.Int(nullable: false),
                        AuthorId = c.Int(),
                        ImageUrl = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId)
                .ForeignKey("dbo.NewsGroups", t => t.NewsGroupId, cascadeDelete: true)
                .Index(t => t.NewsGroupId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.NewsGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "NewsGroupId", "dbo.NewsGroups");
            DropForeignKey("dbo.News", "AuthorId", "dbo.Authors");
            DropIndex("dbo.News", new[] { "AuthorId" });
            DropIndex("dbo.News", new[] { "NewsGroupId" });
            DropTable("dbo.NewsGroups");
            DropTable("dbo.News");
            DropTable("dbo.Authors");
        }
    }
}
