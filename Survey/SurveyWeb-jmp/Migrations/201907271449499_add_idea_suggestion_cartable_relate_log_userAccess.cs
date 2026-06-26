namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_idea_suggestion_cartable_relate_log_userAccess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cartables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        CartableType = c.Int(nullable: false),
                        IsFirstState = c.Boolean(nullable: false),
                        IsLastState = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CartableRelations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.From, cascadeDelete: true)
                .ForeignKey("dbo.Cartables", t => t.To, cascadeDelete: true)
                .Index(t => t.From)
                .Index(t => t.To);
            
            CreateTable(
                "dbo.CartableLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityId = c.String(),
                        UserId = c.Int(nullable: false),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.From, cascadeDelete: true)
                .ForeignKey("dbo.Cartables", t => t.To, cascadeDelete: true)
                .Index(t => t.From)
                .Index(t => t.To);
            
            CreateTable(
                "dbo.CartableUserAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CartableId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        UnitOrJob = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        Problem = c.String(maxLength: 3000),
                        Proposal = c.String(maxLength: 3000),
                        Description = c.String(maxLength: 3000),
                        Cost = c.String(maxLength: 3000),
                        Benefit = c.String(maxLength: 3000),
                        Experience = c.String(maxLength: 3000),
                        Attachment1 = c.String(maxLength: 100),
                        Attachment2 = c.String(maxLength: 100),
                        Attachment3 = c.String(maxLength: 100),
                        Attachment4 = c.String(maxLength: 100),
                        Attachment5 = c.String(maxLength: 100),
                        CartableId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.Suggestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        UnitOrJob = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        Title = c.String(maxLength: 3000),
                        Proposal = c.String(maxLength: 3000),
                        Description = c.String(maxLength: 3000),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Benefit = c.String(maxLength: 3000),
                        Experience = c.String(maxLength: 3000),
                        Attachment1 = c.String(maxLength: 100),
                        Attachment2 = c.String(maxLength: 100),
                        CartableId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suggestions", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.Ideas", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.CartableUserAccesses", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.CartableLogs", "To", "dbo.Cartables");
            DropForeignKey("dbo.CartableLogs", "From", "dbo.Cartables");
            DropForeignKey("dbo.CartableRelations", "To", "dbo.Cartables");
            DropForeignKey("dbo.CartableRelations", "From", "dbo.Cartables");
            DropIndex("dbo.Suggestions", new[] { "CartableId" });
            DropIndex("dbo.Ideas", new[] { "CartableId" });
            DropIndex("dbo.CartableUserAccesses", new[] { "CartableId" });
            DropIndex("dbo.CartableLogs", new[] { "To" });
            DropIndex("dbo.CartableLogs", new[] { "From" });
            DropIndex("dbo.CartableRelations", new[] { "To" });
            DropIndex("dbo.CartableRelations", new[] { "From" });
            DropTable("dbo.Suggestions");
            DropTable("dbo.Ideas");
            DropTable("dbo.CartableUserAccesses");
            DropTable("dbo.CartableLogs");
            DropTable("dbo.CartableRelations");
            DropTable("dbo.Cartables");
        }
    }
}
