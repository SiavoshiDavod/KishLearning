namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa_desc_in_cartableLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartableLogs", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CartableLogs", "EntityId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CartableLogs", "EntityId", c => c.String());
            DropColumn("dbo.CartableLogs", "Description");
        }
    }
}
