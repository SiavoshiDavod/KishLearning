using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;
using SenakLearn.Models.Security;
using SenakLearn.Biz;

namespace SenakLearn.Controllers
{
    public class AzmoonPrivateGroupsController : BaseAdminController
    {


        // GET: AzmoonPrivateGroups
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.AzmoonPrivateGroupBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonPrivateGroupData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<AzmoonPrivateGroup> AzmoonPrivateGroupColumns { get; private set; } = GetAzmoonPrivateGroupColumns();
        public static GridColumnModelList<AzmoonPrivateGroup> GetAzmoonPrivateGroupColumns()
        {
            if (AzmoonPrivateGroupColumns == null)
            {
                AzmoonPrivateGroupColumns = new GridColumnModelList<AzmoonPrivateGroup>();
                AzmoonPrivateGroupColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AzmoonPrivateGroupColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                AzmoonPrivateGroupColumns.Add(x => x.Name).SetCaption("عنوان").SetWidth("300");
            }
            return AzmoonPrivateGroupColumns;
        }
        // GET: AzmoonPrivateGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonPrivateGroup cartable = await AzmoonPrivateGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: AzmoonPrivateGroups/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            AzmoonPrivateGroup cartable = await AzmoonPrivateGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: AzmoonPrivateGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AzmoonPrivateGroup user)
        {
            await AzmoonPrivateGroupBiz.Instance.Save(user);
            return RedirectToAction("Index", "AzmoonPrivateGroups");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonPrivateGroup user = await AzmoonPrivateGroupBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AzmoonPrivateGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AzmoonPrivateGroupBiz.Instance.Remove(id);
            return RedirectToAction("Index", "AzmoonPrivateGroups");
        }
    }
}

