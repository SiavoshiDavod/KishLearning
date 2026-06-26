namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_IsImageDirectionLeft : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrgIntroes", "IsImageDirectionLeft", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrgIntroes", "IsImageDirectionLeft");
        }
    }
}
