namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _change_siavoshi_checklist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplaintCheckListItems", "ValueItem", c => c.Int());
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDatePersian", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintTimePersian", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintTimePersian", c => c.String());
            AlterColumn("dbo.ComplaintCheckLists", "ComplaintDatePersian", c => c.String());
            DropColumn("dbo.ComplaintCheckListItems", "ValueItem");
        }
    }
}
