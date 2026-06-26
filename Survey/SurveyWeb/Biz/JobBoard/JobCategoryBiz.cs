using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using SurveyWeb.Models.JobBoard;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class JobCategoryBiz : RepositoryBase<Models.JobBoard.JobCategory>
    {
        public static readonly JobCategoryBiz Instance = new JobCategoryBiz();

        public virtual JobCategory Add(JobCategory model)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.JobCategories.FirstOrDefault(x => x.Title == model.Title);
                if (item != null)
                    throw new Exception("عنوان دسته بندی تکراری است.");
                ctx.JobCategories.Add(model);
                ctx.SaveChanges();
                return model;
            }
        }

        public virtual JobCategory Find(int id)
        {
            using (var ctx = new Models.Context())
                return ctx.JobCategories.FirstOrDefault(x => x.Id == id);
        }

        public virtual List<JobCategory> FindAll()
        {
            using (var ctx = new Models.Context())
                return ctx.JobCategories.ToList();
        }

        public virtual void Remove(int id)
        {
            using (var ctx = new Models.Context())
            {
                if (ctx.JobPositions.Any(x => x.JobCategoryID == id))
                    throw new Exception("در این دسته بندی موقعیت شغلی تعریف شده است.");
                ctx.JobCategories.Remove(ctx.JobCategories.FirstOrDefault(x => x.Id == id));
                ctx.SaveChanges();
            }
        }

        public virtual JobCategory Update(JobCategory model)
        {
            using (var ctx = new Models.Context())
            {
               var item = ctx.JobCategories.FirstOrDefault(x => x.Id == model.Id);
                item.Title = model.Title;
                ctx.SaveChanges();
                return model;
            }
        }

        public virtual JqGrid.PagedList<JobCategory> GetAllPagedList(GridSettings grid, bool Archive)
        {
            using (var ctx = new Models.Context())
                return ctx.JobCategories.FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
    }
}