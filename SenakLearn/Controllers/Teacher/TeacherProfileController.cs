using SenakLearn.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Teacher
{
    public class TeacherProfileController : BaseTeacherController
    {
        public ActionResult AcceptContract()
        {
            var obj = Biz.JoinUsBiz.Instance.AcceptContract(Current_learn_userId);
            if (obj == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Support()
        {
            return View();
        }
        public ActionResult ShowVideo(int id)
        {
            var videoId = Biz.TeacherSupportBiz.Instance.Get(id)?.VideoId;

            if (videoId != null)
                return PartialView("_PartialVideo", "/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp4");
            return null;
        }
        public ActionResult GetTreeList()
        {
            var TeacherSupport = Biz.TeacherSupportBiz.Instance.GetAll();// db.OnlineClassAccorations.Find(id);
            if (TeacherSupport == null)
            {
                return HttpNotFound();
            }
            var treeList = GetRecursiveJsTreeList<Models.TeacherSupport>.Instance.GetTreeList(TeacherSupport.ToList());
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }


        // GET: TeacherProfile
        public ActionResult Index()
        {
            var me = Biz.TeacherBiz.Instance.FindByUserId(Current_learn_userId);
            return View(me);
        }
        public ActionResult Edit()
        {
            var me = Biz.TeacherBiz.Instance.FindByUserId(Current_learn_userId);
            return View(me);
        }
        [HttpPost]
        
        public ActionResult Edit(learn_teacher learn_teacher, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                learn_teacher.image = EditFile(ImageFile, pathFile.teacher, learn_teacher.image);

                using (var db= new SWEntities())
                {
                    var foundEntity = db.learn_teacher.Where(x => x.id == learn_teacher.id)?.FirstOrDefault();

                    if (null != foundEntity)
                    {
                        learn_teacher.PrincipalId = foundEntity.PrincipalId;
                        db.Entry(foundEntity).CurrentValues.SetValues(learn_teacher);
                        //foundEntity.State = EntityState.Detached;
                    }

                    //db.Entry(learn_teacher).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            return View(learn_teacher);
        }
    }
}