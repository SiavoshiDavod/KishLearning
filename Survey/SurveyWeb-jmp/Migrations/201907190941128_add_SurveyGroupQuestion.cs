namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_SurveyGroupQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SurveyGroupQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyEntityId = c.Int(nullable: false),
                        SurveyGroupQuestionTitle = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyEntities", t => t.SurveyEntityId, cascadeDelete: true)
                .Index(t => t.SurveyEntityId);
            
            AddColumn("dbo.SurveyQuestions", "SurveyGroupQuestionId", c => c.Int());
            CreateIndex("dbo.SurveyQuestions", "SurveyGroupQuestionId");
            AddForeignKey("dbo.SurveyQuestions", "SurveyGroupQuestionId", "dbo.SurveyGroupQuestions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyQuestions", "SurveyGroupQuestionId", "dbo.SurveyGroupQuestions");
            DropForeignKey("dbo.SurveyGroupQuestions", "SurveyEntityId", "dbo.SurveyEntities");
            DropIndex("dbo.SurveyGroupQuestions", new[] { "SurveyEntityId" });
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyGroupQuestionId" });
            DropColumn("dbo.SurveyQuestions", "SurveyGroupQuestionId");
            DropTable("dbo.SurveyGroupQuestions");
        }
    }
}
