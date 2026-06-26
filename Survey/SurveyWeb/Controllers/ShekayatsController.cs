using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;
using SurveyWeb.Models.Resturan;

namespace SurveyWeb.Controllers
{
    public class ShekayatsController : BaseAdminController
    {
        // GET: Shekayats
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.Shekayat);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid, int cartableId)
        {
            var list = Biz.ShekayatBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
            foreach (var item in list)
            {
                item.Description = item.Description?.Length > 50 ? item.Description.Substring(0,50).Replace("\r\n", " ") + " ..." : item.Description?.Replace("\r\n", " ");
            }
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
        public static GridColumnModelList<Shekayat> ShekayatColumns { get; private set; } = GetShekayatColumns();
        public static GridColumnModelList<Shekayat> GetShekayatColumns()
        {
            if (ShekayatColumns == null)
            {
                ShekayatColumns = new GridColumnModelList<Shekayat>();
                ShekayatColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ShekayatColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                ShekayatColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                ShekayatColumns.Add(x => x.Resturant.Code).SetCaption("کد مرکز").SetWidth("50");
                ShekayatColumns.Add(x => x.Resturant.Name).SetCaption("نام مرکزپذیرایی").SetWidth("100");
                ShekayatColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                ShekayatColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                ShekayatColumns.Add(x => x.Mobile).SetCaption("تلفن همراه").SetWidth("100");
                ShekayatColumns.Add(x => x.Title).SetCaption("عنوان شکایت").SetWidth("100");
                ShekayatColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("200");
                ShekayatColumns.Add(x => x.TrackingCode).SetCaption("کدرهگیری").SetWidth("100");
            }
            return ShekayatColumns;
        }
        // GET: Shekayats/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shekayat cartable = await ShekayatBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return PartialView(cartable);
        }

        // GET: Shekayats/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Shekayat cartable = await ShekayatBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Shekayats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Shekayat user)
        {
            await ShekayatBiz.Instance.Save(user);
            return RedirectToAction("Index", "Shekayats");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shekayat user = await ShekayatBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Shekayats/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ShekayatBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Shekayats");
        }
    }
}
