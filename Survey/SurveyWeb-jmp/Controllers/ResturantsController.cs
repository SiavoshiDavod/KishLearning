using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class ResturantsController : BaseAdminController
    {
        // GET: Resturants
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.Resturant);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid, int cartableId)
        {
            var list = Biz.ResturantBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
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
        public static GridColumnModelList<Resturant> ResturantColumns { get; private set; } = GetResturantColumns();
        public static GridColumnModelList<Resturant> GetResturantColumns()
        {
            if (ResturantColumns == null)
            {
                ResturantColumns = new GridColumnModelList<Resturant>();
                ResturantColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ResturantColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                ResturantColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                ResturantColumns.Add(x => x.Code).SetCaption("کد مرکز").SetWidth("100");
                ResturantColumns.Add(x => x.Name).SetCaption("نام مرکز").SetWidth("100");
                ResturantColumns.Add(x => x.Manager).SetCaption("مدیر").SetWidth("100");
                ResturantColumns.Add(x => x.SalonManager).SetCaption("مدیر سالن").SetWidth("100");
                ResturantColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
                ResturantColumns.Add(x => x.Beneficiary).SetCaption("بهره بردار").SetWidth("100");
                ResturantColumns.Add(x => x.Owner).SetCaption("مالک").SetWidth("100");
            }
            return ResturantColumns;
        }
        // GET: Resturants/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resturant cartable = await ResturantBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return PartialView(cartable);
        }

        // GET: Resturants/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Resturant cartable = await ResturantBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Resturants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Resturant user)
        {
            if (user.UserId<=0)
            {
                user.UserId = Current_UserId;
            }
            await ResturantBiz.Instance.Save(user);
            return RedirectToAction("Index", "Resturants");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resturant user = await ResturantBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Resturants/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ResturantBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Resturants");
        }
    }
}
