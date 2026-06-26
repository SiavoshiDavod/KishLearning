namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_resturantModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckListTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResturantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Resturants", "BuildYear", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "Degree", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "MeterGround", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "MeterKitchen", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "MeterSaloon", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "LastDateExtendedLicense", c => c.DateTime());
            AddColumn("dbo.Resturants", "Email", c => c.String(maxLength: 50));
            AddColumn("dbo.Resturants", "WebSite", c => c.String(maxLength: 50));
            AddColumn("dbo.Resturants", "BeneficiaryFatherName", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "BeneficiaryBirthday", c => c.DateTime());
            AddColumn("dbo.Resturants", "BeneficiaryCodeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "BeneficiaryNationalCode", c => c.String(maxLength: 10));
            AddColumn("dbo.Resturants", "BeneficiaryEducation", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "BeneficiaryLastHistory", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "BeneficiaryTel", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Resturants", "ManagerEducation", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "ManagerBirthday", c => c.DateTime());
            AddColumn("dbo.Resturants", "ManagerLastHistory", c => c.String(maxLength: 100));
            AddColumn("dbo.Resturants", "ManagerLearningCourse", c => c.String());
            AddColumn("dbo.Resturants", "ManagerTel", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Resturants", "ManagerReagent", c => c.String(maxLength: 50));
            AddColumn("dbo.Resturants", "ManagerDesc", c => c.String(maxLength: 500));
            AddColumn("dbo.Resturants", "PersonelCountAll", c => c.Byte(nullable: false));
            AddColumn("dbo.Resturants", "PersonelCountLearned", c => c.Byte(nullable: false));
            AddColumn("dbo.Resturants", "PersonelCountEnglishTalking", c => c.Byte(nullable: false));
            AddColumn("dbo.Resturants", "PersonelCountTwoYear", c => c.Byte(nullable: false));
            AddColumn("dbo.Resturants", "PersonelCountAccepted", c => c.Byte(nullable: false));
            AddColumn("dbo.Resturants", "MenuTwoLanguage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "MenuBaby", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "MenuRejim", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "babyseat", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasBreakfast", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasLunch", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasDinner", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasWC", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "CapacitySeatCount", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "ResturantServiceType", c => c.String(maxLength: 200));
            AddColumn("dbo.Resturants", "HasFoodStorage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasFreezerUnderZero", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasFreezerMoreThanZero", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasMasterChefRoom", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasButcher", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "HasMechanicalDishwasher", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resturants", "ResturantTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Resturants", "Code", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantCheckLists", "CheckListId", c => c.Int(nullable: false));
            AddColumn("dbo.ResturantCheckLists", "Name", c => c.String());
            AddColumn("dbo.ResturantCheckLists", "IssueDate", c => c.DateTime());
            AddColumn("dbo.ResturantCheckLists", "ExpireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ResturantPersonels", "LastName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "FatherName", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "Birthday", c => c.DateTime());
            AddColumn("dbo.ResturantPersonels", "BirthdayLocation", c => c.String(maxLength: 20));
            AddColumn("dbo.ResturantPersonels", "CodeNumber", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "NationalCode", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "PassportNumber", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "Nationality", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "IsMan", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResturantPersonels", "IsMarried", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResturantPersonels", "Address", c => c.String(maxLength: 500));
            AddColumn("dbo.ResturantPersonels", "CityStay", c => c.String(maxLength: 20));
            AddColumn("dbo.ResturantPersonels", "AddressStay", c => c.String(maxLength: 500));
            AddColumn("dbo.ResturantPersonels", "PostalCode", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "Tel", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.ResturantPersonels", "Mobile", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.ResturantPersonels", "Education", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "EducationField", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "EducationLocation", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "EducationAddressStay", c => c.String(maxLength: 200));
            AddColumn("dbo.ResturantPersonels", "JobPosition", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "LastJobPosition", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "LastJobName", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "LastStartDate", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "LastEndDate", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "CourseName", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "CourseDuration", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "CourseLocation", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "LanguageName", c => c.String(maxLength: 100));
            AddColumn("dbo.ResturantPersonels", "LanguageReading", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "LanguageWriting", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "LanguageSpeaking", c => c.String(maxLength: 10));
            AddColumn("dbo.ResturantPersonels", "OtherCountry", c => c.String(maxLength: 100));
            AlterColumn("dbo.Resturants", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Resturants", "Beneficiary", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Resturants", "Manager", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Resturants", "ContractDate", c => c.DateTime());
            AlterColumn("dbo.Resturants", "ContractExpireDate", c => c.DateTime());
            CreateIndex("dbo.Resturants", "ResturantTypeId");
            CreateIndex("dbo.ResturantCheckLists", "CheckListId");
            AddForeignKey("dbo.ResturantCheckLists", "CheckListId", "dbo.CheckListTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Resturants", "ResturantTypeId", "dbo.ResturantTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Resturants", "ResturantType");
            DropColumn("dbo.Resturants", "Mobile");
            DropColumn("dbo.ResturantCheckLists", "CheckList");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResturantCheckLists", "CheckList", c => c.Byte(nullable: false));
            AddColumn("dbo.Resturants", "Mobile", c => c.String(maxLength: 11));
            AddColumn("dbo.Resturants", "ResturantType", c => c.Byte(nullable: false));
            DropForeignKey("dbo.Resturants", "ResturantTypeId", "dbo.ResturantTypes");
            DropForeignKey("dbo.ResturantCheckLists", "CheckListId", "dbo.CheckListTypes");
            DropIndex("dbo.ResturantCheckLists", new[] { "CheckListId" });
            DropIndex("dbo.Resturants", new[] { "ResturantTypeId" });
            AlterColumn("dbo.Resturants", "ContractExpireDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Resturants", "ContractDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Resturants", "Manager", c => c.String(maxLength: 100));
            AlterColumn("dbo.Resturants", "Beneficiary", c => c.String(maxLength: 100));
            AlterColumn("dbo.Resturants", "Name", c => c.String(maxLength: 100));
            DropColumn("dbo.ResturantPersonels", "OtherCountry");
            DropColumn("dbo.ResturantPersonels", "LanguageSpeaking");
            DropColumn("dbo.ResturantPersonels", "LanguageWriting");
            DropColumn("dbo.ResturantPersonels", "LanguageReading");
            DropColumn("dbo.ResturantPersonels", "LanguageName");
            DropColumn("dbo.ResturantPersonels", "CourseLocation");
            DropColumn("dbo.ResturantPersonels", "CourseDuration");
            DropColumn("dbo.ResturantPersonels", "CourseName");
            DropColumn("dbo.ResturantPersonels", "LastEndDate");
            DropColumn("dbo.ResturantPersonels", "LastStartDate");
            DropColumn("dbo.ResturantPersonels", "LastJobName");
            DropColumn("dbo.ResturantPersonels", "LastJobPosition");
            DropColumn("dbo.ResturantPersonels", "JobPosition");
            DropColumn("dbo.ResturantPersonels", "EducationAddressStay");
            DropColumn("dbo.ResturantPersonels", "EducationLocation");
            DropColumn("dbo.ResturantPersonels", "EducationField");
            DropColumn("dbo.ResturantPersonels", "Education");
            DropColumn("dbo.ResturantPersonels", "Mobile");
            DropColumn("dbo.ResturantPersonels", "Tel");
            DropColumn("dbo.ResturantPersonels", "PostalCode");
            DropColumn("dbo.ResturantPersonels", "AddressStay");
            DropColumn("dbo.ResturantPersonels", "CityStay");
            DropColumn("dbo.ResturantPersonels", "Address");
            DropColumn("dbo.ResturantPersonels", "IsMarried");
            DropColumn("dbo.ResturantPersonels", "IsMan");
            DropColumn("dbo.ResturantPersonels", "Nationality");
            DropColumn("dbo.ResturantPersonels", "PassportNumber");
            DropColumn("dbo.ResturantPersonels", "NationalCode");
            DropColumn("dbo.ResturantPersonels", "CodeNumber");
            DropColumn("dbo.ResturantPersonels", "BirthdayLocation");
            DropColumn("dbo.ResturantPersonels", "Birthday");
            DropColumn("dbo.ResturantPersonels", "FatherName");
            DropColumn("dbo.ResturantPersonels", "LastName");
            DropColumn("dbo.ResturantCheckLists", "ExpireDate");
            DropColumn("dbo.ResturantCheckLists", "IssueDate");
            DropColumn("dbo.ResturantCheckLists", "Name");
            DropColumn("dbo.ResturantCheckLists", "CheckListId");
            DropColumn("dbo.Resturants", "Code");
            DropColumn("dbo.Resturants", "ResturantTypeId");
            DropColumn("dbo.Resturants", "HasMechanicalDishwasher");
            DropColumn("dbo.Resturants", "HasButcher");
            DropColumn("dbo.Resturants", "HasMasterChefRoom");
            DropColumn("dbo.Resturants", "HasFreezerMoreThanZero");
            DropColumn("dbo.Resturants", "HasFreezerUnderZero");
            DropColumn("dbo.Resturants", "HasFoodStorage");
            DropColumn("dbo.Resturants", "ResturantServiceType");
            DropColumn("dbo.Resturants", "CapacitySeatCount");
            DropColumn("dbo.Resturants", "HasWC");
            DropColumn("dbo.Resturants", "HasDinner");
            DropColumn("dbo.Resturants", "HasLunch");
            DropColumn("dbo.Resturants", "HasBreakfast");
            DropColumn("dbo.Resturants", "babyseat");
            DropColumn("dbo.Resturants", "MenuRejim");
            DropColumn("dbo.Resturants", "MenuBaby");
            DropColumn("dbo.Resturants", "MenuTwoLanguage");
            DropColumn("dbo.Resturants", "PersonelCountAccepted");
            DropColumn("dbo.Resturants", "PersonelCountTwoYear");
            DropColumn("dbo.Resturants", "PersonelCountEnglishTalking");
            DropColumn("dbo.Resturants", "PersonelCountLearned");
            DropColumn("dbo.Resturants", "PersonelCountAll");
            DropColumn("dbo.Resturants", "ManagerDesc");
            DropColumn("dbo.Resturants", "ManagerReagent");
            DropColumn("dbo.Resturants", "ManagerTel");
            DropColumn("dbo.Resturants", "ManagerLearningCourse");
            DropColumn("dbo.Resturants", "ManagerLastHistory");
            DropColumn("dbo.Resturants", "ManagerBirthday");
            DropColumn("dbo.Resturants", "ManagerEducation");
            DropColumn("dbo.Resturants", "BeneficiaryTel");
            DropColumn("dbo.Resturants", "BeneficiaryLastHistory");
            DropColumn("dbo.Resturants", "BeneficiaryEducation");
            DropColumn("dbo.Resturants", "BeneficiaryNationalCode");
            DropColumn("dbo.Resturants", "BeneficiaryCodeNumber");
            DropColumn("dbo.Resturants", "BeneficiaryBirthday");
            DropColumn("dbo.Resturants", "BeneficiaryFatherName");
            DropColumn("dbo.Resturants", "WebSite");
            DropColumn("dbo.Resturants", "Email");
            DropColumn("dbo.Resturants", "LastDateExtendedLicense");
            DropColumn("dbo.Resturants", "MeterSaloon");
            DropColumn("dbo.Resturants", "MeterKitchen");
            DropColumn("dbo.Resturants", "MeterGround");
            DropColumn("dbo.Resturants", "Degree");
            DropColumn("dbo.Resturants", "BuildYear");
            DropTable("dbo.ResturantTypes");
            DropTable("dbo.CheckListTypes");
        }
    }
}
