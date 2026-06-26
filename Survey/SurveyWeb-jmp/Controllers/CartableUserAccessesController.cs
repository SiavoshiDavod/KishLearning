using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Controllers
{
    public class CartableUserAccessesController : BaseAdminController
    {


        // GET: CartableUserAccessUserAccesses
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.CartableUserAccessBiz.Instance.GetAllPagedListVm(grid);
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
        public static GridColumnModelList<CartableUserAccessVm> CartableUserAccessColumns { get; private set; } = GetCartableUserAccessColumns();
        public static GridColumnModelList<CartableUserAccessVm> GetCartableUserAccessColumns()
        {
            if (CartableUserAccessColumns == null)
            {
                CartableUserAccessColumns = new GridColumnModelList<CartableUserAccessVm>();
                CartableUserAccessColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                CartableUserAccessColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                CartableUserAccessColumns.Add(x => x.Cartable).SetCaption("کارتابل").SetWidth("300");
                CartableUserAccessColumns.Add(x => x.User).SetCaption("کاربر").SetWidth("300");
                CartableUserAccessColumns.Add(x => x.CartableTypeName).SetCaption("نوع کارتابل").SetWidth("200");
            }
            return CartableUserAccessColumns;
        }
        // GET: CartableUserAccesss/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartableUserAccess cartable = await CartableUserAccessBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: CartableUserAccesss/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            CartableUserAccess cartable = await CartableUserAccessBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: CartableUserAccesss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CartableUserAccess user)
        {
            await CartableUserAccessBiz.Instance.Save(user);
            return RedirectToAction("Index", "CartableUserAccesses");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartableUserAccess user = await CartableUserAccessBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: CartableUserAccesss/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await CartableUserAccessBiz.Instance.Remove(id);
            return RedirectToAction("Index", "CartableUserAccesses");
        }
    }
}
