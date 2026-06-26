
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;
using System.Collections.Generic;
using System.Linq;

namespace SurveyWeb.Controllers
{
    public class SurveyAnswersController : BaseAdminController
    {
        // GET: SurveyAnswers
        public async Task<ActionResult> Index(int surveyEntityId)
        {
            SurveyEntity obl = await SurveyEntityBiz.Instance.GetIncludeQuestionAndAnswer(surveyEntityId);
            if (obl == null)
            {
                return HttpNotFound();
            }
            return View(obl);
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.SurveyAnswerBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyAnswerData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyAnswer> SurveyAnswerColumns { get; private set; } = GetSurveyAnswerColumns();
        public static GridColumnModelList<SurveyAnswer> GetSurveyAnswerColumns()
        {
            if (SurveyAnswerColumns == null)
            {
                SurveyAnswerColumns = new GridColumnModelList<SurveyAnswer>();
                SurveyAnswerColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                SurveyAnswerColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                SurveyAnswerColumns.Add(x => x.Result).SetCaption("نام").SetWidth("300");
            }
            return SurveyAnswerColumns;
        }
        // GET: SurveyAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyAnswer user = await SurveyAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // GET: SurveyAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyAnswer user = await SurveyAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SurveyAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await SurveyAnswerBiz.Instance.Remove(id);
            return RedirectToAction("Index", "SurveyAnswers");
        }
    }
}
