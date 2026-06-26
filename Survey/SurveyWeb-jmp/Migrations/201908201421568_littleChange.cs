namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class littleChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Complaints", "Title", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Complaints", "Title", c => c.String());
        }
    }
}
