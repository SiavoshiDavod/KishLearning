using SenakLearn.Models;
using SenakLearn.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class SurveyEntityBiz : RepositoryBaseSurvey<Models.SurveyEntity>
    {
        public static readonly SurveyEntityBiz Instance = new SurveyEntityBiz();
        public async Task<SurveyEntity> GetIncludeQuestion(int id, string ip, int userId)
        {
            using (var ctx = new SWEntities())
            {
                var obj = await ctx.SurveyEntity.Include(x => x.SurveyQuestions).Include("SurveyQuestions.SurveyQuestionOptions").FirstOrDefaultAsync(x => x.Id == id);

                if (obj.IsIpRestriction && await ctx.SurveyUserAnswer.AnyAsync(x => x.SurveyEntityId == id && x.Ip == ip))
                {
                    throw new System.Exception(" شما قبلا در این نظر سنجی شرکت کرده اید");
                }
                if (obj.FromDate > DateTime.Now)
                    throw new Exception(" تاریخ و زمان شروع "+obj.Name+" فرا نرسیده است !");
                if (obj.ToDate < DateTime.Now)
                    throw new Exception(" تاریخ و زمان پایان " + obj.Name + " به اتمام رسیده است !");
                if (obj.IsUserMustBeLogin)
                {
                    if (userId <= 0)
                    {
                        throw new System.Exception(" لطفا ابتدا در سیستم لاگین کنید");
                    }
                    if (await ctx.SurveyUserAnswer.AnyAsync(x => x.SurveyEntityId == id && x.UserId == userId))
                    {
                        throw new System.Exception(" شما قبلا در این نظر سنجی شرکت کرده اید");
                    }
                }

                if (obj.IsPrivate && obj.SurveyPrivateGroupId != null && GetAllActivePrivateByUserId(userId).Count(x => x.Id == obj.Id) <= 0)
                {
                    throw new Exception("شما دسترسی برای شرکت در این نظرسنجی را ندارید");
                }

                return obj;
            }
        }
        public async Task AddQuestion(int id)
        {
            using (var ctx = new SWEntities())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.SurveyEntities set QuestionCount=QuestionCount+1 where Id={id}");
        }
        public async Task AddAnswer(int id)
        {
            using (var ctx = new SWEntities())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.SurveyEntities set AnswerCount=AnswerCount+1 where Id={id}");
        }
        public async Task<SurveyEntity> GetIncludeQuestionAndAnswer(int id)
        {
            using (var ctx = new SWEntities())
            {
                return await ctx.SurveyEntity.Include(x => x.SurveyQuestions).Include("SurveyQuestions.SurveyQuestionOptions").Include("SurveyQuestions.SurveyAnswers").FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<SurveyQuestionAnswerdWrapper> GetIncludeQuestionAndAnswerFormated(int id)
        {
            SurveyQuestionAnswerdWrapper result = new SurveyQuestionAnswerdWrapper();
            using (var ctx = new SWEntities())
            {

                try
                {
                    var entity = await ctx.SurveyEntity.SingleOrDefaultAsync(a => a.Id == id);
                    var questionsQuery =  ctx.SurveyQuestion.Where(w => w.SurveyEntityId == id && w.SurveyAnswers.Count() > 0).OrderBy(a => a.Id);
                    var questions = await questionsQuery.ToListAsync();

                    var questionIds = questions.Select(a => a.Id).ToList();
                    var answers = (from a in ctx.SurveyAnswer
                                   //join q in questionsQuery on a.SurveyQuestionId equals q.Id
                                   where questionIds.Contains(a.SurveyQuestionId)
                                   select new SurveyAnswerUserWrapper
                                   {
                                       QuestionId = a.SurveyQuestionId,
                                       UserId = a.SurveyUserAnswerId,
                                       Result = a.Result ?? string.Empty,
                                       Create = a.CreatedDate,
                                       QuestionTypeId=(int)a.SurveyQuestion.QuestionType
                                   }).GroupBy(g => g.UserId).ToList();

                    var Options = await ctx.SurveyQuestionOption.Where(w => questionIds.Contains(w.SurveyQuestionId)).ToListAsync();
                    List<int> optionsId = new List<int> { 1, 3,7 };
                    result.Questions = questions.Select(a => new SurveyQuestionWrapper { Question = a.Question, QuestionId = a.Id }).ToList();
                    result.AnswerUsers = answers.Select(a => new SurveyAnswerUserWrapper
                    {
                        Answers = (from q in questions
                                   join i in a on q.Id equals i.QuestionId into answer
                                   from i in answer.DefaultIfEmpty()
                                   select new { i, q }).Select(x => new SurveyAnswerWrapper
                                   {
                                       Result = x.i != null ?

                                       !optionsId.Contains((int)x.q.QuestionType)? new List<string> { x.i.Result }:
                                           (Options.Any(b => b.Id.ToString() == x.i.Result) ? new List<string> { Options.FirstOrDefault(b => b.Id.ToString() == x.i.Result).QuestionOption } 
                                           : (x.i.Result != null ? Options.Where(w=>x.i.Result.Contains(w.Id.ToString())).Select(o=>o.QuestionOption).ToList()
                                           : new List<string>()))

                                              : new List<string>(),
                                       QuestionId = x.q.Id,
                                   }).OrderBy(o => o.QuestionId).ToList(),
                        UserId = a.Key,
                        Create = a.FirstOrDefault()?.Create,
                        CreateStr = a.FirstOrDefault()?.Create != null ? DateTimeExtensions.ToPersianDateTime(a.First().Create.Value) : string.Empty,
                    }).ToList();
                    result.Name = entity.Name;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return result;
        }
        public List<SurveyEntity> GetAllActive(System.Linq.Expressions.Expression<Func<SurveyEntity, bool>> Expr)
        {
            using (var ctx = new SWEntities())
            {
                return ctx.SurveyEntity.Where(x => x.Status && !x.IsPrivate).Where(Expr).ToList();
            }
        }
        public List<SurveyEntity> GetAllActivePrivateByUserId(int userId)
        {
            using (var ctx = new SWEntities())
            {
                var query = (from s in ctx.SurveyEntity.Where(x => x.Status && x.IsPrivate)
                             join u in ctx.SurveyPrivateGroupUser on s.SurveyPrivateGroupId equals u.SurveyPrivateGroupId
                             where u.UserId == userId
                             select s);
                return query.ToList();
            }
        }
    }
}