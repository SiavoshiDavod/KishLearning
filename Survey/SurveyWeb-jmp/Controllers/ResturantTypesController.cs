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

namespace SurveyWeb.Controllers
{
    public class ResturantTypesController : BaseAdminController
    {
        private Context db = new Context();

        // GET: ResturantTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ResturantType.ToListAsync());
        }

        // GET: ResturantTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResturantType resturantType = await db.ResturantType.FindAsync(id);
            if (resturantType == null)
            {
                return HttpNotFound();
            }
            return View(resturantType);
        }

        // GET: ResturantTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResturantTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DropDownTitle")] ResturantType resturantType)
        {
            if (ModelState.IsValid)
            {
                db.ResturantType.Add(resturantType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(resturantType);
        }

        // GET: ResturantTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResturantType resturantType = await db.ResturantType.FindAsync(id);
            if (resturantType == null)
            {
                return HttpNotFound();
            }
            return View(resturantType);
        }

        // POST: ResturantTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DropDownTitle")] ResturantType resturantType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resturantType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resturantType);
        }

        // GET: ResturantTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResturantType resturantType = await db.ResturantType.FindAsync(id);
            if (resturantType == null)
            {
                return HttpNotFound();
            }
            return View(resturantType);
        }

        // POST: ResturantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ResturantType resturantType = await db.ResturantType.FindAsync(id);
            db.ResturantType.Remove(resturantType);
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
