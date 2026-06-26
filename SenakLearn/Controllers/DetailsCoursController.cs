using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.EMMA;
using SenakLearn.Models;

namespace SenakLearn.Controllers
{
    public class DetailsCoursController : BaseController
    {
        private SWEntities db = new SWEntities();

        // GET: DetailsCours
        public async Task<ActionResult> Index(int id, int type = 1)
        {
            learn_cours learn_cours = db.learn_cours.FirstOrDefault(x => x.id == id);
            bool existsInterview = true;
            if (learn_cours.InterviewPathVideo == null)
                existsInterview = false;
            else
            {
                string filePrevivew = Server.MapPath("/images/VideoFile/" + learn_cours.InterviewPathVideo.ToString().Replace("-", "") + ".mp4");
                if (!System.IO.File.Exists(filePrevivew))
                    existsInterview = false;

            }
            if (existsInterview == false)
            {
                var offlineVideo = db.OfflineVideo.FirstOrDefault(a => a.learn_coursId == learn_cours.id);
                if (offlineVideo != null)
                    learn_cours.InterviewPathVideo = offlineVideo.VideoId;
            }
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
                    //return PartialView("_PartialVideo", "/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp4");
                    return Json(new {id=videoId},JsonRequestBehavior.AllowGet);
                }
                await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.VideoNotFree);
            }

            return null;
        }

        public async Task<ActionResult> DownloadVideo(int id)
        {
            OfflineVideo video = Biz.OfflineVideoBiz.Instance.Get(id);
            var videoId = video?.VideoId;
            string filename = videoId.ToString().Replace("-", "") + ".mp4";
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/images/VideoFile/" + filename);
            if (videoId == null || videoId == Guid.Empty || !System.IO.File.Exists(filepath))
                return null;

            if (video.IsFree || (Current_learn_userId != 0 && Biz.zarinpalBiz.Instance.ExistByUser(video.learn_coursId, Current_learn_userId)))
            {
                await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Video);

                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(filedata, contentType);
            }
            await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.VideoNotFree);

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
