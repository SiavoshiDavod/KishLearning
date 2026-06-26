using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;

namespace SurveyWeb.Controllers
{
    public class SurveyGroupQuestionsController : BaseAdminController
    {

        // GET: SurveyGroupQuestions
        public async Task<ActionResult> Index(int surveyEntityId)
        {
            ViewBag.SurveyEntityId = surveyEntityId;
            SurveyEntity obl = await SurveyEntityBiz.Instance.GetInclude(new SurveyEntity() { Id = surveyEntityId }, "SurveyGroupQuestion");
            return View(obl.SurveyGroupQuestion);
        }

        //// GET: SurveyGroupQuestions/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SurveyGroupQuestion surveyGroupQuestion = await db.SurveyGroupQuestions.FindAsync(id);
        //    if (surveyGroupQuestion == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(surveyGroupQuestion);
        //}

        // GET: SurveyGroupQuestions/Create
        public async Task<ActionResult> Create(int surveyEntityId, int? id)
        {
            if (id == null)
            {
                return View(new SurveyGroupQuestion() { SurveyEntityId = surveyEntityId });
            }
            SurveyGroupQuestion user = await SurveyGroupQuestionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View(new SurveyGroupQuestion() { SurveyEntityId = surveyEntityId });
            }
            return View(user);

        }

        // POST: SurveyGroupQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SurveyGroupQuestion surveyGroupQuestion)
        {
            if (ModelState.IsValid)
            {
                await SurveyGroupQuestionBiz.Instance.Save(surveyGroupQuestion);
                return RedirectToAction("Index", "SurveyGroupQuestions",new { surveyEntityId = surveyGroupQuestion.SurveyEntityId });
            }

            return View(surveyGroupQuestion);
        }
        // GET: SurveyGroupQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyGroupQuestion user = await SurveyGroupQuestionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SurveyGroupQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           int surveyEntityId= await SurveyGroupQuestionBiz.Instance.Remove(id);
            return RedirectToAction("Index", "SurveyGroupQuestions", new { surveyEntityId = surveyEntityId });
        }

    }
}
