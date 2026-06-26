using SurveyWeb.Models.Resturan;
using SurveyWeb.Models.TicketNotice;
using System.Data.Entity;

namespace SurveyWeb.Models
{
    public class Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public Context() : base("name=Context")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }
        //public DbSet<TicketNotice.Ticket> Ticket { get; set; }

        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<EmailSms> EmailSms { get; set; }

        public DbSet<BoardDirector> BoardDirector { get; set; }
        public DbSet<Resturan.ResturantPayment> ResturantPayment { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<Shekayat> Shekayat { get; set; }
        public DbSet<StarRating> StarRating { get; set; }
        public DbSet<Advertising> Advertising { get; set; }
        public DbSet<AdvertisingAttachement> AdvertisingAttachement { get; set; }
        public DbSet<ResturantMenu> ResturantMenu { get; set; }
        public DbSet<ResturantDetailMenu> ResturantDetailMenu { get; set; }
        public DbSet<ResturantType> ResturantType { get; set; }
        public DbSet<CheckListType> CheckListType { get; set; }
        public DbSet<Resturant> Resturant { get; set; }
        public DbSet<ResturantPersonel> ResturantPersonel { get; set; }

        public DbSet<ResturantPersonelJob> ResturantPersonelJob { get; set; }
        public DbSet<ResturantPersonelCourse> ResturantPersonelCourse { get; set; }
        public DbSet<ResturantPersonelLanguage> ResturantPersonelLanguage { get; set; }
        public DbSet<ResturantPersonelEducation> ResturantPersonelEducation { get; set; }

        public DbSet<ResturantCheckList> ResturantCheckList { get; set; }
        public DbSet<CheckListTypeCartable> CheckListTypeCartable { get; set; }



        public DbSet<SiteSetting.SiteSetting> SiteSetting { get; set; }

        public DbSet<UserComment> UserComment { get; set; }
        public DbSet<OrgIntro> OrgIntro { get; set; }
        public DbSet<CompanyLogoAndLink> CompanyLogoAndLink { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsGroup> NewsGroup { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuSub> MenuSub { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SurveyGroupQuestion> SurveyGroupQuestion { get; set; }
        public DbSet<GroupSurvey> GroupSurvey { get; set; }
        public DbSet<SurveyEntity> SurveyEntity { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public DbSet<SurveyQuestionOption> SurveyQuestionOption { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswer { get; set; }
        public DbSet<SurveyUserAnswer> SurveyUserAnswer { get; set; }
        public DbSet<Suggestion> Suggestion { get; set; }
        public DbSet<Idea> Idea { get; set; }

        public DbSet<Cartable> Cartable { get; set; }
        public DbSet<CartableRelation> CartableRelation { get; set; }
        public DbSet<CartableLog> CartableLog { get; set; }
        public DbSet<CartableUserAccess> CartableUserAccess { get; set; }
        public DbSet<Faq> Faq { get; set; }

        public DbSet<NewsSubscription> NewsSubscription { get; set; }
        public DbSet<Complaint> Complaint { get; set; }
        // public DbSet<Tender> Tender { get; set; }
        public DbSet<Regulation> Regulation { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<Security.Role> Role { get; set; }
        public DbSet<Security.RolePermission> RolePermission { get; set; }
        public DbSet<Security.RoleUser> RoleUser { get; set; }
        public DbSet<Security.SurveyPrivateGroup> SurveyPrivateGroup { get; set; }
        public DbSet<Security.SurveyPrivateGroupUser> SurveyPrivateGroupUser { get; set; }
        //CheckList
        public DbSet<CheckList.CheckListGroup> CheckListGroups { get; set; }
        public DbSet<CheckList.CheckList> CheckLists { get; set; }
        public DbSet<CheckList.CheckListItem> CheckListItems { get; set; }
        public DbSet<CheckList.ComplaintCheckList> ComplaintCheckLists { get; set; }
        public DbSet<CheckList.ComplaintCheckListItem> ComplaintCheckListItems { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartableRelation>()
            .HasRequired<Cartable>(s => s.FromCartable)
            .WithMany(g => g.From)
            .HasForeignKey<int>(s => s.From);

            modelBuilder.Entity<CartableRelation>()
            .HasRequired<Cartable>(s => s.ToCartable)
            .WithMany(g => g.To)
            .HasForeignKey<int>(s => s.To);

            
        }

        public DbSet<BaseInfo.Education> Educations { get; set; }
        public DbSet<BaseInfo.City> Citys { get; set; }
        public DbSet<BaseInfo.CompanyType> CompanyTypes { get; set; }
        public DbSet<JobBoard.JobCategory> JobCategories { get; set; }
        public DbSet<JobBoard.JobPosition> JobPositions { get; set; }
        public DbSet<JobBoard.JobRequest> JobRequests { get; set; }
        public DbSet<JobBoard.EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<JobBoard.WorkExperience> WorkExperiences { get; set; }
        public DbSet<JobBoard.EducationalBackground> EducationalBackgrounds { get; set; }

    }
}
