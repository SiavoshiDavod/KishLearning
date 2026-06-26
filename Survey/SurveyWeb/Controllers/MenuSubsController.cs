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
    public class MenuSubsController : BaseAdminController
    {


        // GET: MenuSubs
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> IndexByMenuId(int id)
        {
            ViewBag.MenuName = (await Biz.MenuBiz.Instance.Get(id))?.Title;
            return PartialView(id);
        }

        public ActionResult LoadList(GridSettings grid, int? id)
        {
            var list = Biz.MenuSubBiz.Instance.GetAllPagedListByMenuId(grid, id);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                MenuSubData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<MenuSub> MenuSubColumns { get; private set; } = GetMenuSubColumns();
        public static GridColumnModelList<MenuSub> GetMenuSubColumns()
        {
            if (MenuSubColumns == null)
            {
                MenuSubColumns = new GridColumnModelList<MenuSub>();
                MenuSubColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                MenuSubColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                MenuSubColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
            }
            return MenuSubColumns;
        }
        // GET: MenuSubs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuSub cartable = await MenuSubBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: MenuSubs/Create
        public async Task<ActionResult> Create(int? id, int? MenuId,bool isMenuId=false)
        {

            if (id == null && MenuId == null)
            {
                return View();
            }
            if (id == null && MenuId != null)
            {
                return View(new MenuSub() { MenuId = MenuId.Value, isMenuId = true });
            }
            MenuSub cartable = await MenuSubBiz.Instance.Get(id.Value);

            if (isMenuId)
            {
                cartable.isMenuId = true;
            }
            return View(cartable);
        }

        // POST: MenuSubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuSub user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.Image = SaveFile(File, pathFile.MenuSub);
            }
            else
            {
                user.Image = EditFile(File, pathFile.MenuSub, user.Image);
            }
            await MenuSubBiz.Instance.Save(user);
            if (user.isMenuId)
            {
                return RedirectToAction("IndexByMenuId", "MenuSubs", new { id = user.MenuId });
            }
            return RedirectToAction("Index", "MenuSubs");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuSub user = await MenuSubBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: MenuSubs/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await MenuSubBiz.Instance.Remove(id);
            return RedirectToAction("Index", "MenuSubs");
        }
        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            await MenuSubBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}

