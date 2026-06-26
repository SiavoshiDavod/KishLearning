using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using SurveyWeb.Models.CheckList;

namespace SurveyWeb.Biz.CheckList
{
    public class CheckListGroupBiz : RepositoryBase<CheckListGroup>
    {
        public static readonly CheckListGroupBiz Instance = new CheckListGroupBiz();
        public List<CheckListGroup> FindAll() {
            using (Context context = new Context()) {

                var list = context.CheckListGroups.ToList();
                return list;
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.CheckListGroups.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }
    }
}