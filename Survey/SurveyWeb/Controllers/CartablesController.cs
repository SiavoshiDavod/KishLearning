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
    public class CartablesController : BaseAdminController
    {
        

        // GET: Cartables
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.CartableBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<Cartable> CartableColumns { get; private set; } = GetCartableColumns();
        public static GridColumnModelList<Cartable> GetCartableColumns()
        {
            if (CartableColumns == null)
            {
                CartableColumns = new GridColumnModelList<Cartable>();
                CartableColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                CartableColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                CartableColumns.Add(x => x.Name).SetCaption("نام").SetWidth("200");
                CartableColumns.Add(x => x.CartableTypeName).SetCaption("نوع کارتابل").SetWidth("200");
                CartableColumns.Add(x => x.IsFirstState).SetCaption("مرحله اول").SetWidth("100");
                CartableColumns.Add(x => x.IsLastState).SetCaption("مرحله آخر").SetWidth("100");
                CartableColumns.Add(x => x.Order).SetCaption("ترتیب").SetWidth("100");
            }
            return CartableColumns;
        }
        // GET: Cartables/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartable cartable = await CartableBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Cartables/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Cartable cartable = await CartableBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Cartables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cartable user)
        {
            await CartableBiz.Instance.Save(user);
            return RedirectToAction("Index", "Cartables");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartable user = await CartableBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Cartables/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await CartableBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Cartables");
        }
    }
}
