namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_requerdModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SurveyPrivateGroups", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Roles", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.SurveyPrivateGroups", "Name", c => c.String(maxLength: 100));
        }
    }
}
