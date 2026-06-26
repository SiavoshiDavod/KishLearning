using MVC.Controls;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Biz
{
    public abstract class RepositoryBaseParentChild<T> : RepositoryBase<T> where T : ParentChildEntity
    {
        public async override Task<T> Save(T model, bool changeDate = true)
        {
            context = MyConstructor();
            model.Validate();
            if (model.Id == 0)
            {
                model.CreatedDate = DateTime.Now;
                context.Set<T>().Add(model);
            }
            else
            {
                var foundEntity = context.ChangeTracker.Entries<T>()?.Where(x => x?.Entity?.Id == model.Id)?.FirstOrDefault();
                //context.Entry(foundEntity).CurrentValues.SetValues(model);
                if (null != foundEntity)
                {
                    model.CreatedDate = foundEntity.Entity.CreatedDate;
                    foundEntity.State = EntityState.Detached;
                }
                if (model.ParentId != null && model.ParentId == model.Id)
                {
                    model.ParentId = null;
                }
                model.UpdateDate = DateTime.Now;
                context.Entry(model).State = EntityState.Modified;
            }

           await context.SaveChangesAsync();
            return model;
        }
        //public async Task<List<SelectListItem>> DropDown()
        //{
        //    return await context.Set<T>().Where(x => x.ParentId == null).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToListAsync();
        //}
    }
    public abstract class RepositoryBase<T> where T : BaseEntity
    {
        protected  Models.Context context;

        protected RepositoryBase()
        {
            //context = new Models.Context();
            //context.Configuration.ProxyCreationEnabled = false;
            //context.Configuration.LazyLoadingEnabled = false;
        }
        protected Models.Context MyConstructor()
        {
           var mycontext = new Models.Context();
            mycontext.Configuration.ProxyCreationEnabled = false;
            mycontext.Configuration.LazyLoadingEnabled = false;
            return mycontext;
        }

        //public virtual List<SelectListItem> DropDown()
        //{
        //    return context.Set<T>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        //}

        public async virtual Task<T> Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            context = MyConstructor();
            T result = await context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async virtual Task<T> Save(T model, bool changeDate = true)
        {
            model.Validate();
            context = MyConstructor();
            if (model.Id == 0)
            {
                if (changeDate)
                    model.CreatedDate = DateTime.Now;
                else
                    model.CreatedDate = new DateTime(model.CreatedDate.Ticks, DateTime.Now.Kind);

                if (model.UpdateDate.HasValue)
                    model.UpdateDate = new DateTime(model.UpdateDate.Value.Ticks, DateTime.Now.Kind);
                context.Set<T>().Add(model);
            }
            else
            {
                var foundEntity = context.ChangeTracker.Entries<T>()?.FirstOrDefault(x => x?.Entity?.Id == model.Id);
                //context.Entry(foundEntity).CurrentValues.SetValues(model);
                if (null != foundEntity)
                {
                    if (changeDate)
                        model.CreatedDate = foundEntity.Entity.CreatedDate;
                    else
                        model.CreatedDate = new DateTime(model.CreatedDate.Ticks, DateTimeKind.Local);
                    if (model.CreatedDate == DateTime.MinValue)
                    {
                        model.CreatedDate = foundEntity.Entity.UpdateDate.Value;
                    }
                    foundEntity.State = EntityState.Detached;
                }
                if (changeDate)
                    model.UpdateDate = DateTime.Now;
                else if (model.UpdateDate.HasValue)
                    model.UpdateDate = new DateTime(model.UpdateDate.Value.Ticks, DateTime.Now.Kind);

                if (model.CreatedDate==DateTime.MinValue)
                {
                    model.CreatedDate = model.UpdateDate.Value;
                }
                context.Entry(model).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
            return model;
        }

        public async virtual Task<bool> AddAll(List<T> model)
        {
            context = MyConstructor();
            context.Set<T>().AddRange(model);
            await context.SaveChangesAsync();
            return true;
        }

        public async virtual Task<List<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> Expr)
        {
            context = MyConstructor();
            return await context.Set<T>().Where(Expr).ToListAsync();
        }
        public async virtual Task<List<T>> GetAll()
        {
            context = MyConstructor();
            return await context.Set<T>().ToListAsync();
        }
        public virtual JqGrid.PagedList<T> GetAllPagedList(GridSettings grid)
        {
            context = MyConstructor();
            return context.Set<T>().FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public async virtual Task<List<T>> GetAllPage(System.Linq.Expressions.Expression<Func<T, bool>> Expr, int skip, int take, string include=null)
        {
            if (skip < 0 || take < 0)
            {
                return null;
            }
            context = MyConstructor();
            var result = context.Set<T>().Where(Expr);
            if (!string.IsNullOrEmpty(include))
            {
                result = result.Include(include);
            }
            return await result.OrderByDescending(x => x.Id).Skip(skip).Take(take).ToListAsync();
        }

        public async virtual Task<int> Remove(int id)
        {
            context = MyConstructor();
            T result = await context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (null == result)
            {
                return 0;
            }
            context.Set<T>().Remove(result);
            await context.SaveChangesAsync();

            return 1;
        }
        public async Task<T> GetInclude(T model, params string[] includes)
        {
            context = MyConstructor();
            IQueryable<T> query = context.Set<T>().Where(x => x.Id == model.Id);
            for (int index = 0; index < (includes?.Length ?? 0); index++)
            {
                query = query.Include(includes[index]);
            }

            T result = await query?.FirstOrDefaultAsync();

            return result;
        }
        public  T GetIncludeSync(T model, params string[] includes)
        {
            context = MyConstructor();
            IQueryable<T> query = context.Set<T>().Where(x => x.Id == model.Id);
            for (int index = 0; index < (includes?.Length ?? 0); index++)
            {
                query = query.Include(includes[index]);
            }

            T result =  query?.FirstOrDefault();

            return result;
        }
        //public T GetInclude(T model, params string[] includes)
        //{
        //    IQueryable<T> query = context.Set<T>().Where(x => x.Id == model.Id);
        //    for (int index = 0; index < (includes?.Length ?? 0); index++)
        //    {
        //        query = query.Include(includes[index]);
        //    }

        //    T result =  query?.FirstOrDefault();

        //    return result;
        //}

        public async Task<int> GetCount(System.Linq.Expressions.Expression<Func<T, bool>> Expr)
        {
            context = MyConstructor();
            return await context.Set<T>().CountAsync(Expr);
        }
    }
}
