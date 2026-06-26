using System;
using System.Threading.Tasks;
using SurveyWeb.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using SurveyWeb.Models.Security;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Biz
{
    public class UserBiz : RepositoryBase<Models.User>
    {
        public static readonly UserBiz Instance = new UserBiz();
        public List<SelectListItem> DropDown(bool isAdmin)
        {
            using (var ctx = new Context())
                return ctx.User.Where(x => !isAdmin || x.RoleId == Roles.Admin).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.UserName }).ToList();
        }


        public Models.User FindByUserName(string name)
        {
            using (var ctx = new Models.Context())
            {
                return ctx.User.FirstOrDefault(x => x.UserName == name);
            }
        }
        public async Task<User> UpdateAdmin(User model)
        {
            using (var ctx = new Models.Context())
            {
                if (model.Id == 0)
                {
                    throw new Exception(" کاربر نامعتبر");
                }
                var user = ctx.User.FirstOrDefault(x => x.Id == model.Id);
                if (user == null)
                {
                    throw new Exception(" کاربر نامعتبر");
                }
                user.RoleId = model.RoleId;
                user.Archive = model.Archive;
                user.Pass = model.Pass;
                await ctx.SaveChangesAsync();
                return user;
            }
        }
        public override async Task<User> Save(User model, bool changeDate = true)
        {
            if (model.Id == 0)
            {
                model.CreatedDate = DateTime.Now;
                model.RoleId = Models.Roles.User;
            }
            model.Validate();
            using (var ctx = new Models.Context())
            {
                if (model.Id == 0)
                {
                    if (ctx.User.Where(x => x.UserName == model.UserName).Count() > 0)
                    {
                        throw new Exception(" ایمیل شما در سامانه وجود دارد. لطفا یک ایمیل دیگر انتخاب کنید");
                    }
                    ctx.User.Add(model);
                    await ctx.SaveChangesAsync();
                    return model;
                }
                var user = ctx.User.FirstOrDefault(x => x.Id == model.Id);
                if (user == null)
                {
                    throw new Exception(" کاربر نامعتبر");
                }
                model.UpdateDate = DateTime.Now;
                model.CreatedDate = user.CreatedDate;
                model.UserName = user.UserName;
                model.RoleId = user.RoleId;
                ctx.Entry(user).CurrentValues.SetValues(model);
                //ctx.Entry(model).State = EntityState.Modified;
                //ctx.Entry(model).Property(x => x.CreatedDate).IsModified = false;
                //ctx.Entry(model).Property(x => x.UserName).IsModified = false;
                //ctx.Entry(model).Property(x => x.RoleId).IsModified = false;
                await ctx.SaveChangesAsync();
                return user;
            }
        }

        public JqGrid.PagedList<RoleUserVm> GetAllPagedListRoleByUserId(GridSettings grid, int id)
        {
            using (var ctx = new Context())
                return ctx.RoleUser.Where(x => x.UserId == id).Select(x => new RoleUserVm { Id = x.Id, RoleName = x.Role.Name }).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public async Task SaveRoleUser(RoleUser user)
        {
            using (var ctx = new Context())
            {
                if (ctx.RoleUser.Any(x=>x.RoleId==user.RoleId && x.UserId==user.UserId))
                {

                }
                else
                {
                    ctx.RoleUser.Add(user);
                    await ctx.SaveChangesAsync();
                }
            }

        }

        public bool IsAccess(Permisstion permission, int userId)
        {
            if (userId == 1)
                return true;
            using (var ctx = new Context())
            {
                var query = (
                    from u in ctx.RoleUser.Where(x => x.UserId == userId)
                    join p in ctx.RolePermission on u.RoleId equals p.RoleId //into pppp
                    where p.Permisstion == permission
                    select p
                           );
                return query.Count() > 0;
            }

        }
        public List<Permisstion> GetPermisstionsByRoleId(int roleId)
        {
            using (var ctx = new Context())
                return ctx.RolePermission.Where(x => x.RoleId == roleId).Select(x => x.Permisstion).ToList();
        }
        public List<Permisstion> GetPermisstionsByUserId(int userId)
        {
            if (userId == 1)
            {
                var res = new List<Permisstion>();
                foreach (Permisstion item in Enum.GetValues(typeof(Permisstion)))
                {
                    res.Add(item);
                }
                return res;
            }
            using (var ctx = new Context())
            {
                var query = (
                    from u in ctx.RoleUser.Where(x => x.UserId == userId)
                    join p in ctx.RolePermission on u.RoleId equals p.RoleId //into pppp
                    select p.Permisstion
                           );
                return query.ToList();
            }
        }

        public async Task RemoveRoleUser(int id)
        {
            using (var ctx = new Context())
            {
                var result = ctx.RoleUser.Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    throw new System.Exception("رکورد یافت نشد");
                }
                ctx.RoleUser.Remove(result);
                await ctx.SaveChangesAsync();
            }
        }
    }
}