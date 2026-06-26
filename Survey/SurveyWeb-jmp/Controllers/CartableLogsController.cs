using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Controllers
{
    public class CartableLogsController : BaseAdminController
    {


        // GET: CartableLogLogs
        public ActionResult Index(int id, CartableType cartableType)
        {
            return PartialView(new Tuple<int, CartableType>(id,cartableType));
        }
        public ActionResult LoadList(GridSettings grid, int id, CartableType cartableType)
        {
            var list = Biz.CartableLogBiz.Instance.GetAllPagedListByEntityId(grid,id,cartableType);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyEntityData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        // GET: CartableLogLogs/Details/5
        public static GridColumnModelList<CartableLogVM> CartableLogColumns { get; private set; } = GetCartableLogColumns();
        public static GridColumnModelList<CartableLogVM> GetCartableLogColumns()
        {
            if (CartableLogColumns == null)
            {
                CartableLogColumns = new GridColumnModelList<CartableLogVM>();
                CartableLogColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                CartableLogColumns.Add(x => x.From).SetCaption("از کارتابل").SetWidth("100");
                CartableLogColumns.Add(x => x.To).SetCaption("به کارتابل").SetWidth("100");
                CartableLogColumns.Add(x => x.User).SetCaption("کاربر").SetWidth("150");
                CartableLogColumns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ").SetWidth("100");
                CartableLogColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("200");
            }
            return CartableLogColumns;
        }
        // GET: CartableLogs/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CartableLog cartable = await CartableLogBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartable);
        //}

        // GET: CartableLogs/Create
        //public async Task<ActionResult> Create(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View();
        //    }
        //    CartableLog cartable = await CartableLogBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return View();
        //    }
        //    return View(cartable);
        //}

        // POST: CartableLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(CartableLog user)
        {
            user.UserId = Current_UserId;
            await CartableLogBiz.Instance.Save(user);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CartableLog user = await CartableLogBiz.Instance.Get(id.Value);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: CartableLogs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    await CartableLogBiz.Instance.Remove(id);
        //    return RedirectToAction("Index", "CartableLogs");
        //}
    }
}
