namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_AdminDescription_in_ResturantPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResturantPayments", "AdminDescription", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResturantPayments", "AdminDescription");
        }
    }
}
