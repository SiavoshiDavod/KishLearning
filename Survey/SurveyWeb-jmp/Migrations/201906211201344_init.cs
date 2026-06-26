namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupSurveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupSurveyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        SurveyImageUrl = c.String(maxLength: 100),
                        AnswerCount = c.Int(nullable: false),
                        QuestionCount = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupSurveys", t => t.GroupSurveyId, cascadeDelete: true)
                .Index(t => t.GroupSurveyId);
            
            CreateTable(
                "dbo.SurveyQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyEntityId = c.Int(nullable: false),
                        SurveyOrder = c.Int(nullable: false),
                        Question = c.String(nullable: false, maxLength: 1000),
                        QuestionType = c.Int(nullable: false),
                        required = c.Boolean(nullable: false),
                        QuestionImageUrl = c.String(maxLength: 100),
                        MinType = c.Int(nullable: false),
                        MaxType = c.Int(nullable: false),
                        StringType = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyEntities", t => t.SurveyEntityId, cascadeDelete: true)
                .Index(t => t.SurveyEntityId);
            
            CreateTable(
                "dbo.SurveyAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyUserAnswerId = c.Int(nullable: false),
                        SurveyEntityId = c.Int(nullable: false),
                        SurveyQuestionId = c.Int(nullable: false),
                        Result = c.String(nullable: false, maxLength: 1000),
                        SurveyQuestionOptionId = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyQuestions", t => t.SurveyQuestionId, cascadeDelete: true)
                .ForeignKey("dbo.SurveyUserAnswers", t => t.SurveyUserAnswerId, cascadeDelete: true)
                .Index(t => t.SurveyUserAnswerId)
                .Index(t => t.SurveyQuestionId);
            
            CreateTable(
                "dbo.SurveyUserAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        OldYear = c.Byte(nullable: false),
                        Province = c.Int(nullable: false),
                        UserImageUrl = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Pass = c.String(nullable: false, maxLength: 20),
                        Mobile = c.String(maxLength: 11),
                        OldYear = c.Byte(nullable: false),
                        Province = c.Int(nullable: false),
                        UserImageUrl = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyQuestionOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionOption = c.String(nullable: false, maxLength: 1000),
                        SurveyQuestionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyQuestions", t => t.SurveyQuestionId, cascadeDelete: true)
                .Index(t => t.SurveyQuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyQuestionOptions", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SurveyQuestions", "SurveyEntityId", "dbo.SurveyEntities");
            DropForeignKey("dbo.SurveyUserAnswers", "UserId", "dbo.Users");
            DropForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers");
            DropForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SurveyEntities", "GroupSurveyId", "dbo.GroupSurveys");
            DropIndex("dbo.SurveyQuestionOptions", new[] { "SurveyQuestionId" });
            DropIndex("dbo.SurveyUserAnswers", new[] { "UserId" });
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyQuestionId" });
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyUserAnswerId" });
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyEntityId" });
            DropIndex("dbo.SurveyEntities", new[] { "GroupSurveyId" });
            DropTable("dbo.SurveyQuestionOptions");
            DropTable("dbo.Users");
            DropTable("dbo.SurveyUserAnswers");
            DropTable("dbo.SurveyAnswers");
            DropTable("dbo.SurveyQuestions");
            DropTable("dbo.SurveyEntities");
            DropTable("dbo.GroupSurveys");
        }
    }
}
