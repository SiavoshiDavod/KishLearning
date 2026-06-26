using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Teacher
{
    public class TeacherVideoFileController : BaseTeacherController
    {
        private SWEntities db = new SWEntities();

        // GET: VideoFiles

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Selector()
        {
            return PartialView();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = db.VideoFiles.Where(x=>x.createBy==Current_learn_userId).FilterAndSortJqGrid(grid).ToPagedList(grid);
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
                VideoSelectorColumns.Add(x => x.titel).SetCaption("ویدیو").SetWidth("300");
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
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                Columns.Add(x => x.titel).SetCaption("ویدیو").SetWidth("300");
                Columns.Add(x => x.doc).SetCaption("توضیح").SetWidth("300");
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
                VideoFile.WaitingForAccept = true;

                db.VideoFiles.Add(VideoFile);
                db.SaveChanges();

                var obj = Biz.JoinUsBiz.Instance.UploadVideo(Current_learn_userId);
                return RedirectToAction("Index");
            }

            return View(VideoFile);
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

        // POST: VideoFiles/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
            VideoFile VideoFile = db.VideoFiles.Find(id);
            System.IO.File.Delete(Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile));
            db.VideoFiles.Remove(VideoFile);
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