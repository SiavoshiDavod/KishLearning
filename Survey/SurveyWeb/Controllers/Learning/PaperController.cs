using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class PaperController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPapers(string title, int? groupId, int? qualityId, int? universityId, int? journalId, int? trendId, int? fieldId, int? publisherId, int skip = 0, int take = 10)
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

            var list = Biz.PaperBiz.Instance.GetPapers(title,titleE, titleF, groupId??0, qualityId ?? 0, universityId ?? 0, journalId ?? 0, trendId ?? 0, fieldId ?? 0, publisherId ?? 0, skip, take);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.Item2 / take),
                Page = skip + 1,
                Records = list.Item2,
                Rows = list.Item1.ToArray()
            },
         JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Detail(int id, string title)
        {
            var obj = Biz.PaperBiz.Instance.GetInclude(new Models.Paper { Id = id }, new string[] { "TranslateQuality", "University", "Journal", "Publisher", "Field", "Group" });

            if (obj.TrendArr?.Length > 0)
            {
                var Tids = new List<int>();
                foreach (var item in obj.TrendArr)
                {
                    Tids.Add(int.Parse(item));
                }
                obj.TrendNames = Biz.PaperTrendBiz.Instance.GetAll(x => Tids.Contains(x.Id)).Select(z => z.DropDownTitle);
            }
            await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Paper);
            return View(obj);
        }
    }
}