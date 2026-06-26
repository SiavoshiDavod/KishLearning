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
    public class PostController : BaseAdminController
    {


        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = PostBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<Post> PostColumns { get; private set; } = GetColumns();
        public static GridColumnModelList<Post> GetColumns()
        {
            if (PostColumns == null)
            {
                PostColumns = new GridColumnModelList<Post>();
                PostColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                PostColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                PostColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
            }
            return PostColumns;
        }
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post =  PostBiz.Instance.Get(id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Roles/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View(new Post());
            }
            var post = PostBiz.Instance.Get(id.Value);
            if (post == null)
            {
                return View(new Post());
            }
            return View(post);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            PostBiz.Instance.Save(post);
            return RedirectToAction("Index", "Post");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = PostBiz.Instance.Get(id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Post");
        }
    }
}

