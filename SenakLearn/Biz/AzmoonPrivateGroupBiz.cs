using System;
using System.Threading.Tasks;
using SenakLearn.JqGrid;
using SenakLearn.Models.Security;
using SenakLearn.Models;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models.wrapper;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SenakLearn.Biz
{
    public class AzmoonPrivateGroupBiz : RepositoryBaseSurvey<AzmoonPrivateGroup>
    {
        public static readonly AzmoonPrivateGroupBiz Instance = new AzmoonPrivateGroupBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.AzmoonPrivateGroup.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }

        public JqGrid.PagedList<SurveyPrivateGroupUserVm> GetAllPagedListUserByAzmoonPrivateGroupId(GridSettings grid, int id)
        {
            using (var ctx = new SWEntities())
                return ctx.AzmoonPrivateGroupUser.Where(x => x.AzmoonPrivateGroupId == id).Select(x=>new SurveyPrivateGroupUserVm() { Id=x.Id,UserName=x.User.user_name}).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public async Task SaveAzmoonPrivateGroupUser(AzmoonPrivateGroupUser user)
        {
            using (var ctx = new SWEntities())
            {
                ctx.AzmoonPrivateGroupUser.Add(user);
                await ctx.SaveChangesAsync();
            }

        }

        public async Task RemoveAzmoonPrivateGroupUser(int id)
        {
            using (var ctx = new SWEntities())
            {
                var result = ctx.AzmoonPrivateGroupUser.Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    throw new System.Exception("رکورد یافت نشد");
                }
                ctx.AzmoonPrivateGroupUser.Remove(result);
                await ctx.SaveChangesAsync();
            }
        }
    }
}