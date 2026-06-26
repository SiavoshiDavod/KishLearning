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
using SurveyWeb.Models.wrapper;
using System.Collections.Generic;
using System.Linq;

namespace SurveyWeb.Controllers.CheckList
{
    public class CheckListItemController : BaseAdminController
    {
        public ActionResult Index(int checkListId)
        {
            var checkListItem = new CheckListItem() { CheckListId = checkListId };

            var checkListGroupList = CheckListGroupBiz.Instance.FindAll();
            ViewBag.CheckListGroupId = checkListGroupList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            return View(checkListItem);
        }
        public static GridColumnModelList<CheckListItemWrapper> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<CheckListItemWrapper> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<CheckListItemWrapper>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                Columns.Add(x => x.CheckListId).SetHidden(true);
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                Columns.Add(x => x.CheckListName).SetCaption("ارزیابی").SetWidth("200");
                Columns.Add(x => x.CheckListGroupName).SetCaption("عنوان ارزیابی").SetWidth("200");
                Columns.Add(x => x.Name).SetCaption("نام").SetWidth("200");
                Columns.Add(x => x.CheckListItemTypeName).SetCaption("نوع ارزیابی").SetWidth("200");
            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid, int checkListId)
        {
            var list = CheckListItemBiz.Instance.GetItemsGrid(grid, checkListId);
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
            var item = await CheckListItemBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public async Task<ActionResult> Create(int checkListId, int? id)
        {
            var checkListGroupList = CheckListGroupBiz.Instance.FindAll();
            ViewBag.CheckListGroupId = checkListGroupList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var checkList_ = CheckListBiz.Instance.Find(checkListId);
            if (id == null)
            {
                var model = new CheckListItem { CheckListId = checkListId, CheckListName = checkList_.Name };
                return View(model);
            }
            var item = await CheckListItemBiz.Instance.Get(id.Value);
            if (item == null)
            {
                var model = new CheckListItem { CheckListId = checkListId, CheckListName = checkList_.Name };
                return View(model);
            }
            item.CheckListName = checkList_.Name;
            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CheckListItem model)
        {
            var checkListGroupList = CheckListGroupBiz.Instance.FindAll();
            ViewBag.CheckListGroupId = checkListGroupList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            await CheckListItemBiz.Instance.Save(model);
            var checkList = CheckListBiz.Instance.Find(model.CheckListId);
            var newModel = new CheckListItem { CheckListId = model.CheckListId, CheckListName = checkList.Name };
            return RedirectToAction("Index", "CheckListItem", newModel);
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await CheckListItemBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var item = CheckListItemBiz.Instance.Get(id);
            await CheckListItemBiz.Instance.Remove(id);
            var checkListGroupList = CheckListGroupBiz.Instance.FindAll();
            ViewBag.CheckListGroupId = checkListGroupList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var checkList = CheckListBiz.Instance.Find(item.Result.CheckListId);
            var newModel = new CheckListItem { CheckListId = checkList.Id, CheckListName = checkList.Name };
            return RedirectToAction("Index", "CheckListItem", newModel);
        }
    }
}