using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup cartable =  NewsGroupBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: NewsGroup/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            NewsGroup cartable =  NewsGroupBiz.Instance.Get(id.Value);
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
        public ActionResult Create(NewsGroup user, System.Web.HttpPostedFileBase File)
        {
             NewsGroupBiz.Instance.Save(user);
            return RedirectToAction("Index", "NewsGroups");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup user =  NewsGroupBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: NewsGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             NewsGroupBiz.Instance.Remove(id);
            return RedirectToAction("Index", "NewsGroups");
        }
    }

}