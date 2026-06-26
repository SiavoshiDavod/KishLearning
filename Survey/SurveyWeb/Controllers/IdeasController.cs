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
    public class IdeasController : BaseAdminController
    {


        // GET: Ideas
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.Idea);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid, int cartableId)
        {
            var list = Biz.IdeaBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
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
        public static GridColumnModelList<Idea> IdeaColumns { get; private set; } = GetIdeaColumns();
        public static GridColumnModelList<Idea> GetIdeaColumns()
        {
            if (IdeaColumns == null)
            {
                IdeaColumns = new GridColumnModelList<Idea>();
                IdeaColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                IdeaColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                IdeaColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                IdeaColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                IdeaColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                IdeaColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                IdeaColumns.Add(x => x.Mobile).SetCaption("تلفن همراه").SetWidth("100");
                IdeaColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
                IdeaColumns.Add(x => x.Problem).SetCaption("ضرورت/مشکل").SetWidth("100");
                IdeaColumns.Add(x => x.Proposal).SetCaption("پیشنهاد/راهکار").SetWidth("100");
                IdeaColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("100");
                IdeaColumns.Add(x => x.TrackingCode).SetCaption("کدرهگیری").SetWidth("100");
            }
            return IdeaColumns;
        }
        // GET: Ideas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea cartable = await IdeaBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return PartialView(cartable);
        }

        // GET: Ideas/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Idea cartable = await IdeaBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Idea user)
        {
            await IdeaBiz.Instance.Save(user);
            return RedirectToAction("Index", "Ideas");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea user = await IdeaBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await IdeaBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Ideas");
        }
    }
}
