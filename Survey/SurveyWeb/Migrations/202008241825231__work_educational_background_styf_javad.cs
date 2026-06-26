namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _work_educational_background_styf_javad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EducationalBackgrounds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        FromDate = c.String(),
                        ToDate = c.String(),
                        InstituteName = c.String(),
                        Field = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkExperiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        FromDate = c.String(),
                        ToDate = c.String(),
                        CompanyName = c.String(),
                        Position = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmployeeProfiles", "ProfileImageURI", c => c.String());
            AddColumn("dbo.EmployeeProfiles", "ResumeFile", c => c.Binary());
            DropColumn("dbo.EmployeeProfiles", "WorkExperiences");
            DropColumn("dbo.EmployeeProfiles", "EducationalBackground");
            DropColumn("dbo.JobRequests", "Resume");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobRequests", "Resume", c => c.Binary());
            AddColumn("dbo.EmployeeProfiles", "EducationalBackground", c => c.String());
            AddColumn("dbo.EmployeeProfiles", "WorkExperiences", c => c.String());
            DropColumn("dbo.EmployeeProfiles", "ResumeFile");
            DropColumn("dbo.EmployeeProfiles", "ProfileImageURI");
            DropTable("dbo.WorkExperiences");
            DropTable("dbo.EducationalBackgrounds");
        }
    }
}
