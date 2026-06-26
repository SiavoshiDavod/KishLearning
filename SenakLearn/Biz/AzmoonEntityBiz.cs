using SenakLearn.Models;
using SenakLearn.Models.Azmoon;
using SenakLearn.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class AzmoonEntityBiz : RepositoryBaseSurvey<Models.AzmoonEntity>
    {
        public static readonly AzmoonEntityBiz Instance = new AzmoonEntityBiz();
        public AzmoonEntity GetIncludeQuestion(int id, string ip, int userId)
        {
            AzmoonEntity obj = null;
            using (var ctx = new SWEntities())
            {
                //try
                //{
                obj = ctx.AzmoonEntity.Include(x => x.AzmoonQuestions).Include("AzmoonQuestions.AzmoonQuestionOptions").FirstOrDefault(x => x.Id == id);
                if (obj.IsIpRestriction && ctx.AzmoonUserAnswer.Any(x => x.AzmoonEntityId == id && x.Ip == ip))
                {
                    throw new System.Exception(" شما قبلا در این آزمون شرکت کرده اید");
                }

                if (obj.IsUserMustBeLogin)
                {
                    if (userId <= 0)
                    {
                        //throw new ExceptionHandel.LoginReqException();
                        throw new System.Exception(" برای شرکت در آزمون باید ابتدا وارد شوید !");
                    }
                    if (ctx.AzmoonUserAnswer.Any(x => x.AzmoonEntityId == id && userId > 0 && x.UserId == userId))
                    {
                        throw new System.Exception(" شما قبلا در این آزمون شرکت کرده اید !");
                    }
                }

                if (obj.IsPrivate && obj.AzmoonPrivateGroupId != null && GetAllActivePrivateByUserId(null, userId).Count(x => x.Id == obj.Id) <= 0)
                {
                    throw new Exception("شما دسترسی برای شرکت در این آزمون را ندارید");
                }

                //}
                //catch (Exception ex)
                //{
                //    var err = ex;
                //    return  new AzmoonEntity();
                //}




                return obj;
            }
        }
        public async Task AddQuestion(int id)
        {
            using (var ctx = new SWEntities())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.AzmoonEntities set QuestionCount=QuestionCount+1 where Id={id}");
        }
        public async Task AddAnswer(int id)
        {
            using (var ctx = new SWEntities())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.AzmoonEntities set AnswerCount=AnswerCount+1 where Id={id}");
        }
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
            {
                var list = ctx.AzmoonEntity.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
                list.Insert(0, new SelectListItem { Text = "همه" });
                return list;
            }
        }
        public async Task<AzmoonEntity> GetIncludeQuestionAndAnswer(int id)
        {
            using (var ctx = new SWEntities())
            {
                return await ctx.AzmoonEntity.Include(x => x.AzmoonQuestions).Include("AzmoonQuestions.AzmoonQuestionOptions").Include("AzmoonQuestions.AzmoonAnswers").FirstOrDefaultAsync(x => x.Id == id);
            }
        }
        public async Task<AzmoonEntityWrapper> GetAzmoon(int id)
        {
            using (var ctx = new SWEntities())
            {
                var item = await (from i in ctx.AzmoonEntity
                                  join g in ctx.GroupAzmoon on i.GroupAzmoonId equals g.Id
                                  where i.Id == id
                                  select new { i.Name, i.TimeDuration, groupName = g.Name, i.TotalScore, i.Id })
                              .Select(a => new AzmoonEntityWrapper
                              {
                                  AzmoonEntityId = a.Id,
                                  AzmoonEntityName = a.Name,
                                  GroupAzmoonName = a.groupName,
                                  TimeDuration = a.TimeDuration,
                                  TotalScore = a.TotalScore
                              }).FirstOrDefaultAsync();
                return item;
            }
        }
        public async Task<AzmoonQuestionAnswerdWrapper> GetIncludeQuestionAndAnswerFormated(int id)
        {
            AzmoonQuestionAnswerdWrapper result = new AzmoonQuestionAnswerdWrapper();
            using (var ctx = new SWEntities())
            {

                try
                {
                    var entity = await ctx.AzmoonEntity.SingleOrDefaultAsync(a => a.Id == id);
                    var questionsQuery = ctx.AzmoonQuestion.Include(x => x.AzmoonGroupQuestion).Where(w => w.AzmoonEntityId == id && w.AzmoonAnswers.Count() > 0).OrderBy(a => a.Id);
                    var questions = await questionsQuery.Select(a => new AzmoonQuestionWrapper { Question = a.Question, QuestionId = a.Id, QuestionType = (int)a.QuestionType, QuestionGroup = a.AzmoonGroupQuestion.AzmoonGroupQuestionTitle }).ToListAsync();

                    var questionIds = questions.Select(a => a.QuestionId).ToList();
                    var answers = (from a in ctx.AzmoonAnswer
                                       //join q in questionsQuery on a.SurveyQuestionId equals q.Id
                                   where questionIds.Contains(a.AzmoonQuestionId)
                                   select new AzmoonAnswerUserWrapper
                                   {
                                       QuestionId = a.AzmoonQuestionId,
                                       UserId = a.AzmoonUserAnswerId,
                                       Result = a.Result ?? string.Empty,
                                       Create = a.CreatedDate,
                                       QuestionTypeId = (int)a.AzmoonQuestion.QuestionType
                                   }).GroupBy(g => g.UserId).ToList();

                    var Options = await ctx.AzmoonQuestionOption.Where(w => questionIds.Contains(w.AzmoonQuestionId)).ToListAsync();
                    List<int> optionsId = new List<int> { 1, 3, 7 };
                    result.Questions = questions.ToList();
                    result.AnswerUsers = answers.Select(a => new AzmoonAnswerUserWrapper
                    {
                        Answers = (from q in questions
                                   join i in a on q.QuestionId equals i.QuestionId into answer
                                   from i in answer.DefaultIfEmpty()
                                   select new { i, q }).Select(x => new AzmoonAnswerWrapper
                                   {
                                       Result = x.i != null ?

                                       !optionsId.Contains((int)x.q.QuestionType) ? new List<string> { x.i.Result } :
                                           (Options.Any(b => b.Id.ToString() == x.i.Result) ? new List<string> { Options.FirstOrDefault(b => b.Id.ToString() == x.i.Result).QuestionOption }
                                           : (x.i.Result != null ? Options.Where(w => x.i.Result.Contains(w.Id.ToString())).Select(o => o.QuestionOption).ToList()
                                           : new List<string>()))

                                              : new List<string>(),
                                       QuestionId = x.q.QuestionId,
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
        public List<AzmoonEntity> GetAllActive(AzmoonEntityType? azmoonEntityType, System.Linq.Expressions.Expression<Func<AzmoonEntity, bool>> Expr)
        {
            using (var ctx = new SWEntities())
            {
                return ctx.AzmoonEntity.Where(x => x.Status && !x.IsPrivate && (azmoonEntityType == null || x.AzmoonEntityType == azmoonEntityType)).Where(Expr).ToList();
            }
        }
        public List<AzmoonEntity> GetAllActivePrivateByUserId(AzmoonEntityType? azmoonEntityType, int userId)
        {
            using (var ctx = new SWEntities())
            {
                var query = (from s in ctx.AzmoonEntity.Where(x => x.Status && x.IsPrivate && (azmoonEntityType == null || x.AzmoonEntityType == azmoonEntityType))
                             join u in ctx.AzmoonPrivateGroupUser on s.AzmoonPrivateGroupId equals u.AzmoonPrivateGroupId
                             where u.UserId == userId
                             select s);
                return query.ToList();
            }
        }
    }
}