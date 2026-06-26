namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_UserComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Status = c.Boolean(nullable: false),
                        TypeId = c.Int(),
                        PageTypeId = c.Int(nullable: false),
                        Title = c.String(),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(),
                        ParentId = c.Int(),
                        Description = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserComments", "UserId", "dbo.Users");
            DropIndex("dbo.UserComments", new[] { "UserId" });
            DropTable("dbo.UserComments");
        }
    }
}
