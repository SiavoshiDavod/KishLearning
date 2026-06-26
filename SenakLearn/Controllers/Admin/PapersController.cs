using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class PapersController : BaseAdminController
    {
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
            var list = Biz.PaperBiz.Instance.GetAllPagedList(grid);
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

        //public static GridColumnModelList<Paper> VideoSelectorColumns { get; private set; } = GetVideoSelectorColumns();
        //public static GridColumnModelList<Paper> GetVideoSelectorColumns()
        //{
        //    if (VideoSelectorColumns == null)
        //    {
        //        VideoSelectorColumns = new GridColumnModelList<Paper>();
        //        VideoSelectorColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
        //        VideoSelectorColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
        //        VideoSelectorColumns.Add(x => x.TitleF).SetCaption("عنوان").SetWidth("300");
        //    }
        //    return VideoSelectorColumns;
        //}
        public static GridColumnModelList<Paper> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<Paper> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<Paper>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                Columns.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
                Columns.Add(x => x.TitleF).SetCaption("عنوان فارسی").SetWidth("300");
            }
            return Columns;
        }
        // GET: Paper/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper Paper = Biz.PaperBiz.Instance.Get(id.Value);
            if (Paper == null)
            {
                return HttpNotFound();
            }
            return View(Paper);
        }

        // GET: Paper/Create
        public ActionResult Create(int id = 0)
        {
            Paper paper = id == 0 ? null : Biz.PaperBiz.Instance.Get(id);
            return View(paper);
        }

        // POST: Paper/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Create(Paper paper, HttpPostedFileBase File, HttpPostedFileBase ScreenShot, HttpPostedFileBase TranslateWord, HttpPostedFileBase TranslatePdf)
        {
            var isNew = false;
            // if (ModelState.IsValid)
            {
                if (paper.Id == 0)
                {
                    isNew = true;
                    paper.FileId = SaveFile(File, pathFile.Paper);
                    paper.ScreenShotId = SaveFile(ScreenShot, pathFile.Paper);
                    paper.TranslateWordId = SaveFile(TranslateWord, pathFile.Paper);
                    paper.TranslatePdfId = SaveFile(TranslatePdf, pathFile.Paper);
                }
                else
                {
                    paper.FileId = EditFile(File, pathFile.Paper, paper.FileId);
                    paper.ScreenShotId = EditFile(ScreenShot , pathFile.Paper, paper.ScreenShotId);
                    paper.TranslateWordId = EditFile(TranslateWord, pathFile.Paper, paper.TranslateWordId);
                    paper.TranslatePdfId = EditFile(TranslatePdf, pathFile.Paper, paper.TranslatePdfId);
                }
                if (string.IsNullOrEmpty(paper.FileId))
                {
                    throw new Exception("فایل اصلی را انتخاب کنید");
                }
                Biz.PaperBiz.Instance.Save(paper);
                if (isNew)
                    await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Paper,paper.GroupId);
                return RedirectToAction("Index");
            }

           // return View(paper);
        }

        // GET: Papers/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Paper paper = id == 0 ? null:Biz.PaperBiz.Instance.Get(id);
            return View(paper);
        }

        // POST: Papers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Paper paper, HttpPostedFileBase File, HttpPostedFileBase ScreenShot, HttpPostedFileBase TranslateWord, HttpPostedFileBase TranslatePdf)
        {
           // if (ModelState.IsValid)
            {
                if (paper.Id == 0)
                {
                    paper.FileId = SaveFile(File, pathFile.Paper);
                    paper.ScreenShotId = SaveFile(ScreenShot, pathFile.Paper);
                    paper.TranslateWordId = SaveFile(TranslateWord, pathFile.Paper);
                    paper.TranslatePdfId = SaveFile(TranslatePdf, pathFile.Paper);
                }
                else
                {
                    paper.FileId = EditFile(File, pathFile.Paper, paper.FileId);
                    paper.ScreenShotId = EditFile(ScreenShot, pathFile.Paper, paper.ScreenShotId);
                    paper.TranslateWordId = EditFile(TranslateWord, pathFile.Paper, paper.TranslateWordId);
                    paper.TranslatePdfId = EditFile(TranslatePdf, pathFile.Paper, paper.TranslatePdfId);
                }
                Biz.PaperBiz.Instance.Save(paper);
                return RedirectToAction("Index");
            }
           /// return View(paper);
        }

        // GET: Papers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = Biz.PaperBiz.Instance.Get(id.Value);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: Papers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           var groupId= Biz.PaperBiz.Instance.Remove(id);
            await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Paper,groupId, false);
            return RedirectToAction("Index");
        }

    }
}
