using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.Models.Security;

namespace SurveyWeb.Controllers
{
    public class RolesController : BaseAdminController
    {


        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.RoleBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                RoleData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Role> RoleColumns { get; private set; } = GetRoleColumns();
        public static GridColumnModelList<Role> GetRoleColumns()
        {
            if (RoleColumns == null)
            {
                RoleColumns = new GridColumnModelList<Role>();
                RoleColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                RoleColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("250");
                RoleColumns.Add(x => x.Name).SetCaption("سوال").SetWidth("300");
            }
            return RoleColumns;
        }
        // GET: Roles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role cartable = await RoleBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Roles/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Role cartable = await RoleBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Role user)
        {
            await RoleBiz.Instance.Save(user);
            return RedirectToAction("Index", "Roles");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role user = await RoleBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await RoleBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Roles");
        }
    }
}

