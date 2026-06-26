using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;

namespace SenakLearn.Controllers
{
    public class AzmoonQuestionsController : BaseAdminController
    {
        // GET: AzmoonQuestions
        public ActionResult Index( int AzmoonEntityId)
        {
            return View(new AzmoonQuestion() { AzmoonEntityId=AzmoonEntityId});
        }
        public ActionResult LoadList(GridSettings grid, int AzmoonEntityId)
        {
            var list = Biz.AzmoonQuestionBiz.Instance.GetAllPagedList(grid, AzmoonEntityId);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonQuestionData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<AzmoonQuestion> AzmoonQuestionColumns { get; private set; } = GetAzmoonQuestionColumns();
        public static GridColumnModelList<AzmoonQuestion> GetAzmoonQuestionColumns()
        {
            if (AzmoonQuestionColumns == null)
            {
                AzmoonQuestionColumns = new GridColumnModelList<AzmoonQuestion>();
                AzmoonQuestionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                AzmoonQuestionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                AzmoonQuestionColumns.Add(x => x.AzmoonGroupQuestionId).SetHidden(true);
                AzmoonQuestionColumns.Add(x => x.AzmoonEntityId).SetHidden(true);
                AzmoonQuestionColumns.Add(x => x.AzmoonOrder).SetCaption("ترتیب").SetWidth("50");
                AzmoonQuestionColumns.Add(x => x.Score).SetCaption("نمره").SetWidth("50");
                AzmoonQuestionColumns.Add(x => x.Width).SetCaption("عرض").SetWidth("50");
                AzmoonQuestionColumns.Add(x => x.Height).SetCaption("طول").SetWidth("50");
                AzmoonQuestionColumns.Add(x => x.Question).SetCaption("سوال").SetWidth("300");
                AzmoonQuestionColumns.Add(x => x.QuestionTypeName).SetCaption("نوع").SetWidth("100");
                AzmoonQuestionColumns.Add(x => x.required).SetCaption("اجباری بودن").SetWidth("50");
            }
            return AzmoonQuestionColumns;
        }
        // GET: AzmoonQuestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonQuestion user = await AzmoonQuestionBiz.Instance.GetIncludeOptions(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Layout= "~/Views/Shared/_LayoutAdmin.cshtml";
            return View("_a" + user.QuestionType.ToString(), user);
        }

        public ActionResult New(QuestionEnum type,int AzmoonEntityId)
        {
            return View("Create",new AzmoonQuestion() { QuestionType=type, AzmoonEntityId = AzmoonEntityId });
        }

        // GET: AzmoonQuestions/Create
        public async Task<ActionResult> Create(int AzmoonEntityId,int? id)
        {
            if (id == null)
            {
                return View(new AzmoonQuestion() { AzmoonEntityId = AzmoonEntityId });
            }
            AzmoonQuestion azmoonQuestion =await  AzmoonQuestionBiz.Instance.GetIncludeOptions(id.Value);
            if (azmoonQuestion == null)
            {
                return View(new AzmoonQuestion() { AzmoonEntityId = AzmoonEntityId });
            }
            return View(azmoonQuestion);
        }

        // POST: AzmoonQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AzmoonQuestion user, System.Web.HttpPostedFileBase File, System.Web.HttpPostedFileBase FileA)
        {
            if (user.Id == 0)
            {
                await AzmoonQuestionBiz.Instance.WelcomeGoodbyeValidation(user);
                user.QuestionImageUrl = SaveFile(File, pathFile.Question);
                user.AnswerImageUrl = SaveFile(FileA, pathFile.Answer);
            }
            else
            {
                user.QuestionImageUrl = EditFile(File, pathFile.Question, user.QuestionImageUrl);
                user.AnswerImageUrl = EditFile(FileA, pathFile.Answer, user.AnswerImageUrl);
            }
            var newObj=await AzmoonQuestionBiz.Instance.Save(user);
            await AzmoonEntityBiz.Instance.AddQuestion(newObj.AzmoonEntityId);
            user = await AzmoonQuestionBiz.Instance.GetIncludeOptions(newObj.Id);
            return View(user);
           // return RedirectToAction("Index", "AzmoonQuestions",new { AzmoonEntityId= user.AzmoonEntityId });
        }

        // GET: AzmoonQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonQuestion user = await AzmoonQuestionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AzmoonQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           var AzmoonEntityId= await AzmoonQuestionBiz.Instance.Remove(id);
            return RedirectToAction("Index", "AzmoonQuestions", new { AzmoonEntityId = AzmoonEntityId });
        }
    }
}
