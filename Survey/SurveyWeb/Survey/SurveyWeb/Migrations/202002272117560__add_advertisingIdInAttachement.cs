namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_advertisingIdInAttachement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertisingAttachements", "AdvertisingId", c => c.Int(nullable: false));
            CreateIndex("dbo.AdvertisingAttachements", "AdvertisingId");
            AddForeignKey("dbo.AdvertisingAttachements", "AdvertisingId", "dbo.Advertisings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdvertisingAttachements", "AdvertisingId", "dbo.Advertisings");
            DropIndex("dbo.AdvertisingAttachements", new[] { "AdvertisingId" });
            DropColumn("dbo.AdvertisingAttachements", "AdvertisingId");
        }
    }
}
