namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_relation_user : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CartableUserAccesses", "UserId");
            AddForeignKey("dbo.CartableUserAccesses", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartableUserAccesses", "UserId", "dbo.Users");
            DropIndex("dbo.CartableUserAccesses", new[] { "UserId" });
        }
    }
}
