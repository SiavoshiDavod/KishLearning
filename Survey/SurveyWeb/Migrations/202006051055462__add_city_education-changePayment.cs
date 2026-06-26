namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_city_educationchangePayment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentTypes", "ResturantTypeId", "dbo.ResturantTypes");
            DropIndex("dbo.PaymentTypes", new[] { "ResturantTypeId" });
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                        ProvinceId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PaymentTypes", "Title", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.PaymentTypes", "PaymentTypeEnumId", c => c.Byte(nullable: false));
            AddColumn("dbo.ResturantPayments", "PaymentTypeEnumId", c => c.Byte(nullable: false));
            AlterColumn("dbo.Users", "Province", c => c.Byte(nullable: false));
            AlterColumn("dbo.PaymentTypes", "ResturantTypeId", c => c.Int());
            CreateIndex("dbo.PaymentTypes", "ResturantTypeId");
            AddForeignKey("dbo.PaymentTypes", "ResturantTypeId", "dbo.ResturantTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentTypes", "ResturantTypeId", "dbo.ResturantTypes");
            DropIndex("dbo.PaymentTypes", new[] { "ResturantTypeId" });
            AlterColumn("dbo.PaymentTypes", "ResturantTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Province", c => c.Int(nullable: false));
            DropColumn("dbo.ResturantPayments", "PaymentTypeEnumId");
            DropColumn("dbo.PaymentTypes", "PaymentTypeEnumId");
            DropColumn("dbo.PaymentTypes", "Title");
            DropTable("dbo.Educations");
            DropTable("dbo.Cities");
            CreateIndex("dbo.PaymentTypes", "ResturantTypeId");
            AddForeignKey("dbo.PaymentTypes", "ResturantTypeId", "dbo.ResturantTypes", "Id", cascadeDelete: true);
        }
    }
}
