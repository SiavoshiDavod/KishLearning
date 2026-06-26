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

        public DbSet<ResturantType> ResturantType { get; set; }
        public DbSet<CheckListType> CheckListType { get; set; }
        public DbSet<Resturant> Resturant { get; set; }
        public DbSet<ResturantPersonel> ResturantPersonel { get; set; }
        public DbSet<ResturantCheckList> ResturantCheckList { get; set; }



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

    }
}
