namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_CheckListTypeCartable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckListTypeCartables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CheckListId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CartableId = c.Int(nullable: false),
                        ResturantId = c.Int(nullable: false),
                        CartableCheckListType = c.String(nullable: false, maxLength: 100),
                        Accepted = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CheckListTypeCartables");
        }
    }
}
