namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class complation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDate", c => c.DateTime(nullable: false));
        }
    }
}
