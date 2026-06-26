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
using System.Linq;

namespace SurveyWeb.Controllers.CheckList
{
    public class ComplaintCheckListController : BaseAdminController
    {
        public ActionResult Index()
        {
            var checkListList = CheckListBiz.Instance.FindAll();
            ViewBag.CheckListId = checkListList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var resturantList = ResturantBiz.Instance.ResturantSelectList();
            ViewBag.ResturantId = resturantList;
            return View();
        }
        public static GridColumnModelList<ComplaintCheckListWrapper> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<ComplaintCheckListWrapper> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<ComplaintCheckListWrapper>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                Columns.Add(x => x.CheckListName).SetCaption("نام ارزیابی").SetWidth("200");
                Columns.Add(x => x.ComplaintDatePersian).SetCaption("تاریخ ارزیابی").SetWidth("200");
                Columns.Add(x => x.ComplaintTimePersian).SetCaption("زمان ارزیابی").SetWidth("200");
                Columns.Add(x => x.ResturantName).SetCaption("نام واحد").SetWidth("200");
                Columns.Add(x => x.ModirName).SetCaption("مدیر واحد").SetWidth("200");
                Columns.Add(x => x.UserComplaintName).SetCaption("کارشناس نظارت").SetWidth("200");
            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = ComplaintCheckListBiz.Instance.GetItemsGrid(grid);
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
            var checkListList = CheckListBiz.Instance.FindAll();
            ViewBag.CheckListId = checkListList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var resturantList = ResturantBiz.Instance.ResturantSelectList();
            ViewBag.ResturantId = resturantList;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await ComplaintCheckListBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public async Task<ActionResult> Create(int? id)
        {
            
            var checkListList = CheckListBiz.Instance.FindAll();
            ViewBag.CheckListId = checkListList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var resturantList = ResturantBiz.Instance.ResturantSelectList();
            ViewBag.ResturantId = resturantList;
            if (id == null)
            {
                return View();
            }
            var item = await ComplaintCheckListBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return View();
            }
            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> Create(ComplaintCheckList model)
        {
            if (model.UserComplaintId == null)
                model.UserComplaintId = Current_UserId;
            if(model.ComplaintDate == null || model.ComplaintDate==DateTime.MinValue)
                model.ComplaintDate = model.ComplaintDatePersian.ToGregorianDate();

            await ComplaintCheckListBiz.Instance.Save(model);
            var checkListList = CheckListBiz.Instance.FindAll();
            ViewBag.CheckListId = checkListList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var resturantList = ResturantBiz.Instance.ResturantSelectList();
            ViewBag.ResturantId = resturantList;
            return RedirectToAction("Index", "ComplaintCheckList");
        }
        public async Task<ActionResult> Delete(int? id)
        {
            var checkListList = CheckListBiz.Instance.FindAll();
            ViewBag.CheckListId = checkListList.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var resturantList = ResturantBiz.Instance.ResturantSelectList();
            ViewBag.ResturantId = resturantList;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await ComplaintCheckListBiz.Instance.Get(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ComplaintCheckListBiz.Instance.Remove(id);
            return RedirectToAction("Index", "ComplaintCheckList");
        }
    }
}