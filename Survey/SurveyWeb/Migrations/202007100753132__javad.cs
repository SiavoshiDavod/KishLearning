namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _javad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        JobCategoryID = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Companyname = c.String(nullable: false),
                        CooperationType = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        RequiredSkills = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        MilitaryServiceStatus = c.Int(nullable: false),
                        WorkExperience = c.Int(nullable: false),
                        Location = c.String(nullable: false),
                        SalaryFrom = c.String(nullable: false),
                        SalaryTo = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        JobPositionId = c.Int(nullable: false),
                        Resume = c.Binary(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Email", c => c.String(maxLength: 200));
            AddColumn("dbo.CheckListTypes", "IsReq", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CheckListTypes", "IsReq");
            DropColumn("dbo.Users", "Email");
            DropTable("dbo.JobRequests");
            DropTable("dbo.JobPositions");
            DropTable("dbo.JobCategories");
        }
    }
}
