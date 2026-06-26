namespace SurveyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BirthDay = c.DateTime(),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        ImageUrl = c.String(maxLength: 100),
                        Description = c.String(maxLength: 2000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cartables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        CartableType = c.Int(nullable: false),
                        IsFirstState = c.Boolean(nullable: false),
                        IsLastState = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CartableRelations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.To, cascadeDelete: false)
                .ForeignKey("dbo.Cartables", t => t.From, cascadeDelete: false)
                .Index(t => t.From)
                .Index(t => t.To);
            
            CreateTable(
                "dbo.CartableUserAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CartableId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Pass = c.String(nullable: false, maxLength: 20),
                        Mobile = c.String(nullable: false, maxLength: 11),
                        OldYear = c.Byte(nullable: false),
                        Province = c.Int(nullable: false),
                        Education = c.String(maxLength: 30),
                        Job = c.String(maxLength: 30),
                        IsMarried = c.Boolean(nullable: false),
                        UserImageUrl = c.String(maxLength: 100),
                        Archive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CartableLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1000),
                        EntityId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        CartableType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.From, cascadeDelete: false)
                .ForeignKey("dbo.Cartables", t => t.To, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.From)
                .Index(t => t.To);
            
            CreateTable(
                "dbo.CheckListTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyLogoAndLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        ImageUrl = c.String(maxLength: 200),
                        Link = c.String(maxLength: 900),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Birthday = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 3000),
                        Attachment = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        UnitOrJob = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.ContactUs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 50),
                        Tel = c.String(maxLength: 20),
                        Ip = c.String(maxLength: 20),
                        Title = c.String(),
                        Description = c.String(maxLength: 3000),
                        CartableId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupSurveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsShowInSinglePage = c.Boolean(nullable: false),
                        GroupSurveyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        SurveyImageUrl = c.String(maxLength: 100),
                        AnswerCount = c.Int(nullable: false),
                        QuestionCount = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        IsIpRestriction = c.Boolean(nullable: false),
                        IsUserMustBeLogin = c.Boolean(nullable: false),
                        IsFavorite = c.Boolean(nullable: false),
                        IsImportant = c.Boolean(nullable: false),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 2000),
                        IsPrivate = c.Boolean(nullable: false),
                        SurveyPrivateGroupId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupSurveys", t => t.GroupSurveyId, cascadeDelete: true)
                .ForeignKey("dbo.SurveyPrivateGroups", t => t.SurveyPrivateGroupId)
                .Index(t => t.GroupSurveyId)
                .Index(t => t.SurveyPrivateGroupId);
            
            CreateTable(
                "dbo.SurveyGroupQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyEntityId = c.Int(nullable: false),
                        SurveyGroupQuestionTitle = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyEntities", t => t.SurveyEntityId, cascadeDelete: true)
                .Index(t => t.SurveyEntityId);
            
            CreateTable(
                "dbo.SurveyQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyGroupQuestionId = c.Int(),
                        SurveyEntityId = c.Int(nullable: false),
                        SurveyOrder = c.Int(nullable: false),
                        Question = c.String(nullable: false, maxLength: 1000),
                        QuestionType = c.Int(nullable: false),
                        required = c.Boolean(nullable: false),
                        QuestionImageUrl = c.String(maxLength: 100),
                        Width = c.Short(nullable: false),
                        Height = c.Short(nullable: false),
                        MinType = c.Int(nullable: false),
                        MaxType = c.Int(nullable: false),
                        StringType = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyEntities", t => t.SurveyEntityId, cascadeDelete: true)
                .ForeignKey("dbo.SurveyGroupQuestions", t => t.SurveyGroupQuestionId)
                .Index(t => t.SurveyGroupQuestionId)
                .Index(t => t.SurveyEntityId);
            
            CreateTable(
                "dbo.SurveyAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyUserAnswerId = c.Int(nullable: false),
                        SurveyQuestionId = c.Int(nullable: false),
                        Result = c.String(nullable: false, maxLength: 1000),
                        SurveyQuestionOptionId = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyQuestions", t => t.SurveyQuestionId, cascadeDelete: true)
                .ForeignKey("dbo.SurveyUserAnswers", t => t.SurveyUserAnswerId, cascadeDelete: true)
                .Index(t => t.SurveyUserAnswerId)
                .Index(t => t.SurveyQuestionId);
            
            CreateTable(
                "dbo.SurveyUserAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Ip = c.String(maxLength: 20),
                        SurveyEntityId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SurveyQuestionOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionOption = c.String(nullable: false, maxLength: 1000),
                        QuestionOptionUrl = c.String(maxLength: 100),
                        Width = c.Short(nullable: false),
                        Height = c.Short(nullable: false),
                        SurveyQuestionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyQuestions", t => t.SurveyQuestionId, cascadeDelete: true)
                .Index(t => t.SurveyQuestionId);
            
            CreateTable(
                "dbo.SurveyPrivateGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Problem = c.String(maxLength: 3000),
                        Proposal = c.String(maxLength: 3000),
                        Description = c.String(maxLength: 3000),
                        Cost = c.String(maxLength: 3000),
                        Benefit = c.String(maxLength: 3000),
                        Experience = c.String(maxLength: 3000),
                        Attachment1 = c.String(maxLength: 100),
                        Attachment2 = c.String(maxLength: 100),
                        Attachment3 = c.String(maxLength: 100),
                        Attachment4 = c.String(maxLength: 100),
                        Attachment5 = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        UnitOrJob = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuSubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        MenuId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Url = c.String(maxLength: 1000),
                        Image = c.String(maxLength: 100),
                        Content = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        NewsGroupId = c.Int(nullable: false),
                        Summary = c.String(nullable: false, maxLength: 1000),
                        Keyword = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        VisitCount = c.Int(nullable: false),
                        AuthorId = c.Int(),
                        ImageUrl = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId)
                .ForeignKey("dbo.NewsGroups", t => t.NewsGroupId, cascadeDelete: true)
                .Index(t => t.NewsGroupId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.NewsGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsSubscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrgIntroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        ImageUrl = c.String(maxLength: 200),
                        Summery = c.String(nullable: false, maxLength: 2000),
                        Description = c.String(nullable: false),
                        IsImageDirectionLeft = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regulations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        File = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resturants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        BuildYear = c.Int(nullable: false),
                        Degree = c.Int(nullable: false),
                        MeterGround = c.Int(nullable: false),
                        MeterKitchen = c.Int(nullable: false),
                        MeterSaloon = c.Int(nullable: false),
                        Tel = c.String(maxLength: 20),
                        Address = c.String(maxLength: 500),
                        LastDateExtendedLicense = c.DateTime(),
                        Email = c.String(maxLength: 50),
                        WebSite = c.String(maxLength: 50),
                        Beneficiary = c.String(nullable: false, maxLength: 100),
                        BeneficiaryImageUrl = c.String(maxLength: 100),
                        BeneficiaryFatherName = c.String(maxLength: 100),
                        BeneficiaryBirthday = c.DateTime(),
                        BeneficiaryCodeNumber = c.Int(nullable: false),
                        BeneficiaryNationalCode = c.String(maxLength: 10),
                        BeneficiaryEducation = c.String(maxLength: 100),
                        BeneficiaryLastHistory = c.String(maxLength: 100),
                        BeneficiaryTel = c.String(nullable: false, maxLength: 100),
                        Manager = c.String(nullable: false, maxLength: 100),
                        ManagerImageUrl = c.String(maxLength: 100),
                        ManagerEducation = c.String(maxLength: 100),
                        ManagerBirthday = c.DateTime(),
                        ManagerLastHistory = c.String(maxLength: 100),
                        ManagerLearningCourse = c.String(),
                        ManagerTel = c.String(nullable: false, maxLength: 100),
                        ManagerReagent = c.String(maxLength: 50),
                        ManagerDesc = c.String(maxLength: 500),
                        PersonelCountAll = c.Byte(nullable: false),
                        PersonelCountLearned = c.Byte(nullable: false),
                        PersonelCountEnglishTalking = c.Byte(nullable: false),
                        PersonelCountTwoYear = c.Byte(nullable: false),
                        PersonelCountAccepted = c.Byte(nullable: false),
                        MenuTwoLanguage = c.Boolean(nullable: false),
                        MenuBaby = c.Boolean(nullable: false),
                        MenuRejim = c.Boolean(nullable: false),
                        babyseat = c.Boolean(nullable: false),
                        HasBreakfast = c.Boolean(nullable: false),
                        HasLunch = c.Boolean(nullable: false),
                        HasDinner = c.Boolean(nullable: false),
                        HasWC = c.Boolean(nullable: false),
                        CapacitySeatCount = c.Int(nullable: false),
                        ResturantServiceType = c.String(maxLength: 200),
                        HasFoodStorage = c.Boolean(nullable: false),
                        HasFreezerUnderZero = c.Boolean(nullable: false),
                        HasFreezerMoreThanZero = c.Boolean(nullable: false),
                        HasMasterChefRoom = c.Boolean(nullable: false),
                        HasButcher = c.Boolean(nullable: false),
                        HasMechanicalDishwasher = c.Boolean(nullable: false),
                        ResturantTypeId = c.Int(nullable: false),
                        SalonManager = c.String(maxLength: 100),
                        MasterChef = c.String(maxLength: 100),
                        Owner = c.String(maxLength: 100),
                        RegistrationNumber = c.String(maxLength: 100),
                        ContractDate = c.DateTime(),
                        ContractExpireDate = c.DateTime(),
                        ContractType = c.String(maxLength: 100),
                        EconomicActivity = c.String(maxLength: 100),
                        Use = c.String(maxLength: 100),
                        Code = c.String(maxLength: 10),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        UserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .ForeignKey("dbo.ResturantTypes", t => t.ResturantTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ResturantTypeId)
                .Index(t => t.CartableId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResturantCheckLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantId = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 100),
                        CheckListId = c.Int(nullable: false),
                        Name = c.String(),
                        IssueDate = c.DateTime(),
                        ExpireDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckListTypes", t => t.CheckListId, cascadeDelete: true)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.ResturantId)
                .Index(t => t.CheckListId);
            
            CreateTable(
                "dbo.ResturantPersonels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResturantId = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 100),
                        Name = c.String(maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        FatherName = c.String(maxLength: 100),
                        Birthday = c.DateTime(),
                        BirthdayLocation = c.String(maxLength: 20),
                        CodeNumber = c.String(maxLength: 10),
                        NationalCode = c.String(maxLength: 10),
                        PassportNumber = c.String(maxLength: 10),
                        Nationality = c.String(maxLength: 10),
                        IsMan = c.Boolean(nullable: false),
                        IsMarried = c.Boolean(nullable: false),
                        Address = c.String(maxLength: 500),
                        CityStay = c.String(maxLength: 20),
                        AddressStay = c.String(maxLength: 500),
                        PostalCode = c.String(maxLength: 10),
                        Tel = c.String(nullable: false, maxLength: 20),
                        Mobile = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        EducationField = c.String(maxLength: 100),
                        EducationLocation = c.String(maxLength: 100),
                        EducationAddressStay = c.String(maxLength: 200),
                        JobPosition = c.String(maxLength: 100),
                        LastJobPosition = c.String(maxLength: 100),
                        LastJobName = c.String(maxLength: 100),
                        LastStartDate = c.String(maxLength: 10),
                        LastEndDate = c.String(maxLength: 10),
                        CourseName = c.String(maxLength: 100),
                        CourseDuration = c.String(maxLength: 10),
                        CourseLocation = c.String(maxLength: 100),
                        LanguageName = c.String(maxLength: 100),
                        LanguageReading = c.String(maxLength: 10),
                        LanguageWriting = c.String(maxLength: 10),
                        LanguageSpeaking = c.String(maxLength: 10),
                        OtherCountry = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resturants", t => t.ResturantId, cascadeDelete: true)
                .Index(t => t.ResturantId);
            
            CreateTable(
                "dbo.ResturantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDownTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Permisstion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SiteSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contract = c.String(),
                        Name = c.String(maxLength: 200),
                        NameFa = c.String(maxLength: 200),
                        Description = c.String(maxLength: 160),
                        Keywords = c.String(maxLength: 200),
                        SiteUrl = c.String(maxLength: 200),
                        EmailServer = c.String(maxLength: 200),
                        EmailUser = c.String(maxLength: 200),
                        EmailInfo = c.String(maxLength: 200),
                        EmailPass = c.String(maxLength: 200),
                        FaraPayamakUser = c.String(maxLength: 200),
                        FaraPayamakPass = c.String(maxLength: 200),
                        FaraPayamakNumber = c.String(maxLength: 200),
                        AdobeServerUrl = c.String(maxLength: 200),
                        AdobeAdminUser = c.String(maxLength: 200),
                        AdobeAdminPass = c.String(maxLength: 200),
                        DefaultVideoUrl = c.String(maxLength: 200),
                        LogoUrl = c.String(maxLength: 200),
                        JwtTokenSecretKey = c.String(maxLength: 200),
                        ZarinPalMerchantID = c.String(maxLength: 200),
                        isPecPayment = c.Boolean(nullable: false),
                        GoogleSecretKey = c.String(maxLength: 200),
                        GoogleSiteKey = c.String(maxLength: 200),
                        Address = c.String(maxLength: 200),
                        PostalCode = c.String(maxLength: 200),
                        Mobile = c.String(maxLength: 200),
                        tel = c.String(maxLength: 200),
                        Telegram = c.String(maxLength: 200),
                        Instagram = c.String(maxLength: 200),
                        Twitter = c.String(maxLength: 200),
                        WhatsUp = c.String(maxLength: 200),
                        Skype = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Suggestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 3000),
                        Proposal = c.String(maxLength: 3000),
                        Description = c.String(maxLength: 3000),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Benefit = c.String(maxLength: 3000),
                        Experience = c.String(maxLength: 3000),
                        Attachment1 = c.String(maxLength: 100),
                        Attachment2 = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 20),
                        Family = c.String(nullable: false, maxLength: 20),
                        Education = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 11),
                        Tel = c.String(maxLength: 20),
                        UnitOrJob = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        CartableId = c.Int(nullable: false),
                        Ip = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cartables", t => t.CartableId, cascadeDelete: true)
                .Index(t => t.CartableId);
            
            CreateTable(
                "dbo.SurveyPrivateGroupUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyPrivateGroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyPrivateGroups", t => t.SurveyPrivateGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SurveyPrivateGroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Status = c.Boolean(nullable: false),
                        TypeId = c.Int(),
                        PageTypeId = c.Int(nullable: false),
                        Title = c.String(),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(),
                        ParentId = c.Int(),
                        Description = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserComments", "UserId", "dbo.Users");
            DropForeignKey("dbo.SurveyPrivateGroupUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.SurveyPrivateGroupUsers", "SurveyPrivateGroupId", "dbo.SurveyPrivateGroups");
            DropForeignKey("dbo.Suggestions", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.RoleUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Resturants", "UserId", "dbo.Users");
            DropForeignKey("dbo.Resturants", "ResturantTypeId", "dbo.ResturantTypes");
            DropForeignKey("dbo.ResturantPersonels", "ResturantId", "dbo.Resturants");
            DropForeignKey("dbo.ResturantCheckLists", "ResturantId", "dbo.Resturants");
            DropForeignKey("dbo.ResturantCheckLists", "CheckListId", "dbo.CheckListTypes");
            DropForeignKey("dbo.Resturants", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.News", "NewsGroupId", "dbo.NewsGroups");
            DropForeignKey("dbo.News", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.MenuSubs", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.Ideas", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.SurveyEntities", "SurveyPrivateGroupId", "dbo.SurveyPrivateGroups");
            DropForeignKey("dbo.SurveyQuestionOptions", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SurveyQuestions", "SurveyGroupQuestionId", "dbo.SurveyGroupQuestions");
            DropForeignKey("dbo.SurveyQuestions", "SurveyEntityId", "dbo.SurveyEntities");
            DropForeignKey("dbo.SurveyUserAnswers", "UserId", "dbo.Users");
            DropForeignKey("dbo.SurveyAnswers", "SurveyUserAnswerId", "dbo.SurveyUserAnswers");
            DropForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SurveyGroupQuestions", "SurveyEntityId", "dbo.SurveyEntities");
            DropForeignKey("dbo.SurveyEntities", "GroupSurveyId", "dbo.GroupSurveys");
            DropForeignKey("dbo.ContactUs", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.Complaints", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.CartableLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.CartableLogs", "To", "dbo.Cartables");
            DropForeignKey("dbo.CartableLogs", "From", "dbo.Cartables");
            DropForeignKey("dbo.CartableUserAccesses", "UserId", "dbo.Users");
            DropForeignKey("dbo.CartableUserAccesses", "CartableId", "dbo.Cartables");
            DropForeignKey("dbo.CartableRelations", "From", "dbo.Cartables");
            DropForeignKey("dbo.CartableRelations", "To", "dbo.Cartables");
            DropIndex("dbo.UserComments", new[] { "UserId" });
            DropIndex("dbo.SurveyPrivateGroupUsers", new[] { "UserId" });
            DropIndex("dbo.SurveyPrivateGroupUsers", new[] { "SurveyPrivateGroupId" });
            DropIndex("dbo.Suggestions", new[] { "CartableId" });
            DropIndex("dbo.RoleUsers", new[] { "UserId" });
            DropIndex("dbo.RoleUsers", new[] { "RoleId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleId" });
            DropIndex("dbo.ResturantPersonels", new[] { "ResturantId" });
            DropIndex("dbo.ResturantCheckLists", new[] { "CheckListId" });
            DropIndex("dbo.ResturantCheckLists", new[] { "ResturantId" });
            DropIndex("dbo.Resturants", new[] { "UserId" });
            DropIndex("dbo.Resturants", new[] { "CartableId" });
            DropIndex("dbo.Resturants", new[] { "ResturantTypeId" });
            DropIndex("dbo.News", new[] { "AuthorId" });
            DropIndex("dbo.News", new[] { "NewsGroupId" });
            DropIndex("dbo.MenuSubs", new[] { "MenuId" });
            DropIndex("dbo.Ideas", new[] { "CartableId" });
            DropIndex("dbo.SurveyQuestionOptions", new[] { "SurveyQuestionId" });
            DropIndex("dbo.SurveyUserAnswers", new[] { "UserId" });
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyQuestionId" });
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyUserAnswerId" });
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyEntityId" });
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyGroupQuestionId" });
            DropIndex("dbo.SurveyGroupQuestions", new[] { "SurveyEntityId" });
            DropIndex("dbo.SurveyEntities", new[] { "SurveyPrivateGroupId" });
            DropIndex("dbo.SurveyEntities", new[] { "GroupSurveyId" });
            DropIndex("dbo.ContactUs", new[] { "CartableId" });
            DropIndex("dbo.Complaints", new[] { "CartableId" });
            DropIndex("dbo.CartableLogs", new[] { "To" });
            DropIndex("dbo.CartableLogs", new[] { "From" });
            DropIndex("dbo.CartableLogs", new[] { "UserId" });
            DropIndex("dbo.CartableUserAccesses", new[] { "CartableId" });
            DropIndex("dbo.CartableUserAccesses", new[] { "UserId" });
            DropIndex("dbo.CartableRelations", new[] { "To" });
            DropIndex("dbo.CartableRelations", new[] { "From" });
            DropTable("dbo.UserComments");
            DropTable("dbo.SurveyPrivateGroupUsers");
            DropTable("dbo.Suggestions");
            DropTable("dbo.SiteSettings");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Roles");
            DropTable("dbo.ResturantTypes");
            DropTable("dbo.ResturantPersonels");
            DropTable("dbo.ResturantCheckLists");
            DropTable("dbo.Resturants");
            DropTable("dbo.Regulations");
            DropTable("dbo.OrgIntroes");
            DropTable("dbo.NewsSubscriptions");
            DropTable("dbo.NewsGroups");
            DropTable("dbo.News");
            DropTable("dbo.MenuSubs");
            DropTable("dbo.Menus");
            DropTable("dbo.Ideas");
            DropTable("dbo.SurveyPrivateGroups");
            DropTable("dbo.SurveyQuestionOptions");
            DropTable("dbo.SurveyUserAnswers");
            DropTable("dbo.SurveyAnswers");
            DropTable("dbo.SurveyQuestions");
            DropTable("dbo.SurveyGroupQuestions");
            DropTable("dbo.SurveyEntities");
            DropTable("dbo.GroupSurveys");
            DropTable("dbo.Faqs");
            DropTable("dbo.ContactUs");
            DropTable("dbo.Complaints");
            DropTable("dbo.CompanyLogoAndLinks");
            DropTable("dbo.CheckListTypes");
            DropTable("dbo.CartableLogs");
            DropTable("dbo.Users");
            DropTable("dbo.CartableUserAccesses");
            DropTable("dbo.CartableRelations");
            DropTable("dbo.Cartables");
            DropTable("dbo.Authors");
        }
    }
}
