namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_shekayat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shekayats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(nullable: false, maxLength: 12),
                        Description = c.String(maxLength: 3000),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        TypeShekayatId = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shekayats", "CartableId", "dbo.Cartables");
            DropIndex("dbo.Shekayats", new[] { "CartableId" });
            DropTable("dbo.Shekayats");
        }
    }
}
