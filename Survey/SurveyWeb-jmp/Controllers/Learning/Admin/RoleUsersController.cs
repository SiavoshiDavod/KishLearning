using System.Threading.Tasks;
using System.Web.Mvc;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;
using SenakLearn.Models.Security;
using SenakLearn.Models.wrapper;

namespace SenakLearn.Controllers
{
    public class RoleUsersController : BaseAdminController
    {


        // GET: RoleUserLogs
        public ActionResult Index(int id)
        {
            ViewBag.UserName = ( Biz.UserBiz.Instance.Find(id))?.user_name;
            return PartialView(id);
        }
        public ActionResult LoadList(GridSettings grid, int id)
        {
            var list = Biz.UserBiz.Instance.GetAllPagedListRoleByUserId(grid, id);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyEntityData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        // GET: RoleUserLogs/Details/5
        public static GridColumnModelList<RoleUserVm> RoleUserColumns { get; private set; } = GetRoleUserColumns();
        public static GridColumnModelList<RoleUserVm> GetRoleUserColumns()
        {
            if (RoleUserColumns == null)
            {
                RoleUserColumns = new GridColumnModelList<RoleUserVm>();
                RoleUserColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                RoleUserColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                RoleUserColumns.Add(x => x.RoleName).SetCaption("دسترسی").SetWidth("300");
            }
            return RoleUserColumns;
        }
        // GET: RoleUsers/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoleUser cartable = await RoleUserBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartable);
        //}

        // GET: RoleUsers/Create
        //public async Task<ActionResult> Create(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View();
        //    }
        //    RoleUser cartable = await RoleUserBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return View();
        //    }
        //    return View(cartable);
        //}

        // POST: RoleUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(RoleUser user)
        {
            await UserBiz.Instance.SaveRoleUser(user);
            return RedirectToAction("Index", "RoleUsers", new { id = user.RoleId });
        }

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoleUser user = await RoleUserBiz.Instance.Get(id.Value);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        // POST: RoleUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await UserBiz.Instance.RemoveRoleUser(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
