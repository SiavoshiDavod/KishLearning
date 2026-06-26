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

namespace SurveyWeb.Controllers.CheckList
{
    public class ComplaintCheckListItemController : BaseAdminController
    {
        public ActionResult Index(int complaintCheckListId)
        {
            var parent = ComplaintCheckListBiz.Instance.FindFull(complaintCheckListId);
            var model = new ComplaintCheckListItem()
            {
                ComplaintCheckListId = complaintCheckListId,
                CheckListName = parent.CheckListName,
                ResturantName = parent.ResturantName
            };
            return View(model);
        }
        public static GridColumnModelList<ComplaintCheckListItemWrapper> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<ComplaintCheckListItemWrapper> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<ComplaintCheckListItemWrapper>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50").SetSortable(false);
                Columns.Add(x => x.ComplaintCheckListId).SetHidden(true);
                Columns.Add(x => x.CheckListItemId).SetHidden(true);
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("100").SetSortable(false);
                Columns.Add(x => x.CheckListItemName).SetCaption("موضوع بازدید").SetWidth("200").SetSortable(false);
                Columns.Add(x => x.CheckListItemGroupName).SetCaption("عنوان ارزیابی").SetWidth("200").SetSortable(false);
                Columns.Add(x => x.ComplaintCheckListItemId).SetHidden(true);
                Columns.Add(x => x.IsYesNo).SetHidden(true);
                Columns.Add(x => x.IsGoodMidBad).SetHidden(true);
                Columns.Add(x => x.IsHasItDontHave).SetHidden(true);
                Columns.Add(x => x.ValueItem).SetHidden(true);
            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid, int complaintCheckListId)
        {
            var list = ComplaintCheckListItemBiz.Instance.GetItemsGrid(grid, complaintCheckListId);
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
            var item = await ComplaintCheckListItemBiz.Instance.Get(id.Value);
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
            var item = await ComplaintCheckListItemBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return View();
            }
            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> Create(ComplaintCheckListItem model)
        {
            await ComplaintCheckListItemBiz.Instance.Save(model);
            return RedirectToAction("Index", "ComplaintCheckListItem");
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await ComplaintCheckListItemBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ComplaintCheckListItemBiz.Instance.Remove(id);
            return RedirectToAction("Index", "ComplaintCheckListItem");
        }
        [HttpPost]

        public async Task<ActionResult> UpdateComCheckListItem(List<ComplaintCheckListItemWrapper> things)
        {
            await ComplaintCheckListItemBiz.Instance.UpdateComCheckListItem(things);
            return Json(new {message="OK" },JsonRequestBehavior.AllowGet);
        }
    }
}