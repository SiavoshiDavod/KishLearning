namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_OrgIntro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrgIntroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        ImageUrl = c.String(maxLength: 200),
                        Summery = c.String(nullable: false, maxLength: 2000),
                        Description = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Authors", "BirthDay", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "BirthDay", c => c.DateTime(nullable: false));
            DropTable("dbo.OrgIntroes");
        }
    }
}
