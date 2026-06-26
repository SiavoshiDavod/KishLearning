using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SenakLearn.Models;

namespace SenakLearn.Controllers
{
    public class DetailsCoursController : BaseController
    {
        private SWEntities db = new SWEntities();

        // GET: DetailsCours
        public async Task<ActionResult> Index(int id, int type = 1)
        {
            learn_cours learn_cours = db.learn_cours.FirstOrDefault(x => x.id == id && x.status);
            if (learn_cours == null)
            {
                return HttpNotFound();
            }
            if (learn_cours != null && Current_learn_userId != 0)
            {
                ViewBag.Buy = Biz.zarinpalBiz.Instance.ExistByUser(id, Current_learn_userId);
            }
            else
            {
                ViewBag.Buy = false;
            }
            await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Course);
            return PartialView(learn_cours);
        }

        public async Task<ActionResult> ShowVideo(int id)
        {
            OfflineVideo video = Biz.OfflineVideoBiz.Instance.Get(id);
            var videoId = video?.VideoId;

            if (videoId != null)
            {
                if (video.IsFree || (Current_learn_userId != 0 && Biz.zarinpalBiz.Instance.ExistByUser(video.learn_coursId, Current_learn_userId)))
                {
                    await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Video);
                    return PartialView("_PartialVideo", "/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp4");
                }
                await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.VideoNotFree);
            }

            return null;
        }


        //public ActionResult ShowComments(int id, int type = 1)
        //{
        //    var comments = db.learn_Comment.Where(c => c.RelatedId == id && c.RelatedType == type && c.ParentId == null).ToList();
        //    return PartialView(comments);
        //}
        //public ActionResult CreateComment(int id, int? parentid, int type = 1)
        //{
        //    return PartialView(new learn_Comment()
        //    {
        //        ParentId = parentid,
        //        RelatedId = id,
        //        RelatedType = type
        //    });
        //}
        //[HttpPost]
        //public ActionResult CreateComment(learn_Comment comment)
        //{
        //    comment.Date = DateTime.Now;
        //    db.learn_Comment.Add(comment);
        //    db.SaveChanges();
        //    return PartialView("ShowComments",
        //        db.learn_Comment.Where(c => c.RelatedId == comment.RelatedId && c.RelatedType == comment.RelatedType && c.ParentId == null).ToList());
        //}
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
