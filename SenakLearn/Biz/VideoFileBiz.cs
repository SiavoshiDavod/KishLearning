using DocumentFormat.OpenXml.Office2013.Excel;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SenakLearn.Controllers.BaseController;

namespace SenakLearn.Biz
{
    public class VideoFileBiz
    {
        public static readonly VideoFileBiz Instance = new VideoFileBiz();
        public SenakLearn.Models.wrapper.AudioFileWrapper Find(Guid id, int offlineVideoId)
        {

            SenakLearn.Models.wrapper.AudioFileWrapper item = null;
            OfflineVideo offlineVideo = null;
            learn_cours learnCours = null;
            using (var context = new SWEntities())
            {
                if (offlineVideoId != 0)
                    offlineVideo = context.OfflineVideo.FirstOrDefault(a => a.Id == offlineVideoId);
                if (offlineVideo != null)
                    learnCours = context.learn_cours.FirstOrDefault(a => a.id == offlineVideo.learn_coursId);

                item = context.VideoFiles.Where(w => w.VideoId == id)
                    .Select(a => new Models.wrapper.AudioFileWrapper { Id = a.VideoId, Descript = a.doc }).SingleOrDefault();

                if (learnCours != null)
                    item.Img = "/images/cours/" + learnCours.image;
            }
            return item;
        }

        public byte[] GetBineryVideo(Guid videoId, string serverVideoPath)
        {
            byte[] bytes = null;
            using (var context = new SWEntities())
            {
                VideoFile VideoFile = context.VideoFiles.Find(videoId);
                var videoDir = serverVideoPath + "/" + VideoFile.myFile;
                bytes = System.IO.File.ReadAllBytes(videoDir);

            }
            return bytes;
        }
    }
}