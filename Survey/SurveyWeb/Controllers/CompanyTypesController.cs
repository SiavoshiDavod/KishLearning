
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
    public class CompanyTypesController : BaseAdminController
    {
        private Context db = new Context();

        // GET: CompanyTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.CompanyTypes.ToListAsync());
        }

        // GET: CompanyTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyType CompanyType = await db.CompanyTypes.FindAsync(id);
            if (CompanyType == null)
            {
                return HttpNotFound();
            }
            return View(CompanyType);
        }

        // GET: CompanyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DropDownTitle")] CompanyType CompanyType)
        {
            if (ModelState.IsValid)
            {
                db.CompanyTypes.Add(CompanyType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(CompanyType);
        }

        // GET: CompanyTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyType CompanyType = await db.CompanyTypes.FindAsync(id);
            if (CompanyType == null)
            {
                return HttpNotFound();
            }
            return View(CompanyType);
        }

        // POST: CompanyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DropDownTitle")] CompanyType CompanyType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(CompanyType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(CompanyType);
        }

        // GET: CompanyTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyType CompanyType = await db.CompanyTypes.FindAsync(id);
            if (CompanyType == null)
            {
                return HttpNotFound();
            }
            return View(CompanyType);
        }

        // POST: CompanyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CompanyType CompanyType = await db.CompanyTypes.FindAsync(id);
            db.CompanyTypes.Remove(CompanyType);
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
