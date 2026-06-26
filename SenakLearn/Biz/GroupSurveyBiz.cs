using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class GroupSurveyBiz : RepositoryBaseSurvey<Models.GroupSurvey>
    {
        public static readonly GroupSurveyBiz Instance = new GroupSurveyBiz();

        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.GroupSurvey.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }
    }
}