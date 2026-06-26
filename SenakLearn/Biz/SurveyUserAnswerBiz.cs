using System.Linq;
using SenakLearn.JqGrid;
using SenakLearn.Models.wrapper;
using SenakLearn.JqGrid.Common;

namespace SenakLearn.Biz
{
    public class SurveyUserAnswerBiz : RepositoryBaseSurvey<Models.SurveyUserAnswer>
    {
        public static readonly SurveyUserAnswerBiz Instance = new SurveyUserAnswerBiz();
        public PagedList<SurveyUserAnswerVM> GetAllPagedListVm(GridSettings grid)
        {
            using (var ctx = new SWEntities())
                return (from u in ctx.SurveyUserAnswer
                        join e in ctx.SurveyEntity on u.SurveyEntityId equals e.Id
                        join lu in ctx.learn_user on u.UserId equals lu.id into uuu
                        from user in uuu.DefaultIfEmpty()
                        select new SurveyUserAnswerVM()
                        {

                            Id = u.Id,
                            Ip = u.Ip,
                            SurveyEntity = e.Name,
                            User = user.Name + " " + user.Family,
                            UserName = user.user_name
                        }).FilterAndSortJqGrid<SurveyUserAnswerVM>(grid).ToPagedList<SurveyUserAnswerVM>(grid);
        }
    }
}