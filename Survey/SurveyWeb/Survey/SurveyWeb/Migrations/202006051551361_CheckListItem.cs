namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckListItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CheckListItems", "CheckListId", c => c.Int(nullable: false));
            CreateIndex("dbo.CheckListItems", "CheckListId");
            AddForeignKey("dbo.CheckListItems", "CheckListId", "dbo.CheckLists", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckListItems", "CheckListId", "dbo.CheckLists");
            DropIndex("dbo.CheckListItems", new[] { "CheckListId" });
            DropColumn("dbo.CheckListItems", "CheckListId");
        }
    }
}
