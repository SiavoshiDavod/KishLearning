using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class NewsBiz : RepositoryBase<Models.News>
    {
        public static readonly NewsBiz Instance = new NewsBiz();
        public List<Models.News> GetActiveMenu(int take = 5, int? currentId = null)
        {
            using (var db = new SWEntities())
            {
                return db.News.Where(x => currentId == null || currentId != x.Id).Include(x => x.Author).Include(x => x.NewsGroup).Take(take).OrderByDescending(x => x.Id).ToList();
            }
        }
        public void AddVisitCount(int id)
        {
            using (var ctx = new SWEntities())
                 ctx.Database.ExecuteSqlCommand($"update dbo.News set VisitCount=VisitCount+1 where Id={id}");
        }
    }
    public class NewsGroupBiz : RepositoryBase<Models.NewsGroup>
    {
        public static readonly NewsGroupBiz Instance = new NewsGroupBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.NewsGroup.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title }).ToList();
        }
    }
    public class AuthorBiz : RepositoryBase<Models.Author>
    {
        public static readonly AuthorBiz Instance = new AuthorBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.Authors.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name + " " + i.Family }).ToList();
        }
    }
}