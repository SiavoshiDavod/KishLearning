namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_res_menu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResturantMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                        Description = c.String(),
                        AdminDescription = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.ResturantId);
            
            CreateTable(
                "dbo.ResturantDetailMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        OldPrice = c.Int(),
                        NewPrice = c.Int(nullable: false),
                        FinalPrice = c.Int(nullable: false),
                        AdvertisingMenuTypeId = c.Byte(nullable: false),
                        ResturantMenuId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResturantMenus", t => t.ResturantMenuId, cascadeDelete: true)
                .Index(t => t.ResturantMenuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResturantDetailMenus", "ResturantMenuId", "dbo.ResturantMenus");
            DropForeignKey("dbo.ResturantMenus", "ResturantId", "dbo.Resturants");
            DropIndex("dbo.ResturantDetailMenus", new[] { "ResturantMenuId" });
            DropIndex("dbo.ResturantMenus", new[] { "ResturantId" });
            DropTable("dbo.ResturantDetailMenus");
            DropTable("dbo.ResturantMenus");
        }
    }
}
