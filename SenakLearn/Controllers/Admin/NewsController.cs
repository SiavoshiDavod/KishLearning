using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{

    public class NewsController : BaseAdminController
    {


        // GET: News
        public ActionResult Index()
        {
            var model = new List<SenakLearn.Models.News>();
            return View(model);
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.NewsBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                NewsData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<News> NewsColumns { get; private set; } = GetNewsColumns();
        public static GridColumnModelList<News> GetNewsColumns()
        {
            if (NewsColumns == null)
            {
                NewsColumns = new GridColumnModelList<News>();
                NewsColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                NewsColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                NewsColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("200");
                NewsColumns.Add(x => x.Summary).SetCaption("خلاصه").SetWidth("500");
            }
            return NewsColumns;
        }
        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News cartable =  NewsBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: News/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                var model = new News();
                return View(model);
            }
            Author cartable = AuthorBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                var model = new News();
                return View(model);
            }
            return View(cartable);
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public ActionResult Create(News user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.ImageUrl = SaveFile(File, pathFile.News);
            }
            else
            {
                user.ImageUrl = EditFile(File, pathFile.News, user.ImageUrl);
            }
             NewsBiz.Instance.Save(user);
            return RedirectToAction("Index", "News");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News user =  NewsBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             NewsBiz.Instance.Remove(id);
            return RedirectToAction("Index", "News");
        }
    }

}