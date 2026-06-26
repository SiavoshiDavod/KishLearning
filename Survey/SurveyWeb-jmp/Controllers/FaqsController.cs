using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;

namespace SurveyWeb.Controllers
{
    public class FaqsController : BaseAdminController
    {


        // GET: Faqs
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.FaqBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                FaqData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Faq> FaqColumns { get; private set; } = GetFaqColumns();
        public static GridColumnModelList<Faq> GetFaqColumns()
        {
            if (FaqColumns == null)
            {
                FaqColumns = new GridColumnModelList<Faq>();
                FaqColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                FaqColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                FaqColumns.Add(x => x.Question).SetCaption("سوال").SetWidth("300");
                FaqColumns.Add(x => x.Answer).SetCaption("پاسخ").SetWidth("300");
            }
            return FaqColumns;
        }
        // GET: Faqs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq cartable = await FaqBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Faqs/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Faq cartable = await FaqBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Faqs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Faq user)
        {
            await FaqBiz.Instance.Save(user);
            return RedirectToAction("Index", "Faqs");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq user = await FaqBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await FaqBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Faqs");
        }
    }
}

