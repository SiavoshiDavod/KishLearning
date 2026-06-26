namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _change_BuildYear_todatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Resturants", "BuildYear", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Resturants", "BuildYear", c => c.Int(nullable: false));
        }
    }
}
