namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_update_javad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        AboutMe = c.String(maxLength: 200),
                        Email = c.String(),
                        Phone = c.String(),
                        BirthYear = c.String(),
                        Gender = c.Int(nullable: false),
                        MilitaryStatus = c.Int(nullable: false),
                        MaritalStatus = c.Int(nullable: false),
                        ProvinceOfResidence = c.String(),
                        Address = c.String(),
                        Specialty = c.String(),
                        Skills = c.String(),
                        WorkExperiences = c.String(),
                        Languages = c.String(),
                        EducationalBackground = c.String(),
                        IsVerified = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.JobPositions", "IsVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobPositions", "IsVerified");
            DropTable("dbo.EmployeeProfiles");
        }
    }
}
