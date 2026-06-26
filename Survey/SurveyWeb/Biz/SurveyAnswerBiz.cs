using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class SurveyAnswerBiz : RepositoryBase<Models.SurveyAnswer>
    {
        public static readonly SurveyAnswerBiz Instance = new SurveyAnswerBiz();
        public async Task SaveBatch(List<SurveyAnswer> aswers, string ip, int userId)
        {
            List<SurveyAnswer> result=new List<SurveyAnswer>();
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
            if (result.Count<=0)
            {
                throw new HandledException("پاسخی برای دخیره سازی یافت نشد");
            }
            using (var ctx = new Models.Context())
            {
                ctx.SurveyUserAnswer.Add(new SurveyUserAnswer() { SurveyEntityId= result.First().SurveyEntityId,Ip=ip,UserId=userId>0?userId:(int?)null,CreatedDate=DateTime.Now,SurveyAnswers= result });
                await ctx.SaveChangesAsync();
            }
        }
    }
}