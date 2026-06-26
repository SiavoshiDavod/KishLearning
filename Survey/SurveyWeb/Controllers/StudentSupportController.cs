using System.Linq;
using SenakLearn.Models;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class StudentSupportController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetTreeList(int? id)
        {
            var StudentSupport = Biz.StudentSupportBiz.Instance.GetAll();// db.OnlineClassAccorations.Find(id);
            if (StudentSupport == null)
            {
                return HttpNotFound();
            }
            var treeList = GetRecursiveJsTreeList<StudentSupport>.Instance.GetTreeList(StudentSupport.ToList());
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {

            StudentSupport StudentSupport = Biz.StudentSupportBiz.Instance.Get(id ?? 0);// db.StudentSupports.Find(id);
            if (StudentSupport == null)
            {
                return HttpNotFound();
            }
            return PartialView(StudentSupport);
        }

        public ActionResult Create(int id)
        {
            return PartialView(new StudentSupport() );
        }

        [HttpPost]
        public ActionResult Create(StudentSupport StudentSupport)
        {
            if (ModelState.IsValid)
            {
                Biz.StudentSupportBiz.Instance.Save(StudentSupport);
                return RedirectToAction("Create", new StudentSupport());
            }

            return PartialView(StudentSupport);
        }

        // GET: StudentSupports/Edit/5
        public ActionResult Edit(int? id)
        {

            StudentSupport StudentSupport = Biz.StudentSupportBiz.Instance.Get(id ?? 0);// db.StudentSupports.Find(id);
            if (StudentSupport == null)
            {
                return HttpNotFound();
            }
            return PartialView("Create", StudentSupport);
        }


        [HttpPost]
        public ActionResult Edit(StudentSupport StudentSupport)
        {
            if (ModelState.IsValid)
            {
                Biz.StudentSupportBiz.Instance.Save(StudentSupport);
                //db.Entry(StudentSupport).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Create", new StudentSupport());
            }
            return PartialView("Create", StudentSupport);
        }

        public ActionResult Delete(int id)
        {
            Biz.StudentSupportBiz.Instance.Remove(id);
            return RedirectToAction("Index");
        }
    }
}