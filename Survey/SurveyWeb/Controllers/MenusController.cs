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
    public class MenusController : BaseAdminController
    {


        // GET: Menus
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.MenuBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                MenuData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Menu> MenuColumns { get; private set; } = GetMenuColumns();
        public static GridColumnModelList<Menu> GetMenuColumns()
        {
            if (MenuColumns == null)
            {
                MenuColumns = new GridColumnModelList<Menu>();
                MenuColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                MenuColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                MenuColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
            }
            return MenuColumns;
        }
        // GET: Menus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu cartable = await MenuBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Menus/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Menu cartable = await MenuBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Menu user, System.Web.HttpPostedFileBase File)
        {
            await MenuBiz.Instance.Save(user);
            return RedirectToAction("Index", "Menus");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu user = await MenuBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await MenuBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Menus");
        }
    }
}

