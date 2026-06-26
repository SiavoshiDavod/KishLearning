using SenakLearn.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using System.Data.Entity;

namespace SenakLearn.Biz
{
    public class AzmoonGroupQuestionBiz : RepositoryBaseSurvey<Models.AzmoonGroupQuestion>
    {
        public static readonly AzmoonGroupQuestionBiz Instance = new AzmoonGroupQuestionBiz();

        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.AzmoonGroupQuestion.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.AzmoonGroupQuestionTitle }).ToList();
        }
        public List<AzmoonGroupQuestion> GetAllGroupQuestion()
        {
            using (var context = new SWEntities())
            {
                List<AzmoonGroupQuestion> list = null;
                try
                {
                     list = context.AzmoonGroupQuestion.ToList();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
                return list;
            }
            }
        public async override Task<int> Remove(int id)
        {
            using (var context = new SWEntities())
            {
                AzmoonGroupQuestion result = await context.AzmoonGroupQuestion.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (null == result)
                {
                    return 0;
                }
                context.AzmoonGroupQuestion.Remove(result);
                await context.SaveChangesAsync();
                return 1;
            }

        }
    }
}