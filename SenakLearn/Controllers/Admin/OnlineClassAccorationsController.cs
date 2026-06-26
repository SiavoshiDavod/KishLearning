using System;
using System.Linq;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;

namespace SenakLearn.Controllers
{
    public class OnlineClassAccorationsController : BaseAdminController
    {
        //private SWEntities db = new SWEntities();
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OnlineClassAccorationBiz.Instance.GetAllPagedList(grid);
            //var count = Biz.OnlineClassAccorationBiz.Instance.Count;
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

        public static GridColumnModelList<OnlineClassAccoration> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<OnlineClassAccoration> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<OnlineClassAccoration>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("0");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.Name).SetCaption("عنوان توضیح").SetWidth("300");
            }
            return Columns;
        }
        #endregion Get  Columns
        // GET: OnlineClassAccorations
        public ActionResult Index()
        {
            return View(Biz.OnlineClassAccorationBiz.Instance.GetAll(x => x.Id != 0));
        }

        // GET: OnlineClassAccorations/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClassAccoration OnlineClassAccoration = Biz.OnlineClassAccorationBiz.Instance.Get(id ?? 0);// db.OnlineClassAccorations.Find(id);
            if (OnlineClassAccoration == null)
            {
                return HttpNotFound();
            }
            return View(OnlineClassAccoration);
        }
        public ActionResult GetTreeList(int? id)
        {
            var OnlineClassAccorationDetails = Biz.OnlineClassAccorationDetailsBiz.Instance.GetAll(x=>x.OnlineClassAccorationId==id.Value);// db.OnlineClassAccorations.Find(id);
            if (OnlineClassAccorationDetails == null)
            {
                return HttpNotFound();
            }
            var treeList = GetRecursiveJsTreeList<OnlineClassAccorationDetails>.Instance.GetTreeList(OnlineClassAccorationDetails.ToList());
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
        // GET: OnlineClassAccorations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnlineClassAccorations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create(OnlineClassAccoration OnlineClassAccoration)
        {
            if (ModelState.IsValid)
            {
                Biz.OnlineClassAccorationBiz.Instance.Save(OnlineClassAccoration);
                //db.OnlineClassAccorations.Add(OnlineClassAccoration);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(OnlineClassAccoration);
        }

        // GET: OnlineClassAccorations/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClassAccoration OnlineClassAccoration = Biz.OnlineClassAccorationBiz.Instance.Get(id ?? 0);// db.OnlineClassAccorations.Find(id);
            if (OnlineClassAccoration == null)
            {
                return HttpNotFound();
            }
            return View(OnlineClassAccoration);
        }

        // POST: OnlineClassAccorations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit(OnlineClassAccoration OnlineClassAccoration)
        {
            if (ModelState.IsValid)
            {
                Biz.OnlineClassAccorationBiz.Instance.Save(OnlineClassAccoration);
                //db.Entry(OnlineClassAccoration).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(OnlineClassAccoration);
        }

        // GET: OnlineClassAccorations/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClassAccoration OnlineClassAccoration = Biz.OnlineClassAccorationBiz.Instance.Get(id ?? 0);// db.OnlineClassAccorations.Find(id);
            if (OnlineClassAccoration == null)
            {
                return HttpNotFound();
            }
            return View(OnlineClassAccoration);
        }

        // POST: OnlineClassAccorations/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            //OnlineClassAccoration OnlineClassAccoration = db.OnlineClassAccorations.Find(id);
            //db.OnlineClassAccorations.Remove(OnlineClassAccoration);
            //db.SaveChanges();
            Biz.OnlineClassAccorationBiz.Instance.Remove(id);
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
