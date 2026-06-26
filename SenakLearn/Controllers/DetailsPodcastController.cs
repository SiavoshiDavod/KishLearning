using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SenakLearn.Models;

namespace SenakLearn.Controllers
{
    public class DetailsPodcastController : BaseController
    {
        private SWEntities db = new SWEntities();
        public async Task<ActionResult> Index(int id, int type = 2)
        {
            learn_cours learn_cours = db.learn_cours.FirstOrDefault(x => x.id == id);
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
            return PartialView("~/Views/DetailsPodcast/Index.cshtml", learn_cours);
        }
        public async Task<ActionResult> ShowPodcast(int id)
        {
            OfflineVideo video = Biz.OfflineVideoBiz.Instance.Get(id);
            var videoId = video?.VideoId;

            if (videoId != null)
            {
                if (video.IsFree || (Current_learn_userId != 0 && Biz.zarinpalBiz.Instance.ExistByUser(video.learn_coursId, Current_learn_userId)))
                {
                    await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Video);
                    return PartialView("_PartialAudio", "/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp3");
                }
                await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.VideoNotFree);
            }

            return null;
        }
        public async Task<ActionResult> DownloadAudio(int id)
        {
            OfflineVideo video = Biz.OfflineVideoBiz.Instance.Get(id);
            var videoId = video?.VideoId;
            string filename = videoId.ToString().Replace("-", "") + ".mp3";
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
    }
}