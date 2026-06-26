using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;

namespace SenakLearn.Controllers
{
    public class AzmoonGroupQuestionsController : BaseAdminController
    {

        // GET: AzmoonGroupQuestions
        public async Task<ActionResult> Index()
        {
            //ViewBag.AzmoonEntityId = AzmoonEntityId;
            var models =  AzmoonGroupQuestionBiz.Instance.GetAllGroupQuestion();
            return View(models);
        }

        //// GET: AzmoonGroupQuestions/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AzmoonGroupQuestion AzmoonGroupQuestion = await db.AzmoonGroupQuestions.FindAsync(id);
        //    if (AzmoonGroupQuestion == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(AzmoonGroupQuestion);
        //}

        // GET: AzmoonGroupQuestions/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View(new AzmoonGroupQuestion() { });
            }
            AzmoonGroupQuestion model = await AzmoonGroupQuestionBiz.Instance.Get(id.Value);
            if (model == null)
            {
                return View(new AzmoonGroupQuestion() { });
            }
            return View(model);

        }

        // POST: AzmoonGroupQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AzmoonGroupQuestion AzmoonGroupQuestion)
        {
            if (ModelState.IsValid)
            {
                await AzmoonGroupQuestionBiz.Instance.Save(AzmoonGroupQuestion);
                return RedirectToAction("Index", "AzmoonGroupQuestions", new { });
            }

            return View(AzmoonGroupQuestion);
        }
        // GET: AzmoonGroupQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonGroupQuestion user = await AzmoonGroupQuestionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AzmoonGroupQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int resault = await AzmoonGroupQuestionBiz.Instance.Remove(id);
            return RedirectToAction("Index", "AzmoonGroupQuestions", new { });
        }

    }
}
