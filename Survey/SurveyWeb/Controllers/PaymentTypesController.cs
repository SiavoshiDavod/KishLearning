using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.Models.Resturan;

namespace SurveyWeb.Controllers
{
    public class PaymentTypesController : BaseAdminController
    {

        // GET: PaymentTypes
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.PaymentTypeBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                PaymentTypeData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<PaymentType> PaymentTypeColumns { get; private set; } = GetPaymentTypeColumns();
        public static GridColumnModelList<PaymentType> GetPaymentTypeColumns()
        {
            if (PaymentTypeColumns == null)
            {
                PaymentTypeColumns = new GridColumnModelList<PaymentType>();
                PaymentTypeColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                PaymentTypeColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                PaymentTypeColumns.Add(x => x.Price).SetCaption("مبلغ(ريال)").SetColumnRenderer(new NumberColumnRenderer()).SetSearchable(true).SetCellType(GridCellType.DECIMAL).SetWidth("100");
                PaymentTypeColumns.Add(x => x.PaymentTypeEnumName).SetCaption("نوع").SetWidth("100");
                PaymentTypeColumns.Add(x => x.ResturantTypeName).SetCaption("شرح خدمات").SetWidth("100");
                PaymentTypeColumns.Add(x => x.Desc).SetCaption("توضيحات").SetWidth("200");
                PaymentTypeColumns.Add(x => x.Archive).SetCaption("آرشيو شده").SetWidth("200");
            }
            return PaymentTypeColumns;
        }
        // GET: PaymentTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType cartable = await PaymentTypeBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: PaymentTypes/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            PaymentType cartable = await PaymentTypeBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: PaymentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PaymentType user)
        {
            await PaymentTypeBiz.Instance.Save(user);
            return RedirectToAction("Index", "PaymentTypes");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType user = await PaymentTypeBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: PaymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await PaymentTypeBiz.Instance.SetArchive(id);
            return RedirectToAction("Index", "PaymentTypes");
        }
    }
}

