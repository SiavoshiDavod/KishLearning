using System;
using System.Linq;
using System.Threading.Tasks;
using SurveyWeb.Models;

namespace SurveyWeb.Biz
{
    public class NewsSubscriptionBiz : RepositoryBase<Models.NewsSubscription>
    {
        public static readonly NewsSubscriptionBiz Instance = new NewsSubscriptionBiz();
        public override Task<NewsSubscription> Save(NewsSubscription model, bool changeDate = true)
        {
            if (context.NewsSubscription.Any(x=>x.Email==model.Email))
            {
                throw new Exception("شما قبلا مشترک خبرنامه شده اید");
            }
            return base.Save(model, changeDate);
        }
    }
}