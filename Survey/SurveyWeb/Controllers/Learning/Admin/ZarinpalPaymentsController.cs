using System;
using System.Linq;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;

namespace SenakLearn.Controllers
{
    public class ZarinpalPaymentsController : BaseAdminController
    {
        //private SWEntities db = new SWEntities();
        public ActionResult LoadList(GridSettings grid)
        {
            var list =Biz.zarinpalBiz.Instance.GetAllPagedList(grid);
            //var count = Biz.zarinpalBiz.Instance.Count;
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                UserData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }

        #region Get  Columns

        public static GridColumnModelList<ZarinpalPayment> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<ZarinpalPayment> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<ZarinpalPayment>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("0");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.StatusS).SetCaption("وضعیت").SetWidth("300");
                Columns.Add(x => x.RefId).SetCaption("کد رهگیری").SetCellType(GridCellType.INT).SetWidth("200");
                //Columns.Add(x => x.Autohority).SetCaption("نام خانوادگی").SetWidth("300");
                Columns.Add(x => x.Amount).SetCaption("هزینه(ریال) ").SetCellType(GridCellType.INT).SetWidth("100");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ تایید").SetWidth("100");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ").SetWidth("100");
                //_columns.Add(x => x.username).SetCaption("نوع خودرو").SetWidth("300");
                //_columns.Add(x => x.courseName).SetCaption("پلاک").SetWidth("300");
                //_columns.Add(x => x.onlineclassName).SetCaption("پلاک").SetWidth("300");
                //بابت چه کلاسی پول پرداخت شده جوین می خواد
            }
            return Columns;
        }
        #endregion Get  Columns
        // GET: ZarinpalPayments
        public ActionResult Index()
        {
            return View(/*Biz.zarinpalBiz.Instance.GetAll(x => x.Id != 0)*/);
        }

        // GET: ZarinpalPayments/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ZarinpalPayment zarinpalPayment = Biz.zarinpalBiz.Instance.Get(id??0);// db.ZarinpalPayments.Find(id);
            if (zarinpalPayment == null)
            {
                return HttpNotFound();
            }
            return View(zarinpalPayment);
        }

        // GET: ZarinpalPayments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZarinpalPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "Id,UserId,CourseId,OnlineClassId,Autohority,RefId,Amount,Status,CreatedDate,UpdateDate")] ZarinpalPayment zarinpalPayment)
        {
            if (ModelState.IsValid)
            {
                Biz.zarinpalBiz.Instance.Save(zarinpalPayment);
                //db.ZarinpalPayments.Add(zarinpalPayment);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(zarinpalPayment);
        }

        // GET: ZarinpalPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ZarinpalPayment zarinpalPayment = Biz.zarinpalBiz.Instance.Get(id ?? 0);// db.ZarinpalPayments.Find(id);
            if (zarinpalPayment == null)
            {
                return HttpNotFound();
            }
            return View(zarinpalPayment);
        }

        // POST: ZarinpalPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "Id,UserId,CourseId,OnlineClassId,Autohority,RefId,Amount,Status,CreatedDate,UpdateDate")] ZarinpalPayment zarinpalPayment)
        {
            if (ModelState.IsValid)
            {
                Biz.zarinpalBiz.Instance.Save(zarinpalPayment);
                //db.Entry(zarinpalPayment).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(zarinpalPayment);
        }

        // GET: ZarinpalPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ZarinpalPayment zarinpalPayment = Biz.zarinpalBiz.Instance.Get(id??0);// db.ZarinpalPayments.Find(id);
            if (zarinpalPayment == null)
            {
                return HttpNotFound();
            }
            return View(zarinpalPayment);
        }

        // POST: ZarinpalPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            //ZarinpalPayment zarinpalPayment = db.ZarinpalPayments.Find(id);
            //db.ZarinpalPayments.Remove(zarinpalPayment);
            //db.SaveChanges();
            Biz.zarinpalBiz.Instance.Remove(id);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
