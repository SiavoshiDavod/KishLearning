using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Biz
{
    public class ResturantBiz : RepositoryBase<Models.Resturant>
    {
        public static readonly ResturantBiz Instance = new ResturantBiz();

        public List<SelectListItem> CheckListType()
        {
            using (var ctx = new Context())
                return ctx.CheckListType.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }

        public List<SelectListItem> ResturantType()
        {
            using (var ctx = new Context())
                return ctx.ResturantType.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }


        public JqGrid.PagedList<Resturant> GetAllPagedListByCartable(GridSettings grid, int cartable)
        {
            using (var ctx = new Context())
                return ctx.Resturant.Where(x => cartable == 0 || x.CartableId == cartable).Include(x => x.Cartable).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public async Task<Resturant> FindByUserId(int userId, string include = null)
        {
            using (var ctx = new Context())
            {
                var res = ctx.Resturant.Include(x=>x.Cartable).Where(x => x.UserId == userId);
                if (!string.IsNullOrEmpty(include))
                {
                    res = res.Include(include);
                }
                return await res.FirstOrDefaultAsync();
            }
        }

        private int GetFirstStepofCartable(Context ctx)
        {
            int? cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.Resturant).Select(x => x.Id).FirstOrDefault();
            if (cartable == null)
            {
                throw new System.Exception("مرحله اول کارتابل تعریف نشده است");
            }
            return cartable.Value;
        }
        public override async Task<Resturant> Save(Resturant model, bool changeDate = true)
        {
            using (var ctx = new Context())
            {

                var cartable = GetFirstStepofCartable(ctx);
                if (model.Id == 0)
                {

                    if (ctx.Resturant.Any(x => x.UserId == model.UserId))
                    {
                        throw new System.Exception("شما قبلا ثبت نام کرده اید");
                    }
                    if (!string.IsNullOrEmpty(model.Code) && ctx.Resturant.Any(x => x.Code == model.Code))
                    {
                        throw new System.Exception("این کد مرکز قبلا ثبت شده است");
                    }
                   
                    model.CartableId = cartable;
                }
                else
                {
                    if (model.CartableId!=cartable)
                    {
                        throw new System.Exception(" در حال حاضر امکان ویرایش اطلاعات وجود ندارد");
                    }
                }
                
            }

            return await base.Save(model, changeDate);
        }

        public async Task<bool> SaveResturantCheckList(ResturantCheckList model)
        {
            using (var ctx = new Context())
            {
                model.CreatedDate = DateTime.Now;
                ctx.ResturantCheckList.Add(model);
                await ctx.SaveChangesAsync();
                return true;//await context.Resturant.Where(x=>x.Id==model.ResturantId).Include(x => x.ResturantCheckList).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> SaveResturantPersonel(ResturantPersonel model)
        {
            using (var ctx = new Context())
            {
                if (model.Id>0)
                {
                    model.CreatedDate = DateTime.Now;
                    ctx.ResturantPersonel.Add(model);
                }
                else
                {
                    //var cartable = GetFirstStepofCartable(ctx);
                    //if (model.CartableId != cartable)
                    //{
                    //    throw new System.Exception(" در حال حاضر امکان ویرایش اطلاعات وجود ندارد");
                    //}
                }
              
                await ctx.SaveChangesAsync();
                return true;//await context.Resturant.Where(x => x.Id == model.ResturantId).Include(x=>x.ResturantPersonel).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> RemoveResturantCheckList(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantCheckList.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                var cartable = GetFirstStepofCartable(ctx);
                if (res.Resturant.CartableId != cartable)
                {
                    throw new System.Exception(" در حال حاضر امکان ویرایش اطلاعات وجود ندارد");
                }
                ctx.ResturantCheckList.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RemoveResturantPersonel(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantPersonel.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                var cartable = GetFirstStepofCartable(ctx);
                if (res.Resturant.CartableId != cartable)
                {
                    throw new System.Exception(" در حال حاضر امکان ویرایش اطلاعات وجود ندارد");
                }
                ctx.ResturantPersonel.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<ResturantPersonel> FindResturantPersonel(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonel.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<ResturantCheckList> FindResturantCheckList(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantCheckList.FirstOrDefaultAsync(x => x.Id == id);
            }
        }
    }
}