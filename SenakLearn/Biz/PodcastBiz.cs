using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace SenakLearn.Biz
{
    public class PodcastBiz
    {
        public static readonly PodcastBiz Instance = new PodcastBiz();
        public JqGrid.PagedList<learn_cours> GetAllPagedList(GridSettings grid)
        {
            using (SWEntities db = new SWEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                return db.learn_cours.Include(c => c.learn_teacher).Include(c => c.learn_cours_group).Where(w => w.TypeCours == 2).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
    }
}