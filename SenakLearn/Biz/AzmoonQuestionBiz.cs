using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using SenakLearn.Models.wrapper;
using SenakLearn.Models;

namespace SenakLearn.Biz
{
    public class AzmoonQuestionBiz : RepositoryBaseSurvey<Models.AzmoonQuestion>
    {
        public static readonly AzmoonQuestionBiz Instance = new AzmoonQuestionBiz();
        public virtual JqGrid.PagedList<AzmoonQuestion> GetAllPagedList(GridSettings grid, int AzmoonEntityId)
        {
            using (var ctx = new SWEntities())
                return ctx.AzmoonQuestion.Where(x => x.AzmoonEntityId == AzmoonEntityId).FilterAndSortJqGrid(grid).ToPagedList(grid);//.Select(x=> new AzmoonQuestionVM() { Id=x.Id,Question=x.Question,AzmoonOrder=x.AzmoonOrder,QuestionType=x.QuestionType,required=x.required})
        }
        public async override Task<int> Remove(int id)
        {
            using (var ctx = new SWEntities())
            {
                var result = await ctx.AzmoonQuestion.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (null == result)
                {
                    return 0;
                }
                ctx.AzmoonQuestion.Remove(result);
                await ctx.SaveChangesAsync();

                return result.AzmoonEntityId;
            }

        }
        public async Task<AzmoonQuestion> GetIncludeOptions(int id)
        {
            using (var ctx = new SWEntities())
            {
                return await ctx.AzmoonQuestion.Include(x => x.AzmoonQuestionOptions).FirstOrDefaultAsync(x => x.Id == id);
            }
        }
        public async Task WelcomeGoodbyeValidation(AzmoonQuestion obj)
        {
            if (obj.QuestionType == QuestionEnum.Goodbye)
            {
                obj.AzmoonOrder = 1000;
            }
            if (obj.QuestionType == QuestionEnum.Welcome)
            {
                obj.AzmoonOrder = -1;
            }
            if (obj.Id > 0)
            {
                return;
            }
            if (obj.QuestionType == QuestionEnum.Goodbye || obj.QuestionType == QuestionEnum.Welcome)
            {
                using (var ctx = new SWEntities())
                {
                    int res = await ctx.AzmoonQuestion.CountAsync(x => x.QuestionType == obj.QuestionType && x.AzmoonEntityId == obj.AzmoonEntityId);
                    if (res > 0)
                    {
                        throw new System.Exception("خطا:هر آزمون فقط یک مورد صفحه خوش امدگویی و صفحه تشکر  می تواند داشته باشد");
                    }
                }

            }

        }
    }
}