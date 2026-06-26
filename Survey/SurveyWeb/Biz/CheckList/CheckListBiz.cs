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
    public class CheckListBiz : RepositoryBase<SurveyWeb.Models.CheckList.CheckList>
    {
        public static readonly CheckListBiz Instance = new CheckListBiz();
        public Models.CheckList.CheckList Find(int id)
        {
            using (var Context = new Context())
            {
                var item = Context.CheckLists.Where(a => a.Id == id).FirstOrDefault();

                return item;
            }
        }
        public List<Models.CheckList.CheckList> FindAll()
        {
            using (var Context = new Context())
            {
                var list = Context.CheckLists.ToList();

                return list;
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.CheckLists.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }
    }
}