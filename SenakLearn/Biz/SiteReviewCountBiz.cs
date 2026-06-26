using SenakLearn.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class SiteReviewCountBiz
    {
        public static readonly SiteReviewCountBiz Instanse = new SiteReviewCountBiz();
        private static int CurrentDate = 0;
        public async Task<List<SiteReviewCount>> GetAllAsync()
        {
            using (var ctx = new SWEntities())
            {
                return await ctx.SiteReviewCount.ToListAsync();
            }
        }
        public async Task<SiteReviewCount> GetAllSiteReviewCountForHomePageAsync()
        {
            using (var ctx = new SWEntities())
            {
                return await ctx.SiteReviewCount.SqlQuery("SELECT [Date]=1,sum(Adobe)as  Adobe,sum(video)as Video,sum(VideoNotFree)as VideoNotFree,sum(Online)as [Online],sum(Course)as Course,sum(Paper)as Paper,sum(Book)as Book,sum(Site)as [Site]  FROM  dbo.SiteReviewCount").FirstOrDefaultAsync();
            }
        }

        public async Task Update(SiteReviewCountType type)
        {
            int date = DateTimeExtensions.DateTimeNow();

            string update = $"update SiteReviewCount set {type.ToString()}= {type.ToString()}+1 where Date={date}";
            using (var ctx = new SWEntities())
            {
                if (date == CurrentDate)
                {
                    await ctx.Database.ExecuteSqlCommandAsync(update);
                }
                else
                {
                    CurrentDate = date;
                    ctx.SiteReviewCount.Add(new SiteReviewCount()
                    {
                        Date = date,
                        Adobe = type == SiteReviewCountType.Adobe ? 1 : 0,
                        Book = type == SiteReviewCountType.Book ? 1 : 0,
                        Course = type == SiteReviewCountType.Course ? 1 : 0,
                        Online = type == SiteReviewCountType.Online ? 1 : 0,
                        Paper = type == SiteReviewCountType.Paper ? 1 : 0,
                        Site = type == SiteReviewCountType.Site ? 1 : 0,
                        Video = type == SiteReviewCountType.Video ? 1 : 0,
                        VideoNotFree = type == SiteReviewCountType.VideoNotFree ? 1 : 0
                    });
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (System.Exception e)
                    {
                        try
                        {
                            await ctx.Database.ExecuteSqlCommandAsync(update);
                        }
                        catch (System.Exception ex)
                        {
                            var m = ex.Message;
                            m += e.Message;
                        }
                    }

                }

            }
        }
    }
}