
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Models.BaseInfo;

namespace SurveyWeb.Controllers
{
    public class CitysController : BaseAdminController
    {
        private Context db = new Context();

        // GET: Citys
        public async Task<ActionResult> Index()
        {
            return View(await db.Citys.ToListAsync());
        }

        // GET: Citys/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City City = await db.Citys.FindAsync(id);
            if (City == null)
            {
                return HttpNotFound();
            }
            return View(City);
        }

        // GET: Citys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Citys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DropDownTitle,ProvinceId")] City City)
        {
            if (ModelState.IsValid)
            {
                db.Citys.Add(City);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(City);
        }

        // GET: Citys/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City City = await db.Citys.FindAsync(id);
            if (City == null)
            {
                return HttpNotFound();
            }
            return View(City);
        }

        // POST: Citys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DropDownTitle,ProvinceId")] City City)
        {
            if (ModelState.IsValid)
            {
                db.Entry(City).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(City);
        }

        // GET: Citys/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City City = await db.Citys.FindAsync(id);
            if (City == null)
            {
                return HttpNotFound();
            }
            return View(City);
        }

        // POST: Citys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            City City = await db.Citys.FindAsync(id);
            db.Citys.Remove(City);
            await db.SaveChangesAsync();
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
