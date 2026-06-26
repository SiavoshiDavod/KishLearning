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
    public class CheckListGroupController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public static GridColumnModelList<CheckListGroup> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<CheckListGroup> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<CheckListGroup>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                Columns.Add(x => x.Name).SetCaption("نام").SetWidth("200");
            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = CheckListGroupBiz.Instance.GetAllPagedList(grid);
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
            var item = await CheckListGroupBiz.Instance.Get(id.Value);
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
            var item = await CheckListGroupBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return View();
            }
            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CheckListGroup model)
        {
            await CheckListGroupBiz.Instance.Save(model);
            return RedirectToAction("Index", "CheckListGroup");
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await CheckListGroupBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await CheckListGroupBiz.Instance.Remove(id);
            return RedirectToAction("Index", "CheckListGroup");
        }
    }
}