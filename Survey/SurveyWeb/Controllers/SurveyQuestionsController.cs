using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;

namespace SurveyWeb.Controllers
{
    public class SurveyQuestionsController : BaseAdminController
    {
        // GET: SurveyQuestions
        public ActionResult Index( int surveyEntityId)
        {
            return View(new SurveyQuestion() { SurveyEntityId=surveyEntityId});
        }
        public ActionResult LoadList(GridSettings grid, int surveyEntityId)
        {
            var list = Biz.SurveyQuestionBiz.Instance.GetAllPagedList(grid, surveyEntityId);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyQuestionData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyQuestion> SurveyQuestionColumns { get; private set; } = GetSurveyQuestionColumns();
        public static GridColumnModelList<SurveyQuestion> GetSurveyQuestionColumns()
        {
            if (SurveyQuestionColumns == null)
            {
                SurveyQuestionColumns = new GridColumnModelList<SurveyQuestion>();
                SurveyQuestionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                SurveyQuestionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                SurveyQuestionColumns.Add(x => x.SurveyGroupQuestionId).SetHidden(true);
                SurveyQuestionColumns.Add(x => x.SurveyEntityId).SetHidden(true);
                SurveyQuestionColumns.Add(x => x.SurveyOrder).SetCaption("ترتیب").SetWidth("50");
                SurveyQuestionColumns.Add(x => x.Width).SetCaption("عرض").SetWidth("50");
                SurveyQuestionColumns.Add(x => x.Height).SetCaption("طول").SetWidth("50");
                SurveyQuestionColumns.Add(x => x.Question).SetCaption("سوال").SetWidth("300");
                SurveyQuestionColumns.Add(x => x.QuestionTypeName).SetCaption("نوع").SetWidth("100");
                SurveyQuestionColumns.Add(x => x.required).SetCaption("اجباری بودن").SetWidth("50");
            }
            return SurveyQuestionColumns;
        }
        // GET: SurveyQuestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyQuestion user = await SurveyQuestionBiz.Instance.GetIncludeOptions(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Layout= "~/Views/Shared/_LayoutAdmin.cshtml";
            return View("_p" + user.QuestionType.ToString(), user);
        }

        public ActionResult New(QuestionEnum type,int surveyEntityId)
        {
            return View("Create",new SurveyQuestion() { QuestionType=type, SurveyEntityId = surveyEntityId });
        }

        // GET: SurveyQuestions/Create
        public async Task<ActionResult> Create(int surveyEntityId,int? id)
        {
            if (id == null)
            {
                return View(new SurveyQuestion() { SurveyEntityId = surveyEntityId });
            }
            SurveyQuestion user =await  SurveyQuestionBiz.Instance.GetIncludeOptions(id.Value);
            if (user == null)
            {
                return View(new SurveyQuestion() { SurveyEntityId = surveyEntityId });
            }
            return View(user);
        }

        // POST: SurveyQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SurveyQuestion user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                await SurveyQuestionBiz.Instance.WelcomeGoodbyeValidation(user);
                user.QuestionImageUrl = SaveFile(File, pathFile.Question);
            }
            else
            {
                user.QuestionImageUrl = EditFile(File, pathFile.Question, user.QuestionImageUrl);
            }
            var newObj=await SurveyQuestionBiz.Instance.Save(user);
            await SurveyEntityBiz.Instance.AddQuestion(newObj.SurveyEntityId);
            user = await SurveyQuestionBiz.Instance.GetIncludeOptions(newObj.Id);
            return View(user);
           // return RedirectToAction("Index", "SurveyQuestions",new { surveyEntityId= user.SurveyEntityId });
        }



        // GET: SurveyQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyQuestion user = await SurveyQuestionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SurveyQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           var surveyEntityId= await SurveyQuestionBiz.Instance.Remove(id);
            return RedirectToAction("Index", "SurveyQuestions", new { surveyEntityId = surveyEntityId });
        }
    }
}
