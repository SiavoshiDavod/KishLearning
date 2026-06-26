using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;
using System.Linq;

namespace SurveyWeb.Controllers
{
    public class NewsController : BaseAdminController
    {


        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.NewsBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.Select(x=>new { x.Id,x.Title,x.Summary}).ToArray(),
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
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News cartable = await NewsBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: News/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            News cartable = await NewsBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(News user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.ImageUrl = SaveFile(File, pathFile.News);
            }
            else
            {
                user.ImageUrl = EditFile(File, pathFile.News, user.ImageUrl);
            }
            await NewsBiz.Instance.Save(user);
            return RedirectToAction("Index", "News");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News user = await NewsBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await NewsBiz.Instance.Remove(id);
            return RedirectToAction("Index", "News");
        }
    }
}

