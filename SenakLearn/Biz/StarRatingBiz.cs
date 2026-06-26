using System;
using System.Linq;
using SenakLearn.Models;

namespace SenakLearn.Biz
{
    public class StarRatingBiz : RepositoryBase<StarRating>
    {
        public static readonly StarRatingBiz Instance = new StarRatingBiz();

        public Tuple<double, int> GetRateByTypeAndId(PageType type, int id)
        {
            using (var ctx = new SWEntities())
            {
                var query = ctx.StarRating.Where(x => x.TypeId == id && x.PageTypeId == type);
                return new Tuple<double, int>(query.Average(x =>(double?) x.Rate)??0, query.Count());
            }
        }
        public override bool Save(StarRating model, bool changeDate = true)
        {
            using (var ctx = new SWEntities())
            {
                //check user id and ip
                bool query = ctx.StarRating.Any(x => x.Ip == model.Ip || (model.UserId != null && model.UserId == x.UserId));
                if (query)
                {
                    return false;
                }
                return base.Save(model, changeDate);
            }


        }
    }
}