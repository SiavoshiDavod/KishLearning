using System.Threading.Tasks;
using System.Web.Mvc;
using SenakLearn.Biz;
using SenakLearn.Models.Security;
using SenakLearn.Models.wrapper;
using System.Collections.Generic;

namespace SenakLearn.Controllers
{
    public class RolePermissionsController : BaseAdminController
    {


        // GET: RolePermissionLogs
        public ActionResult Index(int id)
        {
            ViewBag.RoleName = ( Biz.RoleBiz.Instance.Get(id))?.Name;
            ViewBag.Permissions= UserBiz.Instance.GetPermisstionsByRoleId(id);
            return PartialView(id);
        }
        //public ActionResult LoadList(GridSettings grid, int id)
        //{
        //    var list = Biz.RoleBiz.Instance.GetAllPagedListPermissionByRoleId(grid, id);
        //    return Json(new
        //    {
        //        Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
        //        Page = grid.PageIndex,
        //        Records = list.TotalCount,
        //        Rows = list.ToArray(),
        //        SurveyEntityData = "Null"
        //    },
        //  JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetTreeList()
        {
            var treeList = GetTreeJsonModel.Instance.GetTreeList(GetTreeJsonModel.PermissionParentChildStaticList);
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
        // GET: RolePermissionLogs/Details/5
        //public static GridColumnModelList<RolePermission> RolePermissionColumns { get; private set; } = GetRolePermissionColumns();
        //public static GridColumnModelList<RolePermission> GetRolePermissionColumns()
        //{
        //    if (RolePermissionColumns == null)
        //    {
        //        RolePermissionColumns = new GridColumnModelList<RolePermission>();
        //        RolePermissionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
        //        RolePermissionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
        //        RolePermissionColumns.Add(x => x.PermisstionName).SetCaption("دسترسی").SetWidth("300");
        //    }
        //    return RolePermissionColumns;
        //}

        // POST: RolePermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      [Route("RolePermissions/Create/{id}")]
        public async Task<ActionResult> Create(int id, PermissionVm model)
        {
           await RoleBiz.Instance.SaveRolePermission(model.permission, id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        // POST: RolePermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await RoleBiz.Instance.RemoveRolePermission(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
