namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_titleToSurveyEntity_widthToOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SurveyEntities", "IsFavorite", c => c.Boolean(nullable: false));
            AddColumn("dbo.SurveyEntities", "IsImportant", c => c.Boolean(nullable: false));
            AddColumn("dbo.SurveyEntities", "Title", c => c.String());
            AddColumn("dbo.SurveyEntities", "Description", c => c.String());
            AddColumn("dbo.SurveyQuestionOptions", "Width", c => c.Short(nullable: false));
            AddColumn("dbo.SurveyQuestionOptions", "Height", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SurveyQuestionOptions", "Height");
            DropColumn("dbo.SurveyQuestionOptions", "Width");
            DropColumn("dbo.SurveyEntities", "Description");
            DropColumn("dbo.SurveyEntities", "Title");
            DropColumn("dbo.SurveyEntities", "IsImportant");
            DropColumn("dbo.SurveyEntities", "IsFavorite");
        }
    }
}
