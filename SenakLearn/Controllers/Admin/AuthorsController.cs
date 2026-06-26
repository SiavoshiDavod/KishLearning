using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class AuthorsController : BaseAdminController
    {


        // GET: Authors
        public ActionResult Index()
        {
            var model = new List<SenakLearn.Models.Author>();
            return View(model);
        }   
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.AuthorBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AuthorData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Author> AuthorColumns { get; private set; } = GetAuthorColumns();
        public static GridColumnModelList<Author> GetAuthorColumns()
        {
            if (AuthorColumns == null)
            {
                AuthorColumns = new GridColumnModelList<Author>();
                AuthorColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AuthorColumns.Add(x => x.ImageUrl).SetHidden(true);
                AuthorColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                AuthorColumns.Add(x => x.Pic).SetCaption("تصویر").SetWidth("200");
                AuthorColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                AuthorColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                AuthorColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                AuthorColumns.Add(x => x.Mobile).SetCaption("تلفن همراه").SetWidth("100");
                AuthorColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
            }
            return AuthorColumns;
        }
        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author cartable = AuthorBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Authors/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                var model = new Author();
                return View(model);
            }
            Author cartable = AuthorBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                var model = new Author();
                return View(model);
            }
            return View(cartable);
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public ActionResult Create(Author model, System.Web.HttpPostedFileBase File)
        {
            if (model.Id == 0)
            {
                model.ImageUrl = SaveFile(File, pathFile.Author);
            }
            else
            {
                model.ImageUrl = EditFile(File, pathFile.Author, model.ImageUrl);
            }
            if(!string.IsNullOrEmpty(model.BirthDay_l))
            {
                model.BirthDay_l = model.BirthDay_l.ToEnglishNumber();
                model.BirthDay = model.BirthDay_l.PersianStringDateToDatetime(true);
            }
            AuthorBiz.Instance.Save(model);
            return RedirectToAction("Index", "Authors");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author user = AuthorBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuthorBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Authors");
        }
    }

}