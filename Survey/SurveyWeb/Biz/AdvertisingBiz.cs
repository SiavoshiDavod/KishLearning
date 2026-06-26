using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class AdvertisingBiz : RepositoryBase<Models.Advertising>
    {
        public static readonly AdvertisingBiz Instance = new AdvertisingBiz();
        public virtual JqGrid.PagedList<Advertising> GetAllPagedList(GridSettings grid, bool Archive)
        {
            using (var ctx = new Models.Context())
                return ctx.Advertising.Where(x => x.Archive == Archive).Include(x=>x.Resturant).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public List<Advertising> GetAllInView(bool isFav = true, bool isMusical = false, int skip= 0,int take=5)
        {
            using (var ctx = new Models.Context())
                return ctx.Advertising.Where(x => !x.Archive && (isFav == false || x.Resturant.IsFavorite) && (isMusical == false || x.Resturant.IsMusical)).Include(x => x.Resturant.ResturantType).ToList();
        }

        internal async Task<Resturant> FindByUserIdIncludeAdvertising(int userId)
        {
            using (var ctx = new Models.Context())
            {
                return await ctx.Resturant.Include("Advertising.AdvertisingAttachements").Include("ResturantMenu.ResturantDetailMenus").FirstOrDefaultAsync(x => x.UserId == userId);
            }
        }

        public void Accept(int userId)
        {
            using (var ctx = new Models.Context())
            {
                var res = ctx.Advertising.FirstOrDefault(x => x.Id == userId);
                if (res == null)
                {
                    throw new HandledException("اطلاعات وارد شده معتبر نیست");
                }
                if (res.Archive)
                {
                    res.Archive = false;
                    ctx.SaveChanges();
                }
            }
        }

        internal async Task<Advertising> FindByResturantId(int ResturantId)
        {
            using (var ctx = new Models.Context())
                return await ctx.Advertising.FirstOrDefaultAsync(x => x.ResturantId == ResturantId);
        }
        internal async Task<Advertising> FindByUserId(int userId)
        {
            using (var ctx = new Models.Context())
                return await ctx.Advertising.FirstOrDefaultAsync(x => x.Resturant.UserId == userId);
        }
       
        internal async Task<bool> SaveAdvertisingAttachement(AdvertisingAttachement model)
        {
            using (var ctx = new Context())
            {
                model.CreatedDate = DateTime.Now;
                ctx.AdvertisingAttachement.Add(model);
                await ctx.SaveChangesAsync();
                return true;//await context.Resturant.Where(x=>x.Id==model.ResturantId).Include(x => x.ResturantCheckList).FirstOrDefaultAsync();
            }
        }

        internal async Task<string> RemoveAdvertisingAttachement(int id, bool isAdmin)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.AdvertisingAttachement.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return null;
                }
                if (!isAdmin)
                {
                    if (!res.Advertising.Archive)
                    {
                        throw new HandledException(" در حال حاضر امکان حذف اطلاعات وجود ندارد");
                    }
                }
                var ret = res.ImageUrl;
                ctx.AdvertisingAttachement.Remove(res);
                await ctx.SaveChangesAsync();
                return ret;
            }
        }

        internal async Task<AdvertisingAttachement> FindAdvertisingAttachement(int value)
        {
            using (var ctx = new Context())
                return await ctx.AdvertisingAttachement.FirstOrDefaultAsync(x => x.Id == value);
        }
    }
}