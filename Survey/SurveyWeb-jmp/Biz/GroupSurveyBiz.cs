using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SurveyWeb.Biz
{
    public class GroupSurveyBiz : RepositoryBase<Models.GroupSurvey>
    {
        public static readonly GroupSurveyBiz Instance = new GroupSurveyBiz();

        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.GroupSurvey.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }
    }
}