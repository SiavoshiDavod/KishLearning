namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_AdminMobiles_in_siteSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteSettings", "AdminMobiles", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteSettings", "AdminMobiles");
        }
    }
}
