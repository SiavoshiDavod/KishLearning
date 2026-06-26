namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _BoardDirectors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardDirectors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        family = c.String(nullable: false),
                        meli = c.String(nullable: false, maxLength: 10),
                        tel = c.String(),
                        mobile = c.String(nullable: false),
                        address = c.String(),
                        education = c.String(nullable: false),
                        status = c.Boolean(nullable: false),
                        code = c.String(nullable: false),
                        image = c.String(),
                        email = c.String(),
                        Resume = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BoardDirectors");
        }
    }
}
