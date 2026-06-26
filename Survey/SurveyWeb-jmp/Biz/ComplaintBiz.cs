using System;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using SurveyWeb.JqGrid.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SurveyWeb.Biz
{
    public class ComplaintBiz : RepositoryBase<Models.Complaint>
    {
        public static readonly ComplaintBiz Instance = new ComplaintBiz();

        public JqGrid.PagedList<Complaint> GetAllPagedListByCartable(GridSettings grid, int cartable)
        {
            using (var ctx = new Context())
                return ctx.Complaint.Where(x => cartable == 0 || x.CartableId == cartable).Include(x => x.Cartable).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<Complaint> Save(Complaint model, bool changeDate = true)
        {
            int? cartable = null;
            if (model.Id == 0)
            {
                using (var ctx = new Context())
                    cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.Complaint).Select(x => x.Id).FirstOrDefault();
                if (cartable == null)
                {
                    throw new System.Exception("مرحله اول کارتابل تعریف نشده است");
                }
                model.CartableId = cartable.Value;
            }

            return await base.Save(model, changeDate);
        }
    }
}