using MVC.Controls.Grid;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class BoardDirectorsController : BaseAdminController
    {
        // GET: Admin/BoardDirector
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.BoardDirectorBiz.Instance.GetAllPagedList(grid);
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

        public static GridColumnModelList<BoardDirector> BoardDirectorColumns { get; private set; } = GetBoardDirectorColumns();
        public static GridColumnModelList<BoardDirector> GetBoardDirectorColumns()
        {
            if (BoardDirectorColumns == null)
            {
                BoardDirectorColumns = new GridColumnModelList<BoardDirector>();
                BoardDirectorColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                BoardDirectorColumns.Add(x => x.Resume).SetCaption("عملیات").SetWidth("50").SetSortable(false).SetSearchable(false);
                BoardDirectorColumns.Add(x => x.FullName).SetCaption("نام ").SetWidth("150");
                BoardDirectorColumns.Add(x => x.email).SetCaption("ایمیل ").SetWidth("180");
                BoardDirectorColumns.Add(x => x.status).SetCaption("وضعیت").SetWidth("50");
                BoardDirectorColumns.Add(x => x.meli).SetCaption("کد ملی").SetWidth("80");
                BoardDirectorColumns.Add(x => x.tel).SetCaption("تلفن ").SetWidth("80");
                BoardDirectorColumns.Add(x => x.mobile).SetCaption("همراه ").SetWidth("80");
                BoardDirectorColumns.Add(x => x.code).SetCaption("کد استاد ").SetWidth("80");
                BoardDirectorColumns.Add(x => x.education).SetCaption("مدرک تحصیلی ").SetWidth("80");
            }
            return BoardDirectorColumns;
        }

        // GET: Admin/BoardDirector/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardDirector BoardDirector = Biz.BoardDirectorBiz.Instance.FindById(id.Value);
            if (BoardDirector == null)
            {
                return HttpNotFound();
            }
            return View(BoardDirector);
        }

        // GET: Admin/BoardDirector/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/BoardDirector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // 
        public ActionResult Create(BoardDirector BoardDirector, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                BoardDirector.image = SaveFile(ImageFile, pathFile.BoardDirector);
                BoardDirector.CreatedDate = DateTime.Now;
                BoardDirector.status = true;
                Biz.BoardDirectorBiz.Instance.Create(BoardDirector);
                return RedirectToAction("Index");
            }

            return View(BoardDirector);
        }

        // GET: Admin/BoardDirector/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardDirector BoardDirector = Biz.BoardDirectorBiz.Instance.FindById(id.Value);
            if (BoardDirector == null)
            {
                return HttpNotFound();
            }
            return View(BoardDirector);
        }

        // POST: Admin/BoardDirector/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // 
        public ActionResult Edit(BoardDirector BoardDirector, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                BoardDirector.image = EditFile(ImageFile, pathFile.BoardDirector, BoardDirector.image);
                Biz.BoardDirectorBiz.Instance.Update(BoardDirector);
                return RedirectToAction("Index");
            }
            return View(BoardDirector);
        }

        // GET: Admin/BoardDirector/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardDirector BoardDirector = Biz.BoardDirectorBiz.Instance.FindById(id.Value);
            if (BoardDirector == null)
            {
                return HttpNotFound();
            }
            return View(BoardDirector);
        }

        // POST: Admin/BoardDirector/Delete/5
        [HttpPost, ActionName("Delete")]
        // 
        public ActionResult DeleteConfirmed(int id)
        {
            Biz.BoardDirectorBiz.Instance.Remove(id);
            //if (BoardDirector == null)
            //{
            //    return HttpNotFound();
            //}
            //else
            //{
            //    if (BoardDirector.image != null && BoardDirector.image != "no-photo.jpg")
            //        if (System.IO.File.Exists(Server.MapPath("/images/BoardDirector/" + BoardDirector.image)))
            //            System.IO.File.Delete(Server.MapPath("/images/BoardDirector/" + BoardDirector.image));
            //}

            //db.BoardDirector.Remove(BoardDirector);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}