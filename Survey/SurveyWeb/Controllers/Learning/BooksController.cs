using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class BooksController : BaseController
    {
        public ActionResult Index(string title, int? groupId, int? publisherId, string author, string keyword)
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
            ViewBag.Author = author;
            return View();
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