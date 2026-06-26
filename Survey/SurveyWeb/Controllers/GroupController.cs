using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using System.Data.Entity;
using System.Web;

namespace SenakLearn.Controllers.Admin
{
    public class GroupController : SenakLearn.Controllers.BaseAdminController
    {
        private SWEntities db = new SWEntities();

        // GET: Admin/Group
        public ActionResult Index()
        {
            return View(db.learn_cours_group.ToList());
        }

        // GET: Admin/Group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_cours_group learn_cours_group = db.learn_cours_group.Find(id);
            if (learn_cours_group == null)
            {
                return HttpNotFound();
            }
            return View(learn_cours_group);
        }

        // GET: Admin/Group/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // 
        public ActionResult Create(learn_cours_group learn_cours_group, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learn_cours_group.ImageUrl = SaveFile(ImageFile, pathFile.Group);
                db.learn_cours_group.Add(learn_cours_group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(learn_cours_group);
        }

        // GET: Admin/Group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_cours_group learn_cours_group = db.learn_cours_group.Find(id);
            if (learn_cours_group == null)
            {
                return HttpNotFound();
            }
            return View(learn_cours_group);
        }

        // POST: Admin/Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // 
        public ActionResult Edit(learn_cours_group learn_cours_group, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learn_cours_group.ImageUrl = EditFile(ImageFile, pathFile.Group, learn_cours_group.ImageUrl);
                db.Entry(learn_cours_group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(learn_cours_group);
        }

        // GET: Admin/Group/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_cours_group learn_cours_group = db.learn_cours_group.Find(id);
            if (learn_cours_group == null)
            {
                return HttpNotFound();
            }
            return View(learn_cours_group);
        }

        // POST: Admin/Group/Delete/5
        [HttpPost, ActionName("Delete")]
       // 
        public ActionResult DeleteConfirmed(int id)
        {
            learn_cours_group learn_cours_group = db.learn_cours_group.Find(id);
            db.learn_cours_group.Remove(learn_cours_group);
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
