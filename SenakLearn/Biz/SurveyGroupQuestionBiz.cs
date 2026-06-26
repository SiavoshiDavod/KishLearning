using SenakLearn.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using System.Data.Entity;

namespace SenakLearn.Biz
{
    public class SurveyGroupQuestionBiz : RepositoryBaseSurvey<Models.SurveyGroupQuestion>
    {
        public static readonly SurveyGroupQuestionBiz Instance = new SurveyGroupQuestionBiz();

        public List<SelectListItem> DropDown(int SurveyEntityId)
        {
            using (var ctx = new SWEntities())
                return ctx.SurveyGroupQuestion.Where(x => x.SurveyEntityId == SurveyEntityId).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.SurveyGroupQuestionTitle }).ToList();
        }
        public async override Task<int> Remove(int id)
        {
            using (var context = new SWEntities())
            {
                SurveyGroupQuestion result = await context.SurveyGroupQuestion.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (null == result)
                {
                    return 0;
                }
                var ret = result.SurveyEntityId;
                context.SurveyGroupQuestion.Remove(result);
                await context.SaveChangesAsync();
                return ret;
            }

        }
    }
}