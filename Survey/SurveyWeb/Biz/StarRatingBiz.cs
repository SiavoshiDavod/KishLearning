using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SurveyWeb.Biz
{
    public class StarRatingBiz : RepositoryBase<StarRating>
    {
        public static readonly StarRatingBiz Instance = new StarRatingBiz();

        public Tuple<double, int> GetRateByTypeAndId(PageType type, int id)
        {
            using (var ctx = new Context())
            {
                var query = ctx.StarRating.Where(x => x.TypeId == id && x.PageTypeId == type);
                return new Tuple<double, int>(query.Average(x => (double?)x.Rate) ?? 0, query.Count());
            }
        }
        public  async Task<bool> SaveRating(StarRating model, bool changeDate = true)
        {
            using (var ctx = new Context())
            {
                //check user id and ip
                bool query = ctx.StarRating.Any(x => x.Ip == model.Ip || (model.UserId != null && model.UserId == x.UserId));
                if (query)
                {
                    return false;
                }
                await base.Save(model, changeDate);
                return true;
            }


        }
    }

}