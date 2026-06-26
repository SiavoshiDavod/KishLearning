namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_role_permission_group_privateGroup_contactUs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Birthday = c.DateTime(nullable: false),
                        Title = c.String(),
                        Description = c.String(maxLength: 3000),
                        Attachment = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        UnitOrJob = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.ContactUs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 50),
                        Tel = c.String(maxLength: 20),
                        Ip = c.String(maxLength: 20),
                        Title = c.String(),
                        Description = c.String(maxLength: 3000),
                        CartableId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.SurveyPrivateGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsSubscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regulations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        File = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Permisstion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SurveyPrivateGroupUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyPrivateGroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyPrivateGroups", t => t.SurveyPrivateGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SurveyPrivateGroupId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.SurveyEntities", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.SurveyEntities", "SurveyPrivateGroupId", c => c.Int());
            AddColumn("dbo.Ideas", "Ip", c => c.String(maxLength: 20));
            AddColumn("dbo.Suggestions", "Ip", c => c.String(maxLength: 20));
            CreateIndex("dbo.SurveyEntities", "SurveyPrivateGroupId");
            AddForeignKey("dbo.SurveyEntities", "SurveyPrivateGroupId", "dbo.SurveyPrivateGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyPrivateGroupUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.SurveyPrivateGroupUsers", "SurveyPrivateGroupId", "dbo.SurveyPrivateGroups");
            DropForeignKey("dbo.RoleUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.SurveyEntities", "SurveyPrivateGroupId", "dbo.SurveyPrivateGroups");
            DropForeignKey("dbo.ContactUs", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.Complaints", "CartableId", "dbo.Cartables");
            DropIndex("dbo.SurveyPrivateGroupUsers", new[] { "UserId" });
            DropIndex("dbo.SurveyPrivateGroupUsers", new[] { "SurveyPrivateGroupId" });
            DropIndex("dbo.RoleUsers", new[] { "UserId" });
            DropIndex("dbo.RoleUsers", new[] { "RoleId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleId" });
            DropIndex("dbo.SurveyEntities", new[] { "SurveyPrivateGroupId" });
            DropIndex("dbo.ContactUs", new[] { "CartableId" });
            DropIndex("dbo.Complaints", new[] { "CartableId" });
            DropColumn("dbo.Suggestions", "Ip");
            DropColumn("dbo.Ideas", "Ip");
            DropColumn("dbo.SurveyEntities", "SurveyPrivateGroupId");
            DropColumn("dbo.SurveyEntities", "IsPrivate");
            DropTable("dbo.SurveyPrivateGroupUsers");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Roles");
            DropTable("dbo.Regulations");
            DropTable("dbo.NewsSubscriptions");
            DropTable("dbo.SurveyPrivateGroups");
            DropTable("dbo.ContactUs");
            DropTable("dbo.Complaints");
        }
    }
}
