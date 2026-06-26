using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class CourseBiz
    {
        public static readonly CourseBiz Instance = new CourseBiz();

        public JqGrid.PagedList<learn_cours> GetAllPagedList(GridSettings grid)
        {
            using (SWEntities db = new SWEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                return db.learn_cours.Include(c => c.learn_cours_group).Include(c => c.learn_teacher).Where(w=>w.TypeCours==null).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }

        public JqGrid.PagedList<learn_cours> GetAllPagedList(GridSettings grid, int userId)
        {
            using (SWEntities db = new SWEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                return db.learn_cours.Include(c => c.learn_cours_group).Include(c => c.learn_teacher).Where(x=>x.learn_teacher.UserId==userId).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }

        public List<learn_cours> FindAll(System.Linq.Expressions.Expression<Func<learn_cours, bool>> Expr, int take = 3)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_cours.Where(Expr).Take(take).ToList();
            }
        }
        public learn_cours GetInclude(learn_cours model, params string[] includes)
        {
            using (SWEntities context = new SWEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<learn_cours> query = context.learn_cours.Where(x => x.id == model.id);
                for (int index = 0; index < (includes?.Length ?? 0); index++)
                {
                    query = query.Include(includes[index]);
                }

                learn_cours result = query?.FirstOrDefault();

                return result;
            }

        }

        public virtual IEnumerable<learn_cours> GetAllPage(System.Linq.Expressions.Expression<Func<learn_cours, bool>> Expr, int skip, int take)
        {
            if (skip < 0 || take < 0)
            {
                return null;
            }
            using (SWEntities db = new SWEntities())
                return db.Set<learn_cours>().Where(Expr).OrderByDescending(x => x.id).Skip(skip).Take(take).ToList();

        }
        public List<learn_cours> FindAll(int take, int skip, int? groupId)
        {
            using (SWEntities db = new SWEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                return db.learn_cours.Where(x => x.status && (groupId == null | x.id_group == groupId)).OrderByDescending(x => x.id).Skip(skip).Take(take).ToList();
            }
        }
        public List<learn_cours_group> FindAllGroup(bool? online = true, bool? offline = true, bool? paper = true, bool? book = true, bool? booklet = true)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_cours_group.Where(x => x.status && (book == null || book == x.Book) && (booklet == null || booklet == x.Booklet) && (online == null || online == x.Online) && (offline == null || offline == x.Offline) && (paper == null || paper == x.Paper)).OrderBy(x => x.Order).ToList();
            }
        }
        public List<SelectListItem> FindAllGroupDropdown(bool? online = true, bool? offline = true, bool? paper = true, bool? book = true, bool? booklet = true)
        {
            using (SWEntities db = new SWEntities())
            {
                var lists= db.learn_cours_group.Where(x => x.status && (book == null || book == x.Book) && (booklet == null || booklet == x.Booklet) && (online == null || online == x.Online) && (offline == null || offline == x.Offline) && (paper == null || paper == x.Paper)).OrderBy(x => x.Order).ToList();
                var drops = lists.Select(a => new SelectListItem { Text = a.name, Value = a.id.ToString() }).ToList();
                return drops;
            }
        }
        public async Task UpdateGroupCount(CoursGroupCountType type,int groupId, bool add = true)
        {
            string operate = add ? "+" : "-";
            using (var ctx = new SWEntities())
            {
                try
                {
                    await ctx.Database.ExecuteSqlCommandAsync($"update [dbo].[learn_cours_group] set {type.ToString()}Count= {type.ToString()}Count{operate}1 ,{type.ToString()}=1 where id={groupId}");
                }
                catch (Exception)
                {
                }

            }
        }

    }
}