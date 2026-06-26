namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_IsPublic_in_JobPositions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobPositions", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobPositions", "IsPublic");
        }
    }
}
