namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _changeEmail200 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Resturants", "CompanyEmail", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Resturants", "CompanyEmail", c => c.String(maxLength: 20));
        }
    }
}
