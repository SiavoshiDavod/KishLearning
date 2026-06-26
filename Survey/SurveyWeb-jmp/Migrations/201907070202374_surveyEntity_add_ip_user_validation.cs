namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class surveyEntity_add_ip_user_validation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers");
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyUserAnswerId" });
            AddColumn("dbo.SurveyEntities", "IsIpRestriction", c => c.Boolean(nullable: false));
            AddColumn("dbo.SurveyEntities", "IsUserMustBeLogin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SurveyUserAnswers", "SurveyEntityId", c => c.Int(nullable: false));
            AlterColumn("dbo.SurveyAnswers", "SurveyUserAnswerId", c => c.Int(nullable: false));
            CreateIndex("dbo.SurveyAnswers", "SurveyUserAnswerId");
            AddForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers", "Id", cascadeDelete: true);
            DropColumn("dbo.SurveyAnswers", "SurveyEntityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SurveyAnswers", "SurveyEntityId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers");
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyUserAnswerId" });
            AlterColumn("dbo.SurveyAnswers", "SurveyUserAnswerId", c => c.Int());
            DropColumn("dbo.SurveyUserAnswers", "SurveyEntityId");
            DropColumn("dbo.SurveyEntities", "IsUserMustBeLogin");
            DropColumn("dbo.SurveyEntities", "IsIpRestriction");
            CreateIndex("dbo.SurveyAnswers", "SurveyUserAnswerId");
            AddForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers", "Id");
        }
    }
}
