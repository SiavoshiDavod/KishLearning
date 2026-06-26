namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class truePlus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckListGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CheckListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CheckListGroupId = c.Int(nullable: false),
                        CheckListItemType = c.Int(nullable: false),
                        CheckListId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckLists", t => t.CheckListId, cascadeDelete: true)
                .ForeignKey("dbo.CheckListGroups", t => t.CheckListGroupId, cascadeDelete: true)
                .Index(t => t.CheckListGroupId)
                .Index(t => t.CheckListId);
            
            CreateTable(
                "dbo.CheckLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintCheckListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComplaintCheckListId = c.Int(nullable: false),
                        CheckListItemId = c.Int(nullable: false),
                        IsYesNo = c.Boolean(),
                        IsGoodMidBad = c.Int(),
                        IsHasItDontHave = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckListItems", t => t.CheckListItemId, cascadeDelete: true)
                .ForeignKey("dbo.ComplaintCheckLists", t => t.ComplaintCheckListId, cascadeDelete: true)
                .Index(t => t.ComplaintCheckListId)
                .Index(t => t.CheckListItemId);
            
            CreateTable(
                "dbo.ComplaintCheckLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CheckListId = c.Int(nullable: false),
                        ResturantId = c.Int(nullable: false),
                        ComplaintDatePersian = c.String(),
                        ComplaintTimePersian = c.String(),
                        ComplaintDate = c.DateTime(nullable: false),
                        UserComplaintId = c.Int(nullable: false),
                        Descript = c.String(),
                        DayNumResolve = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckLists", t => t.CheckListId, cascadeDelete: true)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserComplaintId, cascadeDelete: true)
                .Index(t => t.CheckListId)
                .Index(t => t.ResturantId)
                .Index(t => t.UserComplaintId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComplaintCheckListItems", "ComplaintCheckListId", "dbo.ComplaintCheckLists");
            DropForeignKey("dbo.ComplaintCheckLists", "UserComplaintId", "dbo.Users");
            DropForeignKey("dbo.ComplaintCheckLists", "ResturantId", "dbo.Resturants");
            DropForeignKey("dbo.ComplaintCheckLists", "CheckListId", "dbo.CheckLists");
            DropForeignKey("dbo.ComplaintCheckListItems", "CheckListItemId", "dbo.CheckListItems");
            DropForeignKey("dbo.CheckListItems", "CheckListGroupId", "dbo.CheckListGroups");
            DropForeignKey("dbo.CheckListItems", "CheckListId", "dbo.CheckLists");
            DropIndex("dbo.ComplaintCheckLists", new[] { "UserComplaintId" });
            DropIndex("dbo.ComplaintCheckLists", new[] { "ResturantId" });
            DropIndex("dbo.ComplaintCheckLists", new[] { "CheckListId" });
            DropIndex("dbo.ComplaintCheckListItems", new[] { "CheckListItemId" });
            DropIndex("dbo.ComplaintCheckListItems", new[] { "ComplaintCheckListId" });
            DropIndex("dbo.CheckListItems", new[] { "CheckListId" });
            DropIndex("dbo.CheckListItems", new[] { "CheckListGroupId" });
            DropTable("dbo.ComplaintCheckLists");
            DropTable("dbo.ComplaintCheckListItems");
            DropTable("dbo.CheckLists");
            DropTable("dbo.CheckListItems");
            DropTable("dbo.CheckListGroups");
        }
    }
}
