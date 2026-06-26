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
    public class BookController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.BookBiz.Instance.GetAllPagedList(grid);
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
        public ActionResult LoadListSlider(GridSettings grid)
        {
            var list = Biz.BookSlideBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<Book> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<Book> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<Book>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("250");
                Columns.Add(x => x.Title).SetCaption("نام انگلیسی").SetWidth("300");
                Columns.Add(x => x.TitleF).SetCaption("نام فارسی").SetWidth("300");
                Columns.Add(x => x.Author).SetCaption("نویسنده").SetWidth("200");
                //Columns.Add(x => x.IsSlider).SetCaption("اسلایدر").SetWidth("200");
            }
            return Columns;
        }

        //

        public static GridColumnModelList<BookSlideModel> Column‌Books { get; private set; } = GetColumnBooks();
        public static GridColumnModelList<BookSlideModel> GetColumnBooks()
        {
            if (Column‌Books == null)
            {
                Column‌Books = new GridColumnModelList<BookSlideModel>();
                Column‌Books.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Column‌Books.Add(x => x.act).SetCaption("عملیات").SetWidth("250");
                Column‌Books.Add(x => x.Img).SetCaption("تصویر").SetWidth("300");
                Column‌Books.Add(x => x.Url).SetCaption("لینک").SetWidth("300");
                Column‌Books.Add(x => x.Title).SetCaption("عنوان").SetWidth("300");
            }
            return Column‌Books;
        }

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book Book = Biz.BookBiz.Instance.Get(id.Value);
            if (Book == null)
            {
                return HttpNotFound();
            }
            return View(Book);
        }

        // GET: Book/Create
        public ActionResult Create(int id = 0)
        {
            Book Book = id == 0 ? new Book() : Biz.BookBiz.Instance.Get(id);
            return View(Book);
        }
        public ActionResult Slide(int id = 0)
        {
            BookSlideModel Slide = id == 0 ? new BookSlideModel() : Biz.BookSlideBiz.Instance.Get(id);

            return View(Slide);
        }
        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Create(Book Book, HttpPostedFileBase File, HttpPostedFileBase ScreenShot,  HttpPostedFileBase TranslateWord, HttpPostedFileBase TranslatePdf)
        {
            var isNew = false;
            // if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    isNew = true;
                    Book.FileId = SaveFile(File, pathFile.Book);
                    Book.ScreenShotId = SaveFile(ScreenShot, pathFile.Book);
                    //Book.SlideImg = SaveFile(SlideImgFile, pathFile.Book);
                }
                else
                {
                    Book.FileId = EditFile(File, pathFile.Book, Book.FileId);
                    Book.ScreenShotId = EditFile(ScreenShot, pathFile.Book, Book.ScreenShotId);
                    //Book.SlideImg = EditFile(SlideImgFile, pathFile.Book, Book.SlideImg);
                }
                if (string.IsNullOrEmpty(Book.FileId))
                {
                    throw new Exception("فایل اصلی را انتخاب کنید");
                }
                Biz.BookBiz.Instance.Save(Book);
                if (isNew)
                    await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Book, Book.GroupId);
                return RedirectToAction("Index");
            }

            // return View(Book);
        }

        [HttpPost]

        public async Task<ActionResult> Slide(BookSlideModel Slide, HttpPostedFileBase SlideImgFile)
        {
            var isNew = false;
            // if (ModelState.IsValid)
            {
                if (Slide.Id == 0)
                {
                    isNew = true;
                    Slide.Img = SaveFile(SlideImgFile, pathFile.Book);

                }
                else
                {
                    Slide.Img = EditFile(SlideImgFile, pathFile.Book, Slide.Img);

                }
                if (string.IsNullOrEmpty(Slide.Img))
                {
                    throw new Exception("فایل تصویر اسلایدر را انتخاب کنید");
                }
                Biz.BookSlideBiz.Instance.Save(Slide);
                return RedirectToAction("Slide");
            }

            // return View(Book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book Book = Biz.BookBiz.Instance.Get(id.Value);
            if (Book == null)
            {
                return HttpNotFound();
            }
            return View(Book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var book = Biz.BookBiz.Instance.Get(id);
            var bookGroupId = Biz.BookBiz.Instance.Remove(id);
            if (!string.IsNullOrEmpty(book.ScreenShotId))
                RemoveFile(book.ScreenShotId, pathFile.Book);
            //if (!string.IsNullOrEmpty(book.SlideImg))
            //    RemoveFile(book.SlideImg, pathFile.Book);
            await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Book, bookGroupId, false);
            return RedirectToAction("Index");
        }
        [HttpGet, ActionName("SlideDelete")]
        public async Task<ActionResult> SlideDelete(int id)
        {
            var book = Biz.BookSlideBiz.Instance.Get(id);
            var bookGroupId = Biz.BookSlideBiz.Instance.Remove(id);
            if (!string.IsNullOrEmpty(book.Img))
                RemoveFile(book.Img, pathFile.Book);

            return RedirectToAction("Slide");
        }
    }
}
