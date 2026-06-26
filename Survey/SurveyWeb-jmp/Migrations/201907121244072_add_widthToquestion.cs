namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_widthToquestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SurveyQuestions", "Width", c => c.Short(nullable: false));
            AddColumn("dbo.SurveyQuestions", "Height", c => c.Short(nullable: false));
            AlterColumn("dbo.SurveyEntities", "Title", c => c.String(maxLength: 200));
            AlterColumn("dbo.SurveyEntities", "Description", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SurveyEntities", "Description", c => c.String());
            AlterColumn("dbo.SurveyEntities", "Title", c => c.String());
            DropColumn("dbo.SurveyQuestions", "Height");
            DropColumn("dbo.SurveyQuestions", "Width");
        }
    }
}
