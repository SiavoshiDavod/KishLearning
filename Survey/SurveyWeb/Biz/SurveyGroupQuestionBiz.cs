using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SurveyWeb.Biz
{
    public class SurveyGroupQuestionBiz : RepositoryBase<Models.SurveyGroupQuestion>
    {
        public static readonly SurveyGroupQuestionBiz Instance = new SurveyGroupQuestionBiz();

        public List<SelectListItem> DropDown(int SurveyEntityId)
        {
            using (var ctx = new Context())
                return ctx.SurveyGroupQuestion.Where(x=>x.SurveyEntityId== SurveyEntityId).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.SurveyGroupQuestionTitle }).ToList();
        }
        public override async Task<int> Remove(int id)
        {
            using (var ctx = new Context())
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