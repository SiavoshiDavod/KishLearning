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
    public class AuthorsController : BaseAdminController
    {


        // GET: Authors
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.AuthorBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AuthorData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Author> AuthorColumns { get; private set; } = GetAuthorColumns();
        public static GridColumnModelList<Author> GetAuthorColumns()
        {
            if (AuthorColumns == null)
            {
                AuthorColumns = new GridColumnModelList<Author>();
                AuthorColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AuthorColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                AuthorColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                AuthorColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                AuthorColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                AuthorColumns.Add(x => x.Mobile).SetCaption("تلفن همراه").SetWidth("100");
                AuthorColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
            }
            return AuthorColumns;
        }
        // GET: Authors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author cartable = await AuthorBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Authors/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Author cartable = await AuthorBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Author user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.ImageUrl = SaveFile(File, pathFile.Author);
            }
            else
            {
                user.ImageUrl = EditFile(File, pathFile.Author, user.ImageUrl);
            }
            await AuthorBiz.Instance.Save(user);
            return RedirectToAction("Index", "Authors");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author user = await AuthorBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AuthorBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Authors");
        }
    }
}

