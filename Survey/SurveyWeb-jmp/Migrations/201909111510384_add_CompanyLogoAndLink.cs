namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_CompanyLogoAndLink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyLogoAndLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        ImageUrl = c.String(maxLength: 200),
                        Link = c.String(maxLength: 900),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompanyLogoAndLinks");
        }
    }
}
