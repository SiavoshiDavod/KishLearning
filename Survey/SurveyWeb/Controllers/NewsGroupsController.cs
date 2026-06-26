using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;

namespace SurveyWeb.Controllers
{
    public class NewsGroupsController : BaseAdminController
    {


        // GET: NewsGroup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.NewsGroupBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                NewsGroupsData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<NewsGroup> NewsGroupsColumns { get; private set; } = GetNewsGroupsColumns();
        public static GridColumnModelList<NewsGroup> GetNewsGroupsColumns()
        {
            if (NewsGroupsColumns == null)
            {
                NewsGroupsColumns = new GridColumnModelList<NewsGroup>();
                NewsGroupsColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                NewsGroupsColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                NewsGroupsColumns.Add(x => x.Title).SetCaption("سوال").SetWidth("300");
            }
            return NewsGroupsColumns;
        }
        // GET: NewsGroup/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup cartable = await NewsGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: NewsGroup/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            NewsGroup cartable = await NewsGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: NewsGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewsGroup user, System.Web.HttpPostedFileBase File)
        {
            await NewsGroupBiz.Instance.Save(user);
            return RedirectToAction("Index", "NewsGroups");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup user = await NewsGroupBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: NewsGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await NewsGroupBiz.Instance.Remove(id);
            return RedirectToAction("Index", "NewsGroups");
        }
    }
}

