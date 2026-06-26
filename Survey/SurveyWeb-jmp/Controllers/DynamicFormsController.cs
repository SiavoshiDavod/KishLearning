using SenakLearn.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class DynamicFormsController : BaseAdminController
    {
        private SWEntities db = new SWEntities();

        // GET: DynamicForms
        public ActionResult Index()
        {
            var dynamicForms = db.DynamicForms.Include(d => d.Menu);
            return View(dynamicForms.ToList());
        }

        // GET: DynamicForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicForm dynamicForm = db.DynamicForms.Find(id);
            if (dynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(dynamicForm);
        }

        // GET: DynamicForms/Create
        public ActionResult Create()
        {
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Title");
            return View();
        }

        // POST: DynamicForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create(DynamicForm dynamicForm, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                dynamicForm.Image = SaveFile(ImageFile, pathFile.DynamicForm);
               
                db.DynamicForms.Add(dynamicForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Title", dynamicForm.MenuId);
            return View(dynamicForm);
        }

        // GET: DynamicForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicForm dynamicForm = db.DynamicForms.Find(id);
            if (dynamicForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Title", dynamicForm.MenuId);
            return View(dynamicForm);
        }

        // POST: DynamicForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit( DynamicForm dynamicForm, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                dynamicForm.Image = EditFile(ImageFile, pathFile.DynamicForm, dynamicForm.Image);
                
                db.Entry(dynamicForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Title", dynamicForm.MenuId);
            return View(dynamicForm);
        }

        // GET: DynamicForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicForm dynamicForm = db.DynamicForms.Find(id);
            if (dynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(dynamicForm);
        }

        // POST: DynamicForms/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            DynamicForm dynamicForm = db.DynamicForms.Find(id);
            db.DynamicForms.Remove(dynamicForm);
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