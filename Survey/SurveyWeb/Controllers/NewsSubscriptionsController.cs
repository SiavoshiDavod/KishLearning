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
    public class NewsSubscriptionsController : BaseAdminController
    {


        // GET: NewsSubscriptions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.NewsSubscriptionBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                NewsSubscriptionData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<NewsSubscription> NewsSubscriptionColumns { get; private set; } = GetNewsSubscriptionColumns();
        public static GridColumnModelList<NewsSubscription> GetNewsSubscriptionColumns()
        {
            if (NewsSubscriptionColumns == null)
            {
                NewsSubscriptionColumns = new GridColumnModelList<NewsSubscription>();
                NewsSubscriptionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                NewsSubscriptionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                NewsSubscriptionColumns.Add(x => x.Email).SetCaption("Email").SetWidth("300");
            }
            return NewsSubscriptionColumns;
        }
        // GET: NewsSubscriptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsSubscription cartable = await NewsSubscriptionBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: NewsSubscriptions/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            NewsSubscription cartable = await NewsSubscriptionBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: NewsSubscriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewsSubscription user, System.Web.HttpPostedFileBase File)
        {
            await NewsSubscriptionBiz.Instance.Save(user);
            return RedirectToAction("Index", "NewsSubscriptions");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsSubscription user = await NewsSubscriptionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: NewsSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await NewsSubscriptionBiz.Instance.Remove(id);
            return RedirectToAction("Index", "NewsSubscriptions");
        }
    }
}

