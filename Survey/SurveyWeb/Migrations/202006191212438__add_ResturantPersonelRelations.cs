namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_ResturantPersonelRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResturantPersonelCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantPersonelId = c.Int(nullable: false),
                        CourseName1 = c.String(maxLength: 100),
                        CourseDuration1 = c.String(maxLength: 10),
                        CourseLocation1 = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResturantPersonels", t => t.ResturantPersonelId, cascadeDelete: true)
                .Index(t => t.ResturantPersonelId);
            
            CreateTable(
                "dbo.ResturantPersonelEducations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantPersonelId = c.Int(nullable: false),
                        EducationId = c.Int(nullable: false),
                        EducationField = c.String(maxLength: 100),
                        EducationLocation = c.String(maxLength: 100),
                        EducationAddressStay = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Educations", t => t.EducationId, cascadeDelete: true)
                .ForeignKey("dbo.ResturantPersonels", t => t.ResturantPersonelId, cascadeDelete: true)
                .Index(t => t.ResturantPersonelId)
                .Index(t => t.EducationId);
            
            CreateTable(
                "dbo.ResturantPersonelJobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantPersonelId = c.Int(nullable: false),
                        LastJobPosition1 = c.String(maxLength: 100),
                        LastJobName1 = c.String(maxLength: 100),
                        LastStartDate1 = c.String(maxLength: 10),
                        LastEndDate1 = c.String(maxLength: 10),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResturantPersonels", t => t.ResturantPersonelId, cascadeDelete: true)
                .Index(t => t.ResturantPersonelId);
            
            CreateTable(
                "dbo.ResturantPersonelLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantPersonelId = c.Int(nullable: false),
                        LanguageName = c.String(maxLength: 100),
                        LanguageReading = c.String(maxLength: 10),
                        LanguageWriting = c.String(maxLength: 10),
                        LanguageSpeaking = c.String(maxLength: 10),
                        OtherCountry = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResturantPersonels", t => t.ResturantPersonelId, cascadeDelete: true)
                .Index(t => t.ResturantPersonelId);
            
            AddColumn("dbo.ResturantPersonels", "EducationId", c => c.Int());
            CreateIndex("dbo.ResturantPersonels", "EducationId");
            AddForeignKey("dbo.ResturantPersonels", "EducationId", "dbo.Educations", "Id");
            DropColumn("dbo.ResturantPersonels", "Education");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResturantPersonels", "Education", c => c.String(maxLength: 100));
            DropForeignKey("dbo.ResturantPersonelLanguages", "ResturantPersonelId", "dbo.ResturantPersonels");
            DropForeignKey("dbo.ResturantPersonelJobs", "ResturantPersonelId", "dbo.ResturantPersonels");
            DropForeignKey("dbo.ResturantPersonelEducations", "ResturantPersonelId", "dbo.ResturantPersonels");
            DropForeignKey("dbo.ResturantPersonelEducations", "EducationId", "dbo.Educations");
            DropForeignKey("dbo.ResturantPersonelCourses", "ResturantPersonelId", "dbo.ResturantPersonels");
            DropForeignKey("dbo.ResturantPersonels", "EducationId", "dbo.Educations");
            DropIndex("dbo.ResturantPersonelLanguages", new[] { "ResturantPersonelId" });
            DropIndex("dbo.ResturantPersonelJobs", new[] { "ResturantPersonelId" });
            DropIndex("dbo.ResturantPersonelEducations", new[] { "EducationId" });
            DropIndex("dbo.ResturantPersonelEducations", new[] { "ResturantPersonelId" });
            DropIndex("dbo.ResturantPersonelCourses", new[] { "ResturantPersonelId" });
            DropIndex("dbo.ResturantPersonels", new[] { "EducationId" });
            DropColumn("dbo.ResturantPersonels", "EducationId");
            DropTable("dbo.ResturantPersonelLanguages");
            DropTable("dbo.ResturantPersonelJobs");
            DropTable("dbo.ResturantPersonelEducations");
            DropTable("dbo.ResturantPersonelCourses");
        }
    }
}
