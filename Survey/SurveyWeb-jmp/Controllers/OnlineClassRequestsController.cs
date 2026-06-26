using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SenakLearn.Models;

namespace SenakLearn.Controllers
{
    public class OnlineClassRequestsController : BaseAdminController
    {
        private SWEntities db = new SWEntities();

        // GET: OnlineClassRequests
        public ActionResult Index()
        {
            return View(db.OnlineClassRequests.ToList());
        }

        // GET: OnlineClassRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineClassRequest onlineClassRequest = db.OnlineClassRequests.Find(id);
            if (onlineClassRequest == null)
            {
                return HttpNotFound();
            }
            return View(onlineClassRequest);
        }

        // GET: OnlineClassRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnlineClassRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "Id,UserId,OnlineClassId,Notices,CreatedDate,UpdateDate")] OnlineClassRequest onlineClassRequest)
        {
            if (ModelState.IsValid)
            {
                db.OnlineClassRequests.Add(onlineClassRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(onlineClassRequest);
        }

        // GET: OnlineClassRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineClassRequest onlineClassRequest = db.OnlineClassRequests.Find(id);
            if (onlineClassRequest == null)
            {
                return HttpNotFound();
            }
            return View(onlineClassRequest);
        }

        // POST: OnlineClassRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "Id,UserId,OnlineClassId,Notices,CreatedDate,UpdateDate")] OnlineClassRequest onlineClassRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineClassRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(onlineClassRequest);
        }

        // GET: OnlineClassRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineClassRequest onlineClassRequest = db.OnlineClassRequests.Find(id);
            if (onlineClassRequest == null)
            {
                return HttpNotFound();
            }
            return View(onlineClassRequest);
        }

        // POST: OnlineClassRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            OnlineClassRequest onlineClassRequest = db.OnlineClassRequests.Find(id);
            db.OnlineClassRequests.Remove(onlineClassRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
