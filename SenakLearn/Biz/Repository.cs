using MVC.Controls;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public abstract class RepositoryBaseParentChildSurvey<T> : RepositoryBaseSurvey<T> where T : ParentChildEntity
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
    public abstract class RepositoryBaseSurvey<T> where T : BaseEntity
    {
        protected SWEntities context;
        protected RepositoryBaseSurvey()
        {
            //context = new Models.Context();
            //context.Configuration.ProxyCreationEnabled = false;
            //context.Configuration.LazyLoadingEnabled = false;
        }

        protected SWEntities MyConstructor()
        {
            var mycontext = new SWEntities();
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

                if (model.CreatedDate == DateTime.MinValue)
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
        public async virtual Task<List<T>> GetAllPage(System.Linq.Expressions.Expression<Func<T, bool>> Expr, int skip, int take)
        {
            if (skip < 0 || take < 0)
            {
                return null;
            }
            context = MyConstructor();
            var result = await context.Set<T>().Where(Expr).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToListAsync();

            return result;
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
        public T GetIncludeSync(T model, params string[] includes)
        {
            context = MyConstructor();
            IQueryable<T> query = context.Set<T>().Where(x => x.Id == model.Id);
            for (int index = 0; index < (includes?.Length ?? 0); index++)
            {
                query = query.Include(includes[index]);
            }

            T result = query?.FirstOrDefault();

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
            return await context.Set<T>().CountAsync(Expr);
        }
    }
    public abstract class RepositoryBaseParentChild<T> : RepositoryBase<T> where T : ParentChildEntity
    {
        public override bool Save(T model, bool changeDate = true)
        {
            //var context =  MyConstructor();
            model.Validate();
            using (var context = new SWEntities())
            {
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

                context.SaveChanges();
                return true;
            }
        }
        public List<SelectListItem> DropDown(int? selected = null)
        {
            using (var context = new SWEntities())
                return context.Set<T>().Where(x => x.ParentId == null).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Description,
                    Selected = i.Id == selected ? true : false }).ToList();
        }
    }
    public abstract class RepositoryBase<T> where T : Models.BaseEntity
    {
        //protected SWEntities context;

        //protected RepositoryBase()
        //{
        //    context = new SWEntities();
        //    context.Configuration.ProxyCreationEnabled = false;
        //    context.Configuration.LazyLoadingEnabled = false;
        //}
        //protected SWEntities MyConstructor()
        //{
        //   var context = new SWEntities();
        //    context.Configuration.ProxyCreationEnabled = false;
        //    context.Configuration.LazyLoadingEnabled = false;
        //    return context;
        //}
        //public virtual List<SelectListItem> DropDown()
        //{
        //    return context.Set<T>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        //}

        public virtual T Get(int id)
        {
            // var context =  MyConstructor();
            if (id == 0)
            {
                return null;
            }
            using (var context = new SWEntities())
                return context.Set<T>().Where(x => x.Id == id).FirstOrDefault();

            //return result;
        }

        public virtual bool Save(T model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                model.Validate();
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
                    var foundEntity = context.ChangeTracker.Entries<T>()?.Where(x => x?.Entity?.Id == model.Id)?.FirstOrDefault();
                    //context.Entry(foundEntity).CurrentValues.SetValues(model);
                    if (null != foundEntity)
                    {
                        if (changeDate)
                            model.CreatedDate = foundEntity.Entity.CreatedDate;
                        else
                            model.CreatedDate = new DateTime(model.CreatedDate.Ticks, DateTimeKind.Local);
                        foundEntity.State = EntityState.Detached;
                    }
                    else
                    {
                        model.CreatedDate = DateTime.Now;
                    }
                    if (changeDate)
                        model.UpdateDate = DateTime.Now;
                    else if (model.UpdateDate.HasValue)
                        model.UpdateDate = new DateTime(model.UpdateDate.Value.Ticks, DateTime.Now.Kind);
                    context.Entry(model).State = EntityState.Modified;
                }

                context.SaveChanges();
                return true;
            }
        }

        public virtual async Task<bool> SaveAsync(T model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                model.Validate();
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
                    var foundEntity = context.ChangeTracker.Entries<T>()?.Where(x => x?.Entity?.Id == model.Id)?.FirstOrDefault();
                    //context.Entry(foundEntity).CurrentValues.SetValues(model);
                    if (null != foundEntity)
                    {
                        if (changeDate)
                            model.CreatedDate = foundEntity.Entity.CreatedDate;
                        else
                            model.CreatedDate = new DateTime(model.CreatedDate.Ticks, DateTimeKind.Local);
                        foundEntity.State = EntityState.Detached;
                    }
                    else
                    {
                        model.CreatedDate = DateTime.Now;
                    }
                    if (changeDate)
                        model.UpdateDate = DateTime.Now;
                    else if (model.UpdateDate.HasValue)
                        model.UpdateDate = new DateTime(model.UpdateDate.Value.Ticks, DateTime.Now.Kind);
                    context.Entry(model).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
                return true;
            }
        }
        public virtual bool AddAll(List<T> model)
        {
            using (var context = new SWEntities())
            {
                context.Set<T>().AddRange(model);
                context.SaveChanges();
                return true;
            }
        }

        public virtual IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> Expr)
        {
            using (var context = new SWEntities())
                return context.Set<T>().Where(Expr).ToList();
        }
        public virtual IEnumerable<T> GetAll()
        {
            using (var context = new SWEntities())
                return context.Set<T>().ToList();
        }
        public virtual JqGrid.PagedList<T> GetAllPagedList(GridSettings grid)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                return context.Set<T>().FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
        public virtual IEnumerable<T> GetAllPage(System.Linq.Expressions.Expression<Func<T, bool>> Expr, int skip, int take)
        {

            if (skip < 0 || take < 0)
            {
                return null;
            }
            using (var context = new SWEntities())
                return context.Set<T>().Where(Expr).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();

        }

        public virtual int Remove(int id)
        {
            using (var context = new SWEntities())
            {
                T result = context.Set<T>().Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    return 0;
                }
                context.Set<T>().Remove(result);
                context.SaveChanges();

                return 1;
            }
        }
        public T GetInclude(T model, params string[] includes)
        {
            using (var context = new SWEntities())
            {
                IQueryable<T> query = context.Set<T>().Where(x => x.Id == model.Id);
                for (int index = 0; index < (includes?.Length ?? 0); index++)
                {
                    query = query.Include(includes[index]);
                }

                T result = query?.FirstOrDefault();

                return result;
            }
        }

        public int GetCount(System.Linq.Expressions.Expression<Func<T, bool>> Expr)
        {
            using (var context = new SWEntities())
                return context.Set<T>().Count(Expr);
        }
        //public int Count
        //{
        //    get
        //    {
        //        return context.Set<T>().Count();
        //    }
        //}
    }
}
