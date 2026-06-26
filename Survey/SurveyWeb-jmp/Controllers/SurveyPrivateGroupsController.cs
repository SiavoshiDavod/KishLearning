using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Models.Security;
using SurveyWeb.Biz;

namespace SurveyWeb.Controllers
{
    public class SurveyPrivateGroupsController : BaseAdminController
    {


        // GET: SurveyPrivateGroups
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.SurveyPrivateGroupBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyPrivateGroupData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyPrivateGroup> SurveyPrivateGroupColumns { get; private set; } = GetSurveyPrivateGroupColumns();
        public static GridColumnModelList<SurveyPrivateGroup> GetSurveyPrivateGroupColumns()
        {
            if (SurveyPrivateGroupColumns == null)
            {
                SurveyPrivateGroupColumns = new GridColumnModelList<SurveyPrivateGroup>();
                SurveyPrivateGroupColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                SurveyPrivateGroupColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                SurveyPrivateGroupColumns.Add(x => x.Name).SetCaption("عنوان").SetWidth("300");
            }
            return SurveyPrivateGroupColumns;
        }
        // GET: SurveyPrivateGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyPrivateGroup cartable = await SurveyPrivateGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: SurveyPrivateGroups/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            SurveyPrivateGroup cartable = await SurveyPrivateGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: SurveyPrivateGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SurveyPrivateGroup user)
        {
            await SurveyPrivateGroupBiz.Instance.Save(user);
            return RedirectToAction("Index", "SurveyPrivateGroups");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyPrivateGroup user = await SurveyPrivateGroupBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SurveyPrivateGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await SurveyPrivateGroupBiz.Instance.Remove(id);
            return RedirectToAction("Index", "SurveyPrivateGroups");
        }
    }
}

