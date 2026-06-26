using System.Linq;
using SenakLearn.Models;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class TeacherSupportController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetTreeList(int? id)
        {
            var TeacherSupport = Biz.TeacherSupportBiz.Instance.GetAll();// db.OnlineClassAccorations.Find(id);
            if (TeacherSupport == null)
            {
                return HttpNotFound();
            }
            var treeList = GetRecursiveJsTreeList<TeacherSupport>.Instance.GetTreeList(TeacherSupport.ToList());
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {

            TeacherSupport TeacherSupport = Biz.TeacherSupportBiz.Instance.Get(id ?? 0);// db.TeacherSupports.Find(id);
            if (TeacherSupport == null)
            {
                return HttpNotFound();
            }
            return PartialView(TeacherSupport);
        }

        public ActionResult Create(int id)
        {
            return PartialView(new TeacherSupport());
        }

        [HttpPost]
        public ActionResult Create(TeacherSupport TeacherSupport)
        {
            if (ModelState.IsValid)
            {
                Biz.TeacherSupportBiz.Instance.Save(TeacherSupport);
                return RedirectToAction("Create", new TeacherSupport());
            }

            return PartialView(TeacherSupport);
        }

        // GET: TeacherSupports/Edit/5
        public ActionResult Edit(int? id)
        {

            TeacherSupport TeacherSupport = Biz.TeacherSupportBiz.Instance.Get(id ?? 0);// db.TeacherSupports.Find(id);
            if (TeacherSupport == null)
            {
                return HttpNotFound();
            }
            return PartialView("Create", TeacherSupport);
        }


        [HttpPost]
        public ActionResult Edit(TeacherSupport TeacherSupport)
        {
            if (ModelState.IsValid)
            {
                Biz.TeacherSupportBiz.Instance.Save(TeacherSupport);
                //db.Entry(TeacherSupport).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Create", new TeacherSupport());
            }
            return PartialView("Create", TeacherSupport);
        }

        public ActionResult Delete(int id)
        {
            Biz.TeacherSupportBiz.Instance.Remove(id);
            return RedirectToAction("Index");
        }
    }
}