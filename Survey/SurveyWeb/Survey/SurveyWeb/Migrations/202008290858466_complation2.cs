namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class complation2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplaintCheckListItems", "ValueItem", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComplaintCheckListItems", "ValueItem");
        }
    }
}
