using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    [AllowAnonymous]
    public class VideoFileController : BaseAdminController
    {
        private SWEntities db = new SWEntities();

        // GET: VideoFiles

        public ActionResult Index(int? userId)
        {
            if (userId == null)
            {
                ViewBag.UserId = "";
            }
            else
            {
                ViewBag.UserId = "?wating=null&userId=" + userId;
            }
            return View();
        }

        public ActionResult Selector()
        {
            return PartialView();
        }
        public ActionResult Accept(Guid id)
        {
            var obj = db.VideoFiles.First(x => x.VideoId == id);
            if (obj == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            if (obj.WaitingForAccept)
            {
                obj.WaitingForAccept = false;
                db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadList(GridSettings grid, int? userId, bool? wating)
        {
            grid.SortColumn = "createDate";
            grid.SortOrder = "dsc";
            var list = db.VideoFiles.Where(x => (userId == null || x.createBy == userId) && (userId != null || !x.WaitingForAccept) && (wating == null || x.WaitingForAccept == wating)).FilterAndSortJqGrid(grid).ToPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                UserData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<VideoFile> VideoSelectorColumns { get; private set; } = GetVideoSelectorColumns();
        public static GridColumnModelList<VideoFile> GetVideoSelectorColumns()
        {
            if (VideoSelectorColumns == null)
            {
                VideoSelectorColumns = new GridColumnModelList<VideoFile>();

                VideoSelectorColumns.Add(x => x.VideoId).SetAsPrimaryKey().SetHidden(true);
                VideoSelectorColumns.Add(x => x.titel).SetCaption("ویدیو/صدا").SetWidth("300");
                VideoSelectorColumns.Add(x => x.format).SetCaption("فرمت").SetWidth("100");

            }
            return VideoSelectorColumns;
        }
        public static GridColumnModelList<VideoFile> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<VideoFile> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<VideoFile>();
                Columns.Add(x => x.VideoId).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("150");
                Columns.Add(x => x.titel).SetCaption("ویدیو/صدا").SetWidth("300");
                Columns.Add(x => x.doc).SetCaption("توضیح").SetWidth("300");
                Columns.Add(x => x.WaitingForAccept).SetCaption("منتظر تایید ادمین").SetWidth("100");
            }
            return Columns;
        }
        // GET: VideoFiles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoFile VideoFile = db.VideoFiles.Find(id);
            if (VideoFile == null)
            {
                return HttpNotFound();
            }
            return View(VideoFile);
        }

        // GET: VideoFiles/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreatePartial()
        {
            return PartialView("~/Views/VideoFile/CreatePartial.cshtml");
        }

        // POST: VideoFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(VideoFile VideoFile, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid || ImageFile == null)
            {
                var type = Path.GetExtension(ImageFile.FileName).ToLower();
                if (type != ".mp4")
                {
                    ModelState.AddModelError("myFile", "فرمت فایل ارسالی باید mp4 باشد");
                    return View(VideoFile);
                }

                VideoFile.VideoId = Guid.NewGuid();
                VideoFile.myFile = VideoFile.VideoId.ToString().Replace("-", "") + type;
                ImageFile.SaveAs(Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile));
                // VideoFile.myFile = SaveFile(ImageFile, pathFile.VideoFile);

                VideoFile.createBy = Current_learn_userId;
                VideoFile.createDate = DateTime.Now;
                VideoFile.format = type.Replace(".", "");

                db.VideoFiles.Add(VideoFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(VideoFile);
        }

        [HttpPost]

        public ActionResult CreateAjax(VideoFile VideoFile)
        {
            if (Request.Files.Count == 0)
                ModelState.AddModelError("myFile", "یک فایل انتخاب نماپید !");
            HttpPostedFileBase file = Request.Files[0];
            //var files = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            if (ModelState.IsValid || file == null)
            {
                var type = Path.GetExtension(file.FileName).ToLower();
                if (type != ".mp4" && type != ".mp3")
                {
                    ModelState.AddModelError("myFile", "فرمت فایل ارسالی باید mp3/mp4 باشد");
                    return View(VideoFile);
                }

                VideoFile.VideoId = Guid.NewGuid();
                VideoFile.myFile = VideoFile.VideoId.ToString().Replace("-", "") + type;
                file.SaveAs(Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile));
                // VideoFile.myFile = SaveFile(ImageFile, pathFile.VideoFile);

                VideoFile.createBy = Current_learn_userId;
                VideoFile.createDate = DateTime.Now;
                VideoFile.format = type.Replace(".", "");

                db.VideoFiles.Add(VideoFile);
                db.SaveChanges();
                return Json(new { VideoId = VideoFile.VideoId, VideoName = VideoFile.titel }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PlayPartial(Guid videoId)
        {
            VideoFile VideoFile = db.VideoFiles.Find(videoId);
            return PartialView("~/Views/VideoFile/PlayPartial.cshtml", VideoFile);
        }
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoFile VideoFile = db.VideoFiles.Find(id);
            if (VideoFile == null)
            {
                return HttpNotFound();
            }
            return View(VideoFile);
        }
        [AllowAnonymous]
        public async Task FetchVideo(Guid videoId)
        {
            try
            {

                VideoFile VideoFile = db.VideoFiles.Find(videoId);
                var videoDir = Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile);
                byte[] bytes = System.IO.File.ReadAllBytes(videoDir);

                long fileSize = bytes.Length;
                long totalByte = fileSize - 1;
                long startByte = 0;
                long endByte = totalByte;
                int bufferSize = 1024 * 1024; // 24KB buffer size

                if (!string.IsNullOrEmpty(Request.Headers["X-Playback-Session-Id"]))
                    Response.AddHeader("X-Playback-Session-Id", Request.Headers["X-Playback-Session-Id"]);

                if (!string.IsNullOrEmpty(Request.Headers["Range"]))
                {
                    //Range: <unit>=<range-start>
                    string range = Request.Headers["Range"].Replace("bytes=", "");
                    string[] rangeParts = range.Split('-');
                    startByte = long.Parse(rangeParts[0]);
                    if (rangeParts.Length > 1 && !string.IsNullOrEmpty(rangeParts[1]))
                        endByte = long.Parse(rangeParts[1]);
                }

                // recalculate after range has been interpreted
                int bytesToRead = Math.Min((int)(endByte - startByte + 1), bufferSize);

                Response.AddHeader("Content-Range", $"bytes {startByte}-{endByte}/{fileSize}");
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AddHeader("Content-Type", "video/mp4");
                Response.AddHeader("Connection", "Keep-Alive");
                Response.AddHeader("Content-Name", VideoFile.titel);
                Response.AddHeader("Content-Version", "1.0");
                Response.AddHeader("Content-Vendor", "XMP");
                Response.AddHeader("Content-Size", bytesToRead.ToString());
                Response.AddHeader("Content-Length", bytesToRead.ToString());

                Response.StatusCode = 206;
                Response.ContentType = "video/mp4";

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    memoryStream.Seek(startByte, SeekOrigin.Begin);

                    byte[] buffer = new byte[bufferSize];
                    long bytesRemaining = bytesToRead;

                    while (bytesRemaining > 0)
                    {
                        int bytesRead = await memoryStream.ReadAsync(buffer, 0, bytesToRead);

                        if (bytesRead == 0)
                            break;

                        if (Response.IsClientConnected)
                        {
                            await Response.OutputStream.WriteAsync(buffer, 0, bytesRead);
                            await Response.OutputStream.FlushAsync();
                            bytesRemaining -= bytesRead;
                        }
                        else
                        {
                            break; // Client disconnected
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        public async Task FetchAudio(Guid videoId)
        {
            try
            {

                VideoFile VideoFile = db.VideoFiles.Find(videoId);
                var videoDir = Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile);
                byte[] bytes = System.IO.File.ReadAllBytes(videoDir);

                long fileSize = bytes.Length;
                long totalByte = fileSize - 1;
                long startByte = 0;
                long endByte = totalByte;
                int bufferSize = 1024 * 1024; // 24KB buffer size

                if (!string.IsNullOrEmpty(Request.Headers["X-Playback-Session-Id"]))
                    Response.AddHeader("X-Playback-Session-Id", Request.Headers["X-Playback-Session-Id"]);

                if (!string.IsNullOrEmpty(Request.Headers["Range"]))
                {
                    //Range: <unit>=<range-start>
                    string range = Request.Headers["Range"].Replace("bytes=", "");
                    string[] rangeParts = range.Split('-');
                    startByte = long.Parse(rangeParts[0]);
                    if (rangeParts.Length > 1 && !string.IsNullOrEmpty(rangeParts[1]))
                        endByte = long.Parse(rangeParts[1]);
                }

                // recalculate after range has been interpreted
                int bytesToRead = Math.Min((int)(endByte - startByte + 1), bufferSize);

                Response.AddHeader("Content-Range", $"bytes {startByte}-{endByte}/{fileSize}");
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AddHeader("Content-Type", "audio/mp3");
                Response.AddHeader("Connection", "Keep-Alive");
                Response.AddHeader("Content-Name", VideoFile.titel);
                Response.AddHeader("Content-Version", "1.0");
                Response.AddHeader("Content-Vendor", "XMP");
                Response.AddHeader("Content-Size", bytesToRead.ToString());
                Response.AddHeader("Content-Length", bytesToRead.ToString());

                Response.StatusCode = 206;
                Response.ContentType = "audio/mp3";

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    memoryStream.Seek(startByte, SeekOrigin.Begin);

                    byte[] buffer = new byte[bufferSize];
                    long bytesRemaining = bytesToRead;

                    while (bytesRemaining > 0)
                    {
                        int bytesRead = await memoryStream.ReadAsync(buffer, 0, bytesToRead);

                        if (bytesRead == 0)
                            break;

                        if (Response.IsClientConnected)
                        {
                            await Response.OutputStream.WriteAsync(buffer, 0, bytesRead);
                            await Response.OutputStream.FlushAsync();
                            bytesRemaining -= bytesRead;
                        }
                        else
                        {
                            break; // Client disconnected
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        // POST: VideoFiles/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> getDesriptVideo(Guid videoId)
        {
            var video =await db.VideoFiles.FindAsync(videoId);
            if (video == null)
                return  Json(new{res=string.Empty },JsonRequestBehavior.AllowGet);
            else
                return Json(new { res = video.doc }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public ActionResult Edit(VideoFile VideoFile)
        {
            if (string.IsNullOrEmpty(VideoFile.titel))
            {
                return View(VideoFile);
            }
            VideoFile found = db.VideoFiles.Find(VideoFile.VideoId);
            if (found == null)
            {
                return View(VideoFile);
            }
            found.doc = VideoFile.doc;
            found.titel = VideoFile.titel;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: VideoFiles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoFile VideoFile = db.VideoFiles.Find(id);
            if (VideoFile == null)
            {
                return HttpNotFound();
            }
            return View(VideoFile);
        }

        // POST: VideoFiles/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                VideoFile VideoFile = db.VideoFiles.Find(id);
                System.IO.File.Delete(Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile));

                var offlineVideos = db.OfflineVideo.Where(w => w.VideoId == VideoFile.VideoId).ToList();
                offlineVideos.RemoveAt(0);
                db.VideoFiles.Remove(VideoFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw new Exception("حذف ویدیو با مشکل روبرو شد !");
            }

            return RedirectToAction("Index");
        }
        public ActionResult DeleteAll()
        {
            try
            {
                var VideoFiles = db.VideoFiles.ToList();
                foreach (var VideoFile in VideoFiles)
                {
                    if (System.IO.File.Exists(Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile)))
                        System.IO.File.Delete(Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile));

                    var offlineVideos = db.OfflineVideo.Where(w => w.VideoId == VideoFile.VideoId).ToList();
                    offlineVideos.RemoveAt(0);
                    db.VideoFiles.Remove(VideoFile);
                    db.SaveChanges();
                }

                return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { message = "NOK" }, JsonRequestBehavior.AllowGet);
            }
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