using SenakLearn.Models;
using SenakLearn.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class SurveyAnswerBiz : RepositoryBaseSurvey<Models.SurveyAnswer>
    {
        public static readonly SurveyAnswerBiz Instance = new SurveyAnswerBiz();
        public async Task SaveBatch(List<SurveyAnswer> aswers, string ip, int userId)
        {
            List<SurveyAnswer> result = new List<SurveyAnswer>();
            foreach (var item in aswers)
            {
                item.CreatedDate = DateTime.Now;
                try
                {
                    item.CreatedDate = DateTime.Now;
                    item.Validate();
                    result.Add(item);
                }
                catch (Exception)
                {
                }
            }
            if (result.Count <= 0)
            {
                throw new Exception("پاسخی برای دخیره سازی یافت نشد");
            }
            using (var ctx = new SWEntities())
            {
                ctx.SurveyUserAnswer.Add(new SurveyUserAnswer() { SurveyEntityId = result.First().SurveyEntityId, Ip = ip, UserId = userId > 0 ? userId : (int?)null, CreatedDate = DateTime.Now, SurveyAnswers = result });
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<List<SurveyUserAnswerVM>> CheckRequiredQuestion(List<SurveyAnswer> aswers)
        {
            List<SurveyUserAnswerVM> result = new List<SurveyUserAnswerVM>();
            using (var ctx = new SWEntities())
            {
                foreach (var item in aswers)
                {
                    var question = await ctx.SurveyQuestion.SingleOrDefaultAsync(a => a.Id == item.SurveyQuestionId);
                    if (question != null)
                        result.Add(new SurveyUserAnswerVM { Question = question.Question, Id = item.SurveyQuestionId, IsRequired = question.required, Answered = !string.IsNullOrEmpty(item.Result) });
                }
            }

            return result;
        }
    }
}