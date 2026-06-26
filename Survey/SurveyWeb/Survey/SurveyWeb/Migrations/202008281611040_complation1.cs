    namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class complation1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDatePersian", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintTimePersian", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDate", c => c.DateTime());
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintTimePersian", c => c.String());
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDatePersian", c => c.String());
        }
    }
}
