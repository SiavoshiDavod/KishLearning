
using System.Linq;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class SupportController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View(Biz.StudentSupportBiz.Instance.GetAll().ToList());
        }
        public ActionResult ShowVideo(int id)
        {
            var videoId = Biz.StudentSupportBiz.Instance.Get(id)?.VideoId;

            if (videoId!=null)
                return PartialView("_PartialVideo", "/images/VideoFile/" + videoId.ToString().Replace("-","")+".mp4");
            return null;
        }
        public ActionResult GetTreeList()
        {
            var StudentSupport = Biz.StudentSupportBiz.Instance.GetAll();// db.OnlineClassAccorations.Find(id);
            if (StudentSupport == null)
            {
                return HttpNotFound();
            }
            var treeList = GetRecursiveJsTreeList<Models.StudentSupport>.Instance.GetTreeList(StudentSupport.ToList());
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
    }
}