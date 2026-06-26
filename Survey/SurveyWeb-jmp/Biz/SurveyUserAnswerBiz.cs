using System.Linq;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.wrapper;
using SurveyWeb.JqGrid.Common;

namespace SurveyWeb.Biz
{
    public class SurveyUserAnswerBiz : RepositoryBase<Models.SurveyUserAnswer>
    {
        public static readonly SurveyUserAnswerBiz Instance = new SurveyUserAnswerBiz();
        public PagedList<SurveyUserAnswerVM> GetAllPagedListVm(GridSettings grid)
        {
            using (var ctx = new Models.Context())
                return (from u in ctx.SurveyUserAnswer
                        join e in ctx.SurveyEntity on u.SurveyEntityId equals e.Id
                        select new SurveyUserAnswerVM()
                        {

                            Id = u.Id,
                            Ip = u.Ip,
                            SurveyEntity = e.Name
                        }).FilterAndSortJqGrid<SurveyUserAnswerVM>(grid).ToPagedList<SurveyUserAnswerVM>(grid);
        }
    }
}