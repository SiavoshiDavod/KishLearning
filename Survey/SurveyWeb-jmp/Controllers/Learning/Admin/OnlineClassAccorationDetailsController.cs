using System;
using System.Linq;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;
namespace SenakLearn.Controllers
{
    public class OnlineClassAccorationDetailsController : BaseAdminController
    {
        //private SWEntities db = new SWEntities();
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OnlineClassAccorationDetailsBiz.Instance.GetAllPagedListToViewModel(grid);
            //var count = Biz.OnlineClassAccorationDetailsBiz.Instance.Count;
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

        public static GridColumnModelList<OnlineClassAccorationDetailsViewModel> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<OnlineClassAccorationDetailsViewModel> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<OnlineClassAccorationDetailsViewModel>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("0");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.Order).SetCaption("ترتیب").SetWidth("50");
                Columns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("300");
                Columns.Add(x => x.Parent).SetCaption("توضیحات پدر").SetWidth("300");
                Columns.Add(x => x.OnlineClassAccoration).SetCaption("عنوان").SetWidth("300");
            }
            return Columns;
        }
        #endregion Get  Columns
        // GET: OnlineClassAccorationDetailss
        public ActionResult Index()
        {
            return View(Biz.OnlineClassAccorationDetailsBiz.Instance.GetAll(x => x.Id != 0));
        }

        // GET: OnlineClassAccorationDetailss/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClassAccorationDetails OnlineClassAccorationDetails = Biz.OnlineClassAccorationDetailsBiz.Instance.Get(id ?? 0);// db.OnlineClassAccorationDetailss.Find(id);
            if (OnlineClassAccorationDetails == null)
            {
                return HttpNotFound();
            }
            return PartialView(OnlineClassAccorationDetails);
        }

        // GET: OnlineClassAccorationDetailss/Create
        public ActionResult Create(int id)
        {
            return PartialView(new OnlineClassAccorationDetails() { OnlineClassAccorationId=id});
        }

        // POST: OnlineClassAccorationDetailss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //
        public ActionResult Create( OnlineClassAccorationDetails OnlineClassAccorationDetails)
        {
            if (ModelState.IsValid)
            {
                Biz.OnlineClassAccorationDetailsBiz.Instance.Save(OnlineClassAccorationDetails);
                //db.OnlineClassAccorationDetailss.Add(OnlineClassAccorationDetails);
                //db.SaveChanges();
                return RedirectToAction("Create", new { id = OnlineClassAccorationDetails.OnlineClassAccorationId });
            }

            return PartialView(OnlineClassAccorationDetails);
        }

        // GET: OnlineClassAccorationDetailss/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClassAccorationDetails OnlineClassAccorationDetails = Biz.OnlineClassAccorationDetailsBiz.Instance.Get(id ?? 0);// db.OnlineClassAccorationDetailss.Find(id);
            if (OnlineClassAccorationDetails == null)
            {
                return HttpNotFound();
            }
            return PartialView(OnlineClassAccorationDetails);
        }

        // POST: OnlineClassAccorationDetailss/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //
        public ActionResult Edit(OnlineClassAccorationDetails OnlineClassAccorationDetails)
        {
            if (ModelState.IsValid)
            {
                Biz.OnlineClassAccorationDetailsBiz.Instance.Save(OnlineClassAccorationDetails);
                //db.Entry(OnlineClassAccorationDetails).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Create",new { id=OnlineClassAccorationDetails.OnlineClassAccorationId});
            }
            return PartialView(OnlineClassAccorationDetails);
        }

        // GET: OnlineClassAccorationDetailss/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    OnlineClassAccorationDetails OnlineClassAccorationDetails = Biz.OnlineClassAccorationDetailsBiz.Instance.Get(id ?? 0);// db.OnlineClassAccorationDetailss.Find(id);
        //    if (OnlineClassAccorationDetails == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView(OnlineClassAccorationDetails);
        //}

        // POST: OnlineClassAccorationDetailss/Delete/5
        //[HttpPost, ActionName("Delete")]
        //
        public ActionResult Delete(int id)
        {
            //OnlineClassAccorationDetails OnlineClassAccorationDetails = db.OnlineClassAccorationDetailss.Find(id);
            //db.OnlineClassAccorationDetailss.Remove(OnlineClassAccorationDetails);
            //db.SaveChanges();
            Biz.OnlineClassAccorationDetailsBiz.Instance.Remove(id);
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
