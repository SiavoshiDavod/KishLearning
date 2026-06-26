namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_Resturant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resturants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ResturantType = c.Byte(nullable: false),
                        Beneficiary = c.String(maxLength: 100),
                        Manager = c.String(maxLength: 100),
                        SalonManager = c.String(maxLength: 100),
                        MasterChef = c.String(maxLength: 100),
                        Staff1 = c.String(maxLength: 100),
                        Staff2 = c.String(maxLength: 100),
                        Staff3 = c.String(maxLength: 100),
                        Owner = c.String(maxLength: 100),
                        RegistrationNumber = c.String(maxLength: 100),
                        ContractDate = c.DateTime(nullable: false),
                        ContractExpireDate = c.DateTime(nullable: false),
                        ContractType = c.String(maxLength: 100),
                        EconomicActivity = c.String(maxLength: 100),
                        Use = c.String(maxLength: 100),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        Address = c.String(maxLength: 500),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CartableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resturants", "UserId", "dbo.Users");
            DropForeignKey("dbo.Resturants", "CartableId", "dbo.Cartables");
            DropIndex("dbo.Resturants", new[] { "CartableId" });
            DropIndex("dbo.Resturants", new[] { "UserId" });
            DropTable("dbo.Resturants");
        }
    }
}
