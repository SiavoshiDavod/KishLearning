namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_PaymentResturant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResturantPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentTypeId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        IsOnlinePayment = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        ResturantId = c.Int(nullable: false),
                        FishPic = c.String(maxLength: 100),
                        PaymentDate = c.DateTime(nullable: false),
                        VarizKonande = c.String(maxLength: 100),
                        RefId = c.Long(),
                        IsAccepted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.PaymentTypeId)
                .Index(t => t.ResturantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResturantPayments", "ResturantId", "dbo.Resturants");
            DropForeignKey("dbo.ResturantPayments", "PaymentTypeId", "dbo.PaymentTypes");
            DropIndex("dbo.ResturantPayments", new[] { "ResturantId" });
            DropIndex("dbo.ResturantPayments", new[] { "PaymentTypeId" });
            DropTable("dbo.ResturantPayments");
        }
    }
}
