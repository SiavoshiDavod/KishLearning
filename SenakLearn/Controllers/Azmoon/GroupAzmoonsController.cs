
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;

namespace SenakLearn.Controllers
{
    public class GroupAzmoonsController : BaseAdminController
    {
        // GET: GroupAzmoons
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.GroupAzmoonBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                GroupAzmoonData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<GroupAzmoon> GroupAzmoonColumns { get; private set; } = GetGroupAzmoonColumns();
        public static GridColumnModelList<GroupAzmoon> GetGroupAzmoonColumns()
        {
            if (GroupAzmoonColumns == null)
            {
                GroupAzmoonColumns = new GridColumnModelList<GroupAzmoon>();
                GroupAzmoonColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                GroupAzmoonColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                GroupAzmoonColumns.Add(x => x.Name).SetCaption("نام").SetWidth("300");
            }
            return GroupAzmoonColumns;
        }
        // GET: GroupAzmoons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupAzmoon user = await GroupAzmoonBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: GroupAzmoons/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            GroupAzmoon user = await GroupAzmoonBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: GroupAzmoons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroupAzmoon user)
        {
            await GroupAzmoonBiz.Instance.Save(user);
            return RedirectToAction("Index", "GroupAzmoons");
        }



        // GET: GroupAzmoons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupAzmoon user = await GroupAzmoonBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: GroupAzmoons/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await GroupAzmoonBiz.Instance.Remove(id);
            return RedirectToAction("Index", "GroupAzmoons");
        }
    }
}
