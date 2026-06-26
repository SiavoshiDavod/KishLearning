using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using SurveyWeb.Models.wrapper;
using SurveyWeb.Models;

namespace SurveyWeb.Biz
{
    public class SurveyQuestionBiz : RepositoryBase<Models.SurveyQuestion>
    {
        public static readonly SurveyQuestionBiz Instance = new SurveyQuestionBiz();
        public virtual JqGrid.PagedList<SurveyQuestion> GetAllPagedList(GridSettings grid, int surveyEntityId)
        {
            using (var ctx = new Context())
                return ctx.SurveyQuestion.Where(x => x.SurveyEntityId == surveyEntityId).FilterAndSortJqGrid(grid).ToPagedList(grid);//.Select(x=> new SurveyQuestionVM() { Id=x.Id,Question=x.Question,SurveyOrder=x.SurveyOrder,QuestionType=x.QuestionType,required=x.required})
        }
        public async override Task<int> Remove(int id)
        {
            using (var ctx = new Context())
            {
                var result = await ctx.SurveyQuestion.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (null == result)
                {
                    return 0;
                }
                ctx.SurveyQuestion.Remove(result);
                await ctx.SaveChangesAsync();

                return result.SurveyEntityId;
            }
                
        }
        public async Task<SurveyQuestion> GetIncludeOptions(int id)
        {
            using (var ctx=new Context() )
            {
                return await ctx.SurveyQuestion.Include(x => x.SurveyQuestionOptions).FirstOrDefaultAsync(x=>x.Id==id);
            }
        }
        public async Task WelcomeGoodbyeValidation(SurveyQuestion obj)
        {
            if (obj.QuestionType == QuestionEnum.Goodbye)
            {
                obj.SurveyOrder = 1000;
            }
            if (obj.QuestionType == QuestionEnum.Welcome)
            {
                obj.SurveyOrder = -1;
            }
            if (obj.Id>0)
            {
                return;
            }
            if (obj.QuestionType==QuestionEnum.Goodbye|| obj.QuestionType==QuestionEnum.Welcome)
            {
                using (var ctx = new Context())
                {
                    int res = await ctx.SurveyQuestion.CountAsync(x=>x.QuestionType==obj.QuestionType && x.SurveyEntityId==obj.SurveyEntityId);
                    if (res>0)
                    {
                        throw new System.Exception("خطا:هر نظرسنجی فقط یک مورد صفحه خوش امدگویی و صفحه تشکر  می تواند داشته باشد");
                    }
                }
               
            }
           
        }
    }
}