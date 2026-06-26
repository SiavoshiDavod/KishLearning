using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;
using SurveyWeb.Models.CheckList;
using SurveyWeb.Biz.CheckList;

namespace SurveyWeb.Controllers.CheckList
{
    public class CheckListController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public static GridColumnModelList<SurveyWeb.Models.CheckList.CheckList> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<Models.CheckList.CheckList> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<Models.CheckList.CheckList>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("120");
                Columns.Add(x => x.Name).SetCaption("نام").SetWidth("400");
                Columns.Add(x => x.IsActive).SetCaption("فعال").SetWidth("400").SetColumnRenderer(new CheckboxColumnRenderer());
            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = CheckListBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                CartableData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await CheckListBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            var item = await CheckListBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return View();
            }
            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> Create(Models.CheckList.CheckList model)
        {
            await CheckListBiz.Instance.Save(model);
            return RedirectToAction("Index", "CheckList");
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await CheckListBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await CheckListBiz.Instance.Remove(id);
            return RedirectToAction("Index", "CheckList");
        }
    }
}