namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _alter_shekayat_resturantCodeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shekayats", "ResturantCode", c => c.Int());
            AddColumn("dbo.Shekayats", "ResturantName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shekayats", "ResturantName");
            DropColumn("dbo.Shekayats", "ResturantCode");
        }
    }
}
