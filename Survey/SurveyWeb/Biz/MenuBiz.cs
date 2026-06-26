using SurveyWeb.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;

namespace SurveyWeb.Biz
{
    public class MenuBiz : RepositoryBase<Models.Menu>
    {
        public static readonly MenuBiz Instance = new MenuBiz();
        public List<Models.Menu> GetActiveMenu()
        {
            using (var db = new Context())
            {
                return db.Menu.Where(x => x.Status).Include(x => x.MenuSubs).ToList();
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.Menu.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title  }).ToList();
        }
    }
    public class MenuSubBiz : RepositoryBase<MenuSub>
    {
        public static readonly MenuSubBiz Instance = new MenuSubBiz();
        public  PagedList<MenuSub> GetAllPagedListByMenuId(GridSettings grid,int? menuId)
        {
            using (var ctx = new Context())
                return ctx.MenuSub.Where(x=>menuId==null|| x.MenuId==menuId).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
    }
}