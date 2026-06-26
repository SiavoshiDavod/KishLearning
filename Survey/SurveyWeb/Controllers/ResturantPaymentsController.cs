using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models.Resturan;
using SurveyWeb.Biz;
using MVC.Controls.Grid;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Controllers
{
    public class ResturantPaymentsController : BaseAdminController
    {

        // GET: ResturantPayments
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid, bool IsAccepted = false)
        {
            var list = Biz.ResturantPaymentBiz.Instance.GetAllPagedList(grid, IsAccepted);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                ResturantPaymentData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<ResturantPayment> ResturantPaymentColumns { get; private set; } = GetResturantPaymentColumns();
        public static GridColumnModelList<ResturantPayment> GetResturantPaymentColumns()
        {
            if (ResturantPaymentColumns == null)
            {
                ResturantPaymentColumns = new GridColumnModelList<ResturantPayment>();
                ResturantPaymentColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ResturantPaymentColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.PaymentTypeEnumName).SetCaption("نوع").SetWidth("200");
                ResturantPaymentColumns.Add(x => x.Resturant.Name).SetCaption("نام مرکزپذیرایی").SetWidth("200");
                ResturantPaymentColumns.Add(x => x.Price).SetCaption("مبلغ(ريال)").SetColumnRenderer(new NumberColumnRenderer()).SetSearchable(true).SetCellType(GridCellType.DECIMAL).SetWidth("100");
                ResturantPaymentColumns.Add(x => x.PaymentDateShamsi).SetCaption("تاريخ پرداخت").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.VarizKonande).SetCaption("واريز کننده").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.RefId).SetCaption("شماره فیش/کد رهگیری").SetWidth("100");
                ResturantPaymentColumns.Add(x => x.AdminDescription).SetCaption("توضیحات").SetWidth("200");
            }
            return ResturantPaymentColumns;
        }
        // GET: ResturantPayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResturantPayment cartable = await ResturantPaymentBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: ResturantPayments/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            ResturantPayment cartable = await ResturantPaymentBiz.Instance.GetInclude(new ResturantPayment() { Id = id.Value }, "Resturant");
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        public async Task<ActionResult> Accept(int id, string desc, bool accept = true)
        {
            await Biz.ResturantPaymentBiz.Instance.AcceptResturantPayment(id, accept, true, desc);
            return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ResturantPayment user)
        {
            await ResturantPaymentBiz.Instance.Save(user);
            return RedirectToAction("Index", "ResturantPayments");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResturantPayment user = await ResturantPaymentBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: ResturantPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ResturantPaymentBiz.Instance.Remove(id);
            return RedirectToAction("Index", "ResturantPayments");
        }
    }
}

