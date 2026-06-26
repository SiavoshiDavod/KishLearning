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
    public class OrgController : BaseAdminController
    {


        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = OrgBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<Organization> OrgColumns { get; private set; } = GetRoleColumns();
        public static GridColumnModelList<Organization> GetRoleColumns()
        {
            if (OrgColumns == null)
            {
                OrgColumns = new GridColumnModelList<Organization>();
                OrgColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                OrgColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                OrgColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
                OrgColumns.Add(x => x.Descript).SetCaption("توضیحات").SetWidth("300");
            }
            return OrgColumns;
        }
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var org =  OrgBiz.Instance.Get(id.Value);
            if (org == null)
            {
                return HttpNotFound();
            }
            return View(org);
        }

        // GET: Roles/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View(new Organization());
            }
            var cartable = OrgBiz.Instance.Get(id.Value);
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
        public ActionResult Create(Organization org)
        {
            OrgBiz.Instance.Save(org);
            return RedirectToAction("Index", "Org");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var org = OrgBiz.Instance.Get(id.Value);
            if (org == null)
            {
                return HttpNotFound();
            }
            return View(org);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrgBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Org");
        }
    }
}

