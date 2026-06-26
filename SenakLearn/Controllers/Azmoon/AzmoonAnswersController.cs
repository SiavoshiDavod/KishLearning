
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;
using System.Collections.Generic;
using System.Linq;

namespace SenakLearn.Controllers
{
    public class AzmoonAnswersController : BaseAdminController
    {
        // GET: AzmoonAnswers
        public async Task<ActionResult> Index(int AzmoonEntityId)
        {
            var obl = await AzmoonEntityBiz.Instance.GetIncludeQuestionAndAnswerFormated(AzmoonEntityId);
            if (obl == null)
            {
                return HttpNotFound();
            }
            return View(obl);
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.AzmoonAnswerBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonAnswerData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<AzmoonAnswer> AzmoonAnswerColumns { get; private set; } = GetAzmoonAnswerColumns();
        public static GridColumnModelList<AzmoonAnswer> GetAzmoonAnswerColumns()
        {
            if (AzmoonAnswerColumns == null)
            {
                AzmoonAnswerColumns = new GridColumnModelList<AzmoonAnswer>();
                AzmoonAnswerColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                AzmoonAnswerColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AzmoonAnswerColumns.Add(x => x.Result).SetCaption("نام").SetWidth("300");
            }
            return AzmoonAnswerColumns;
        }
        // GET: AzmoonAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonAnswer user = await AzmoonAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // GET: AzmoonAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonAnswer user = await AzmoonAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AzmoonAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AzmoonAnswerBiz.Instance.Remove(id);
            return RedirectToAction("Index", "AzmoonAnswers");
        }
    }
}
