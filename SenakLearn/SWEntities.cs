using SenakLearn.Models;
using SenakLearn.Models.Common;
using SenakLearn.Models.Person;
using SenakLearn.Models.Security;
using System.Data.Entity;

namespace SenakLearn
{
    public class SWEntities : DbContext
    {
        public SWEntities() : base("name=SWEntities")
        {
            Database.SetInitializer<SWEntities>(null);
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<AzmoonGroupQuestion> AzmoonGroupQuestion { get; set; }
        public DbSet<GroupAzmoon> GroupAzmoon { get; set; }
        public DbSet<AzmoonEntity> AzmoonEntity { get; set; }
        public DbSet<AzmoonQuestion> AzmoonQuestion { get; set; }
        public DbSet<AzmoonQuestionOption> AzmoonQuestionOption { get; set; }
        public DbSet<AzmoonAnswer> AzmoonAnswer { get; set; }
        public DbSet<AzmoonUserAnswer> AzmoonUserAnswer { get; set; }
        public DbSet<AzmoonPrivateGroup> AzmoonPrivateGroup { get; set; }
        public DbSet<AzmoonPrivateGroupUser> AzmoonPrivateGroupUser { get; set; }



        public DbSet<SurveyGroupQuestion> SurveyGroupQuestion { get; set; }
        public DbSet<GroupSurvey> GroupSurvey { get; set; }
        public DbSet<SurveyEntity> SurveyEntity { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public DbSet<SurveyQuestionOption> SurveyQuestionOption { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswer { get; set; }
        public DbSet<SurveyUserAnswer> SurveyUserAnswer { get; set; }
        public DbSet<SurveyPrivateGroup> SurveyPrivateGroup { get; set; }
        public DbSet<SurveyPrivateGroupUser> SurveyPrivateGroupUser { get; set; }

        public DbSet<StarRating> StarRating { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }

        public DbSet<SiteSetting.SiteSetting> SiteSetting { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<NewsGroup> NewsGroup { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<CompanyLogoAndLink> CompanyLogoAndLinks { get; set; }
        public DbSet<SiteReviewCount> SiteReviewCount { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupDetail> GroupDetail { get; set; }
        public DbSet<EmailSms> EmailSms { get; set; }
        public DbSet<Paper> Paper { get; set; }
        public DbSet<PaperTranslateQuality> PaperTranslateQuality { get; set; }
        public DbSet<PaperUniversity> PaperUniversity { get; set; }
        public DbSet<PaperJournal> PaperJournal { get; set; }
        public DbSet<PaperPublisher> PaperPublisher { get; set; }
        public DbSet<PaperTrend> PaperTrend { get; set; }
        public DbSet<PaperField> PaperField { get; set; }
        public DbSet<UserCommnet> UserCommnet { get; set; }
        public DbSet<StudentSupport> StudentSupport { get; set; }
        public DbSet<TeacherSupport> TeacherSupport { get; set; }
        public DbSet<OfflineVideo> OfflineVideo { get; set; }
        public DbSet<JoinUs> JoinUs { get; set; }
        public DbSet<OnlineClassAccoration> OnlineClassAccorations { get; set; }
        public DbSet<OnlineClassAccorationDetails> OnlineClassAccorationDetails { get; set; }
        public DbSet<ZarinpalPayment> ZarinpalPayments { get; set; }
        public DbSet<OnlineClass> OnlineClasses { get; set; }
        public DbSet<OnlineClassRequest> OnlineClassRequests { get; set; }
        public DbSet<DynamicForm> DynamicForms { get; set; }
        public DbSet<VideoFile> VideoFiles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<learn_cours_group> learn_cours_group { get; set; }
        public DbSet<learn_teacher> learn_teacher { get; set; }

        public DbSet<learn_user> learn_user { get; set; }
        public DbSet<learn_cours> learn_cours { get; set; }
        public DbSet<ObjCount> ObjCounts { get; set; }
        public DbSet<Organization> Orgs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Person_Course> Person_Courses { get; set; }
        public DbSet<Person_Teacher> Person_Teachers { get; set; }
        public DbSet<Person_Certificate> Person_Certificates { get; set; }
        public DbSet<VW_Person_Certificate> VW_Person_Certificates { get; set; }
        public DbSet<EntityMasterData> EntityMasterDatas { get; set; }
        public DbSet<FactorModel> Factors { get; set; }
        public DbSet<BookSlideModel> BookSlides { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<StarRating>().Property(t => t.PageTypeId).IsRequired();
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}