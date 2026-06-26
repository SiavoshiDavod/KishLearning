using System.Threading.Tasks;
using SurveyWeb.Models;
using System.Linq;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using System.Data.Entity;

namespace SurveyWeb.Biz
{
    public class IdeaBiz : RepositoryBase<Models.Idea>
    {
        public static readonly IdeaBiz Instance = new IdeaBiz();
        public JqGrid.PagedList<Idea> GetAllPagedListByCartable(GridSettings grid, int cartable)
        {
            using (var ctx = new Context())
                return ctx.Idea.Where(x => cartable == 0 || x.CartableId == cartable).Include(x => x.Cartable).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<Idea> Save(Idea model, bool changeDate = true)
        {
            int? cartable = null;
            if (model.Id == 0)
            {
                using (var ctx = new Context())
                    cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.Idea).Select(x => x.Id).FirstOrDefault();
                if (cartable == null)
                {
                    throw new HandledException("مرحله اول کارتابل تعریف نشده است");
                }
                model.CartableId = cartable.Value;
            }

            return await base.Save(model, changeDate);
        }
    }
    public class SuggestionBiz : RepositoryBase<Models.Suggestion>
    {
        public static readonly SuggestionBiz Instance = new SuggestionBiz();
        public JqGrid.PagedList<Suggestion> GetAllPagedListByCartable(GridSettings grid, int cartable)
        {
            using (var ctx = new Context())
                return ctx.Suggestion.Where(x => cartable == 0 || x.CartableId == cartable).Include(x=>x.Cartable).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<Suggestion> Save(Suggestion model, bool changeDate = true)
        {
            int? cartable = null;
            if (model.Id == 0)
            {
                using (var ctx = new Context())
                    cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.Suggestion).Select(x => x.Id).FirstOrDefault();
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