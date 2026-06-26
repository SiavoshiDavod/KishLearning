using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.Models.Security;

namespace SenakLearn.Controllers
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
                RoleColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                RoleColumns.Add(x => x.Name).SetCaption("سوال").SetWidth("300");
            }
            return RoleColumns;
        }
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role cartable =  RoleBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Roles/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Role cartable =  RoleBiz.Instance.Get(id.Value);
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
        public ActionResult Create(Role user)
        {
             RoleBiz.Instance.Save(user);
            return RedirectToAction("Index", "Roles");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role user =  RoleBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             RoleBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Roles");
        }
    }
}

