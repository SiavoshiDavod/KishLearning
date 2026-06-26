using SurveyWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class NewsBiz : RepositoryBase<Models.News>
    {
        public static readonly NewsBiz Instance = new NewsBiz();
        public List<Models.News> GetActiveMenu(int take=5,int? currentId=null)
        {
            using (var db = new Context())
            {
                return db.News.Where(x=>currentId==null ||currentId!=x.Id).Include(x => x.Author).Include(x=>x.NewsGroup).Take(take).OrderByDescending(x=>x.Id).ToList();
            }
        }
        public async Task AddVisitCount(int id)
        {
            using (var ctx = new Context())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.News set VisitCount=VisitCount+1 where Id={id}");
        }
    }
    public class NewsGroupBiz : RepositoryBase<Models.NewsGroup>
    {
        public static readonly NewsGroupBiz Instance = new NewsGroupBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.NewsGroup.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title }).ToList();
        }
    }
    public class AuthorBiz : RepositoryBase<Models.Author>
    {
        public static readonly AuthorBiz Instance = new AuthorBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.Author.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name+" "+i.Family }).ToList();
        }
    }
}