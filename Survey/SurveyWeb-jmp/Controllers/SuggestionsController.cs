using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;

namespace SurveyWeb.Controllers
{
    public class SuggestionsController : BaseAdminController
    {
        // GET: Suggestions
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.Suggestion);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid,int cartableId)
        {
            var list = Biz.SuggestionBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                CartableId = cartableId
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Suggestion> SuggestionColumns { get; private set; } = GetSuggestionColumns();
        public static GridColumnModelList<Suggestion> GetSuggestionColumns()
        {
            if (SuggestionColumns == null)
            {
                SuggestionColumns = new GridColumnModelList<Suggestion>();
                SuggestionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                SuggestionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                SuggestionColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                SuggestionColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                SuggestionColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                SuggestionColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                SuggestionColumns.Add(x => x.Mobile).SetCaption("تلفن همراه").SetWidth("100");
                SuggestionColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
                SuggestionColumns.Add(x => x.Title).SetCaption("عنوان ایده").SetWidth("100");
                SuggestionColumns.Add(x => x.Proposal).SetCaption("کلیات طرح").SetWidth("100");
                SuggestionColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("100");
                SuggestionColumns.Add(x => x.TrackingCode).SetCaption("کدرهگیری").SetWidth("100");
            }
            return SuggestionColumns;
        }
        // GET: Suggestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion cartable = await SuggestionBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return PartialView(cartable);
        }

        // GET: Suggestions/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Suggestion cartable = await SuggestionBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Suggestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Suggestion user)
        {
            await SuggestionBiz.Instance.Save(user);
            return RedirectToAction("Index", "Suggestions");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion user = await SuggestionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await SuggestionBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Suggestions");
        }
    }
}
