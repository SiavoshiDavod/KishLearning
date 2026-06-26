namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class answerUser_no_requared : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers");
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyUserAnswerId" });
            AlterColumn("dbo.SurveyAnswers", "SurveyUserAnswerId", c => c.Int());
            AlterColumn("dbo.SurveyUserAnswers", "Name", c => c.String(maxLength: 20));
            AlterColumn("dbo.SurveyUserAnswers", "Family", c => c.String(maxLength: 20));
            AlterColumn("dbo.SurveyUserAnswers", "UserName", c => c.String(maxLength: 50));
            AlterColumn("dbo.SurveyUserAnswers", "OldYear", c => c.Byte());
            AlterColumn("dbo.SurveyUserAnswers", "Province", c => c.Int());
            CreateIndex("dbo.SurveyAnswers", "SurveyUserAnswerId");
            AddForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers");
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyUserAnswerId" });
            AlterColumn("dbo.SurveyUserAnswers", "Province", c => c.Int(nullable: false));
            AlterColumn("dbo.SurveyUserAnswers", "OldYear", c => c.Byte(nullable: false));
            AlterColumn("dbo.SurveyUserAnswers", "UserName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.SurveyUserAnswers", "Family", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.SurveyUserAnswers", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.SurveyAnswers", "SurveyUserAnswerId", c => c.Int(nullable: false));
            CreateIndex("dbo.SurveyAnswers", "SurveyUserAnswerId");
            AddForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers", "Id", cascadeDelete: true);
        }
    }
}
