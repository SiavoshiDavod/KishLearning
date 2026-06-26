using SurveyWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class SurveyQuestionOptionBiz : RepositoryBase<Models.SurveyQuestionOption>
    {
        public static readonly SurveyQuestionOptionBiz Instance = new SurveyQuestionOptionBiz();
     
        public async Task<bool> UpdateImageProperty(int SurveyQuestionOptionId, short width, short height, string QuestionOption)
        {
            using (var ctx = new Context())
            {
                var obj= ctx.SurveyQuestionOption.FirstOrDefault(x => x.Id== SurveyQuestionOptionId);
                if (obj==null)
                {
                    return false;
                }
                obj.QuestionOption = QuestionOption;
                obj.Height = height;
                obj.Width = width;
                await ctx.SaveChangesAsync();
                return true;
            }
        }
    }
}