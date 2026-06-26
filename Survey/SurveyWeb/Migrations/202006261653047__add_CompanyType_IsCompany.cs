namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _add_CompanyType_IsCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Resturants", "IsCompany", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "CompanyShenaseMelli", c => c.String(maxLength: 11));
            AddColumn("dbo.Resturants", "CompanyName", c => c.String(maxLength: 50));
            AddColumn("dbo.Resturants", "CompanyShomareSabt", c => c.String(maxLength: 20));
            AddColumn("dbo.Resturants", "CompanyCodeEghtesadi", c => c.String(maxLength: 20));
            AddColumn("dbo.Resturants", "CompanyTel", c => c.String(maxLength: 20));
            AddColumn("dbo.Resturants", "CompanyMobile", c => c.String(maxLength: 20));
            AddColumn("dbo.Resturants", "CompanyFax", c => c.String(maxLength: 20));
            AddColumn("dbo.Resturants", "CompanyEmail", c => c.String(maxLength: 20));
            AddColumn("dbo.Resturants", "CompanyCityId", c => c.Int());
            AddColumn("dbo.Resturants", "CompanyTypeId", c => c.Int());
            CreateIndex("dbo.Resturants", "CompanyCityId");
            CreateIndex("dbo.Resturants", "CompanyTypeId");
            AddForeignKey("dbo.Resturants", "CompanyCityId", "dbo.Cities", "Id");
            AddForeignKey("dbo.Resturants", "CompanyTypeId", "dbo.CompanyTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resturants", "CompanyTypeId", "dbo.CompanyTypes");
            DropForeignKey("dbo.Resturants", "CompanyCityId", "dbo.Cities");
            DropIndex("dbo.Resturants", new[] { "CompanyTypeId" });
            DropIndex("dbo.Resturants", new[] { "CompanyCityId" });
            DropColumn("dbo.Resturants", "CompanyTypeId");
            DropColumn("dbo.Resturants", "CompanyCityId");
            DropColumn("dbo.Resturants", "CompanyEmail");
            DropColumn("dbo.Resturants", "CompanyFax");
            DropColumn("dbo.Resturants", "CompanyMobile");
            DropColumn("dbo.Resturants", "CompanyTel");
            DropColumn("dbo.Resturants", "CompanyCodeEghtesadi");
            DropColumn("dbo.Resturants", "CompanyShomareSabt");
            DropColumn("dbo.Resturants", "CompanyName");
            DropColumn("dbo.Resturants", "CompanyShenaseMelli");
            DropColumn("dbo.Resturants", "IsCompany");
            DropTable("dbo.CompanyTypes");
        }
    }
}
