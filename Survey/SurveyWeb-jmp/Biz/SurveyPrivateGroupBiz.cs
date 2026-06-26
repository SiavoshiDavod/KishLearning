using System;
using System.Threading.Tasks;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.Security;
using SurveyWeb.Models;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models.wrapper;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SurveyWeb.Biz
{
    public class SurveyPrivateGroupBiz : RepositoryBase<SurveyPrivateGroup>
    {
        public static readonly SurveyPrivateGroupBiz Instance = new SurveyPrivateGroupBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.SurveyPrivateGroup.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }

        public JqGrid.PagedList<SurveyPrivateGroupUserVm> GetAllPagedListUserBySurveyPrivateGroupId(GridSettings grid, int id)
        {
            using (var ctx = new Context())
                return ctx.SurveyPrivateGroupUser.Where(x => x.SurveyPrivateGroupId == id).Select(x=>new SurveyPrivateGroupUserVm() { Id=x.Id,UserName=x.User.UserName}).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public async Task SaveSurveyPrivateGroupUser(SurveyPrivateGroupUser user)
        {
            using (var ctx = new Context())
            {
                ctx.SurveyPrivateGroupUser.Add(user);
                await ctx.SaveChangesAsync();
            }

        }

        public async Task RemoveSurveyPrivateGroupUser(int id)
        {
            using (var ctx = new Context())
            {
                var result = ctx.SurveyPrivateGroupUser.Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    throw new System.Exception("رکورد یافت نشد");
                }
                ctx.SurveyPrivateGroupUser.Remove(result);
                await ctx.SaveChangesAsync();
            }
        }
    }
}