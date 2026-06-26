namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _change_Education_int : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Resturants", "BeneficiaryEducation", c => c.Int());
            AlterColumn("dbo.Resturants", "ManagerEducation", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Resturants", "ManagerEducation", c => c.String(maxLength: 100));
            AlterColumn("dbo.Resturants", "BeneficiaryEducation", c => c.String(maxLength: 100));
        }
    }
}
