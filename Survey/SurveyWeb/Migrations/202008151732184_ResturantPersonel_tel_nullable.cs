namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResturantPersonel_tel_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResturantPersonels", "Tel", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResturantPersonels", "Tel", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
