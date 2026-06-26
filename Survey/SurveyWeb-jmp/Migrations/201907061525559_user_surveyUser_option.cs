namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_surveyUser_option : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SurveyUserAnswers", "Ip", c => c.String(maxLength: 20));
            AddColumn("dbo.Users", "Education", c => c.String(maxLength: 30));
            AddColumn("dbo.Users", "Job", c => c.String(maxLength: 30));
            AddColumn("dbo.Users", "IsMarried", c => c.Boolean(nullable: false));
            AddColumn("dbo.SurveyQuestionOptions", "QuestionOptionUrl", c => c.String(maxLength: 100));
            DropColumn("dbo.SurveyUserAnswers", "Name");
            DropColumn("dbo.SurveyUserAnswers", "Family");
            DropColumn("dbo.SurveyUserAnswers", "UserName");
            DropColumn("dbo.SurveyUserAnswers", "Mobile");
            DropColumn("dbo.SurveyUserAnswers", "OldYear");
            DropColumn("dbo.SurveyUserAnswers", "Province");
            DropColumn("dbo.SurveyUserAnswers", "UserImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SurveyUserAnswers", "UserImageUrl", c => c.String(maxLength: 100));
            AddColumn("dbo.SurveyUserAnswers", "Province", c => c.Int());
            AddColumn("dbo.SurveyUserAnswers", "OldYear", c => c.Byte());
            AddColumn("dbo.SurveyUserAnswers", "Mobile", c => c.String(maxLength: 11));
            AddColumn("dbo.SurveyUserAnswers", "UserName", c => c.String(maxLength: 50));
            AddColumn("dbo.SurveyUserAnswers", "Family", c => c.String(maxLength: 20));
            AddColumn("dbo.SurveyUserAnswers", "Name", c => c.String(maxLength: 20));
            DropColumn("dbo.SurveyQuestionOptions", "QuestionOptionUrl");
            DropColumn("dbo.Users", "IsMarried");
            DropColumn("dbo.Users", "Job");
            DropColumn("dbo.Users", "Education");
            DropColumn("dbo.SurveyUserAnswers", "Ip");
        }
    }
}
