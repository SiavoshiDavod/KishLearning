using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class RoleBiz : RepositoryBase<Models.Security.Role>
    {
        public static readonly RoleBiz Instance = new RoleBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.Role.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
        }

        public JqGrid.PagedList<RolePermission> GetAllPagedListPermissionByRoleId(GridSettings grid, int id)
        {
            using (var ctx = new SWEntities())
                return ctx.RolePermission.Where(x => x.RoleId == id).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public async Task SaveRolePermission(string permissions, int roleId)
        {
            List<RolePermission> permission = new List<RolePermission>();
            foreach (var x in permissions.Replace("[", "").Replace("]", "").Split(',').Select(z => int.Parse(z)))
            {
                if (x < 100)
                {
                    permission.Add(new RolePermission() { Permisstion = (Permisstion)x, RoleId = roleId });
                }
            }
            using (var ctx = new SWEntities())
            {
                ctx.RolePermission.RemoveRange(ctx.RolePermission.Where(z => z.RoleId == roleId));
                ctx.RolePermission.AddRange(permission);
                // ctx.RolePermission.Add(user);
                await ctx.SaveChangesAsync();
            }

        }

        public async Task RemoveRolePermission(int id)
        {
            using (var ctx = new SWEntities())
            {
                var result = ctx.RolePermission.Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    throw new System.Exception("رکورد یافت نشد");
                }
                ctx.RolePermission.Remove(result);
                await ctx.SaveChangesAsync();
            }
        }
    }
}