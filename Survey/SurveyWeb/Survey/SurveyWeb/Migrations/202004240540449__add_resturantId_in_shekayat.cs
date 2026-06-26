namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_resturantId_in_shekayat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shekayats", "ResturantId", c => c.Int());
            DropColumn("dbo.Shekayats", "ResturantCode");
            DropColumn("dbo.Shekayats", "ResturantName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shekayats", "ResturantName", c => c.String(maxLength: 50));
            AddColumn("dbo.Shekayats", "ResturantCode", c => c.Int());
            DropColumn("dbo.Shekayats", "ResturantId");
        }
    }
}
