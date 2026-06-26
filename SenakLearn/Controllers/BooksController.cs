using SenakLearn.Biz;
using SenakLearn.Models;
using SenakLearn.Models.Common;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class BooksController : BaseController
    {
        public ActionResult Index(string title, int? groupId, int? publisherId, string author, string keyword, int page = 1,bool filter=false)
        {
            string titleE = null;
            string titleF = null;
            if (!string.IsNullOrWhiteSpace(title))
            {
                if (regex.IsMatch(title))
                    titleF = title;
                else
                    titleE = title;
            }
            ViewBag.groupId = groupId;
            ViewBag.publisherId = publisherId;
            ViewBag.keyword = keyword;
            ViewBag.TitleE = titleE;
            ViewBag.TitleF = titleF;
            ViewBag.TitleFilter = title;
            ViewBag.Author = author;
            Pagination<Book> model = new Pagination<Book>();
            model.Data = BookBiz.Instance.GetAllBooks(i => (groupId == null || groupId == i.GroupId) && (string.IsNullOrEmpty(keyword) || i.Keyword.Contains(keyword)) && (string.IsNullOrEmpty(titleF) || i.TitleF.Contains(titleF)) && (string.IsNullOrEmpty(title) || i.Title.Contains(title)) && (string.IsNullOrEmpty(author) || i.Author.Contains(author)));
            model.CurrentPage= page;
            ViewBag.CoursGroupIds = CourseBiz.Instance.FindAllGroupDropdown();
            ViewBag.filter = filter;

            ViewBag.SliderBooks = BookBiz.Instance.GetBooksInSlider();

            return View(model);
        }

        //[HttpPost]
        //public ActionResult GetBooks(string title, int? groupId, int? publisherId, int skip = 0, int take = 10)
        //{
        //    string titleE = null;
        //    string titleF = null;
        //    if (!string.IsNullOrWhiteSpace(title))
        //    {
        //        if (regex.IsMatch(title))
        //            titleF = title;
        //        else
        //            titleE = title;
        //    }

        //    var list = Biz.BookBiz.Instance.GetBooks(title, titleE, titleF, groupId ?? 0, publisherId ?? 0, skip, take);
        //    return Json(new
        //    {
        //        Total = (int)Math.Ceiling((double)list.Item2 / take),
        //        Page = skip + 1,
        //        Records = list.Item2,
        //        Rows = list.Item1.ToArray()
        //    },
        // JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> Detail(int id, string title)
        {
            var obj = Biz.BookBiz.Instance.GetInclude(new Models.Book { Id = id }, new string[] { "Publisher", "Group" });
            await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Book);

            return View(obj);
        }
    }
}