namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _changeresturantPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTypes", "Degree", c => c.Byte(nullable: false));
            AddColumn("dbo.PaymentTypes", "ResturantTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Resturants", "Degree", c => c.Byte(nullable: false));
            AlterColumn("dbo.ResturantPayments", "RefId", c => c.Long(nullable: false));
            CreateIndex("dbo.PaymentTypes", "ResturantTypeId");
            AddForeignKey("dbo.PaymentTypes", "ResturantTypeId", "dbo.ResturantTypes", "Id", cascadeDelete: false);
            DropColumn("dbo.PaymentTypes", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentTypes", "Title", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.PaymentTypes", "ResturantTypeId", "dbo.ResturantTypes");
            DropIndex("dbo.PaymentTypes", new[] { "ResturantTypeId" });
            AlterColumn("dbo.ResturantPayments", "RefId", c => c.Long());
            AlterColumn("dbo.Resturants", "Degree", c => c.Int(nullable: false));
            DropColumn("dbo.PaymentTypes", "ResturantTypeId");
            DropColumn("dbo.PaymentTypes", "Degree");
        }
    }
}
