namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _rename_commnet_to_comment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserCommnets", newName: "UserComments");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserComments", newName: "UserCommnets");
        }
    }
}
