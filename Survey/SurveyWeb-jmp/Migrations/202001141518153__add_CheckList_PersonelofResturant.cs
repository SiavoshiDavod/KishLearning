namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_CheckList_PersonelofResturant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResturantCheckLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantId = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 100),
                        CheckList = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.ResturantId);
            
            CreateTable(
                "dbo.ResturantPersonels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.ResturantId);
            
            AddColumn("dbo.Resturants", "Name", c => c.String(maxLength: 100));
            DropColumn("dbo.Resturants", "Staff1");
            DropColumn("dbo.Resturants", "Staff2");
            DropColumn("dbo.Resturants", "Staff3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Resturants", "Staff3", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "Staff2", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "Staff1", c => c.String(maxLength: 100));
            DropForeignKey("dbo.ResturantPersonels", "ResturantId", "dbo.Resturants");
            DropForeignKey("dbo.ResturantCheckLists", "ResturantId", "dbo.Resturants");
            DropIndex("dbo.ResturantPersonels", new[] { "ResturantId" });
            DropIndex("dbo.ResturantCheckLists", new[] { "ResturantId" });
            DropColumn("dbo.Resturants", "Name");
            DropTable("dbo.ResturantPersonels");
            DropTable("dbo.ResturantCheckLists");
        }
    }
}
