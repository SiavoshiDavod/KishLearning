namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _change_ResturantPersonelRelations : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ResturantPersonelLanguages", "OtherCountry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResturantPersonelLanguages", "OtherCountry", c => c.String(maxLength: 100));
        }
    }
}
