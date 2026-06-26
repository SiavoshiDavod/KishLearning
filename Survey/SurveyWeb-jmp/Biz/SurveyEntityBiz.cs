using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class SurveyEntityBiz : RepositoryBase<Models.SurveyEntity>
    {
        public static readonly SurveyEntityBiz Instance = new SurveyEntityBiz();
        public async Task<SurveyEntity> GetIncludeQuestion(int id, string ip, int userId)
        {
            using (var ctx = new Context())
            {
                var obj = await ctx.SurveyEntity.Include(x => x.SurveyQuestions).Include("SurveyQuestions.SurveyQuestionOptions").FirstOrDefaultAsync(x => x.Id == id);

                if (obj.IsIpRestriction && await ctx.SurveyUserAnswer.AnyAsync(x => x.SurveyEntityId == id && x.Ip == ip))
                {
                    throw new System.Exception(" شما قبلا در این نظر سنجی شرکت کرده اید");
                }

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
            using (var ctx = new Context())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.SurveyEntities set QuestionCount=QuestionCount+1 where Id={id}");
        }
        public async Task AddAnswer(int id)
        {
            using (var ctx = new Context())
                await ctx.Database.ExecuteSqlCommandAsync($"update dbo.SurveyEntities set AnswerCount=AnswerCount+1 where Id={id}");
        }
        public async Task<SurveyEntity> GetIncludeQuestionAndAnswer(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.SurveyEntity.Include(x => x.SurveyQuestions).Include("SurveyQuestions.SurveyQuestionOptions").Include("SurveyQuestions.SurveyAnswers").FirstOrDefaultAsync(x => x.Id == id);
            }
        }
        public List<SurveyEntity> GetAllActive(System.Linq.Expressions.Expression<Func<SurveyEntity, bool>> Expr)
        {
            using (var ctx = new Context())
            {
                return ctx.SurveyEntity.Where(x => x.Status && !x.IsPrivate).Where(Expr).ToList();
            }
        }
        public List<SurveyEntity> GetAllActivePrivateByUserId(int userId)
        {
            using (var ctx = new Context())
            {
                var query = (from s in ctx.SurveyEntity.Where(x => x.Status && x.IsPrivate)
                             join u in ctx.SurveyPrivateGroupUser on s.SurveyPrivateGroupId equals u.SurveyPrivateGroupId
                             where u.UserId == userId select s);
                return query.ToList();
            }
        }
    }
}