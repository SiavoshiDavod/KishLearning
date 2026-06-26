using System;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using SurveyWeb.JqGrid.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using SurveyWeb.Models.Resturan;

namespace SurveyWeb.Biz
{
    public class ShekayatBiz : RepositoryBase<Shekayat>
    {
        public static readonly ShekayatBiz Instance = new ShekayatBiz();

        public JqGrid.PagedList<Shekayat> GetAllPagedListByCartable(GridSettings grid, int cartable)
        {
            using (var ctx = new Context())
                return ctx.Shekayat.Where(x => cartable == 0 || x.CartableId == cartable).Include(x => x.Cartable).Include(x => x.Resturant).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<Shekayat> Save(Shekayat model, bool changeDate = true)
        {
            int? cartable = null;
            if (model.Id == 0)
            {
                using (var ctx = new Context())
                    cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.Shekayat).Select(x => x.Id).FirstOrDefault();
                if (cartable == null)
                {
                    throw new HandledException("مرحله اول کارتابل تعریف نشده است");
                }
                model.CartableId = cartable.Value;
            }

            return await base.Save(model, changeDate);
        }
    }
}