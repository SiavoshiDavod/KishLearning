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
    public class CartableRelationsController : BaseAdminController
    {


        // GET: CartableRelationRelations
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.CartableRelationBiz.Instance.GetAllPagedListVm(grid);
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
        public static GridColumnModelList<CartableLogVM> CartableRelationColumns { get; private set; } = GetCartableRelationColumns();
        public static GridColumnModelList<CartableLogVM> GetCartableRelationColumns()
        {
            if (CartableRelationColumns == null)
            {
                CartableRelationColumns = new GridColumnModelList<CartableLogVM>();
                CartableRelationColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                CartableRelationColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                CartableRelationColumns.Add(x => x.From).SetCaption("از کارتابل").SetWidth("200");
                CartableRelationColumns.Add(x => x.To).SetCaption("به کارتابل").SetWidth("200");
                CartableRelationColumns.Add(x => x.CartableTypeName).SetCaption("نوع کارتابل").SetWidth("200");
            }
            return CartableRelationColumns;
        }
        // GET: CartableRelations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartableRelation cartable = await CartableRelationBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: CartableRelations/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            CartableRelation cartable = await CartableRelationBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: CartableRelations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CartableRelation user)
        {
            await CartableRelationBiz.Instance.Save(user);
            return RedirectToAction("Index", "CartableRelations");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartableRelation user = await CartableRelationBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: CartableRelations/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await CartableRelationBiz.Instance.Remove(id);
            return RedirectToAction("Index", "CartableRelations");
        }
    }
}
