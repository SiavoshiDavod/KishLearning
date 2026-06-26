namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _relation_resturant_shekayat : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Shekayats", "ResturantId");
            AddForeignKey("dbo.Shekayats", "ResturantId", "dbo.Resturants", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shekayats", "ResturantId", "dbo.Resturants");
            DropIndex("dbo.Shekayats", new[] { "ResturantId" });
        }
    }
}
