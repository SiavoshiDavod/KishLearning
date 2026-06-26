using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;

namespace SurveyWeb.Controllers
{
    public class CheckListTypesController : BaseAdminController
    {
        private Context db = new Context();

        // GET: CheckListTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.CheckListType.ToListAsync());
        }

        // GET: CheckListTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListType checkListType = await db.CheckListType.FindAsync(id);
            if (checkListType == null)
            {
                return HttpNotFound();
            }
            return View(checkListType);
        }

        // GET: CheckListTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckListTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CheckListType checkListType)
        {
            if (ModelState.IsValid)
            {
                db.CheckListType.Add(checkListType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(checkListType);
        }

        // GET: CheckListTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListType checkListType = await db.CheckListType.FindAsync(id);
            if (checkListType == null)
            {
                return HttpNotFound();
            }
            return View(checkListType);
        }

        // POST: CheckListTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( CheckListType checkListType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkListType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(checkListType);
        }

        // GET: CheckListTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListType checkListType = await db.CheckListType.FindAsync(id);
            if (checkListType == null)
            {
                return HttpNotFound();
            }
            return View(checkListType);
        }

        // POST: CheckListTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CheckListType checkListType = await db.CheckListType.FindAsync(id);
            db.CheckListType.Remove(checkListType);
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
