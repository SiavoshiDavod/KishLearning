namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_adverting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantId = c.Int(nullable: false),
                        Description = c.String(maxLength: 4000),
                        LinkReserve = c.String(maxLength: 200),
                        ImageUrl = c.String(maxLength: 100),
                        Archive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.ResturantId);
            
            CreateTable(
                "dbo.AdvertisingAttachements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(maxLength: 100),
                        IsVideo = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Resturants", "IsFavorite", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "IsMusical", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertisings", "ResturantId", "dbo.Resturants");
            DropIndex("dbo.Advertisings", new[] { "ResturantId" });
            DropColumn("dbo.Resturants", "IsMusical");
            DropColumn("dbo.Resturants", "IsFavorite");
            DropTable("dbo.AdvertisingAttachements");
            DropTable("dbo.Advertisings");
        }
    }
}
