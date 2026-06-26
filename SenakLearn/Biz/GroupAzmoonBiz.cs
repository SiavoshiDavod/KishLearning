using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class GroupAzmoonBiz : RepositoryBaseSurvey<Models.GroupAzmoon>
    {
        public static readonly GroupAzmoonBiz Instance = new GroupAzmoonBiz();

        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.GroupAzmoon.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }
    }
}