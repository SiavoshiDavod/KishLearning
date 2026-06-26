using SenakLearn.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class AzmoonQuestionOptionBiz : RepositoryBaseSurvey<Models.AzmoonQuestionOption>
    {
        public static readonly AzmoonQuestionOptionBiz Instance = new AzmoonQuestionOptionBiz();
     
        public async Task<bool> UpdateImageProperty(int AzmoonQuestionOptionId, short width, short height, string QuestionOption)
        {
            using (var ctx = new SWEntities())
            {
                var obj= ctx.AzmoonQuestionOption.FirstOrDefault(x => x.Id== AzmoonQuestionOptionId);
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

        internal async Task<bool> SetCorrect(int id, bool iscorrect)
        {
            using (var ctx = new SWEntities())
            {
                var obj = ctx.AzmoonQuestionOption.FirstOrDefault(x => x.Id == id);
                if (obj == null)
                {
                    return false;
                }
                obj.IsCorrect = iscorrect;
                await ctx.SaveChangesAsync();
                return true;
            }
        }
    }
}