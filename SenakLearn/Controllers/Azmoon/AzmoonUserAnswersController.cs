using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.Biz.Person;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using SenakLearn.Models.Person;
using SenakLearn.Models.wrapper;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class AzmoonUserAnswersController : BaseAdminController
    {
        // GET: AzmoonUserAnswers
        public ActionResult Index()
        {
            var azmonList = AzmoonEntityBiz.Instance.DropDown();
            ViewBag.azmoonList = azmonList;
            return View();
        }
        public ActionResult LoadList(GridSettings grid,int? azmoonEntityId)
        {
            var list = Biz.AzmoonUserAnswerBiz.Instance.GetAllPagedListVm(grid, azmoonEntityId);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonUserAnswerData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyUserAnswerVM> AzmoonUserAnswerColumns { get; private set; } = GetAzmoonUserAnswerColumns();
        public static GridColumnModelList<SurveyUserAnswerVM> GetAzmoonUserAnswerColumns()
        {
            if (AzmoonUserAnswerColumns == null)
            {
                AzmoonUserAnswerColumns = new GridColumnModelList<SurveyUserAnswerVM>();
                AzmoonUserAnswerColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.Ip).SetCaption("آی پی").SetWidth("200");
                AzmoonUserAnswerColumns.Add(x => x.SurveyEntity).SetCaption("آزمون").SetWidth("200");
                AzmoonUserAnswerColumns.Add(x => x.UserName).SetCaption("نام کاربري").SetWidth("200");
                AzmoonUserAnswerColumns.Add(x => x.User).SetCaption("نام کاربر").SetWidth("200");
                AzmoonUserAnswerColumns.Add(x => x.NoAnswerd).SetCaption("پاسخ داده نشده").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.WrongAnswerd).SetCaption("پاسخ غلط").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.CorrectAnswerd).SetCaption("پاسخ صحيح").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.TotalScore).SetCaption("نمره").SetWidth("50").SetCellType(GridCellType.DECIMAL).SetColumnRenderer(new NumberColumnRenderer(2));
                AzmoonUserAnswerColumns.Add(x => x.TotalRank).SetCaption("رتبه").SetWidth("50"); 
                AzmoonUserAnswerColumns.Add(x => x.maxScore).SetCaption("حداکثر نمره ").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.TotalCorrectScore).SetCaption("جمع کل نمرات ").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.AzmounDate).SetHidden(true);

            }
            return AzmoonUserAnswerColumns;
        }
        // GET: AzmoonUserAnswers/Details/5
        public async Task<ActionResult> Calculator(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonUserAnswer user = await AzmoonUserAnswerBiz.Instance.GetInclude(new AzmoonUserAnswer() { Id = id.Value }, "AzmoonAnswers", "AzmoonEntity.AzmoonQuestions.AzmoonQuestionOptions", "User");
            if (user == null)
            {
                return HttpNotFound();
            }
            user.TotalCount = await AzmoonUserAnswerBiz.Instance.GetTotalCount(user.AzmoonEntityId);
            return View(user);
        }

        public async Task<ActionResult> ReCalculator(int id,double score)
        {
            try
            {
                await AzmoonAnswerBiz.Instance.EditScore(id,score);
                return Json(new ApiJsonResult { success = true, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ApiJsonResult { success = false, ErrorMessage = e.Message, InnerExceptionMessage = ExceptionExtensions.GetStackTraceWithMessage(e) }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonUserAnswer user = await AzmoonUserAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: AzmoonUserAnswers/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            AzmoonUserAnswer user = await AzmoonUserAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: AzmoonUserAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AzmoonUserAnswer user, System.Web.HttpPostedFileBase File)
        {
            await AzmoonUserAnswerBiz.Instance.Save(user);
            var azmonList = AzmoonEntityBiz.Instance.DropDown();
            ViewBag.azmoonList = azmonList;
            return RedirectToAction("Index", "AzmoonUserAnswers");
        }



        // GET: AzmoonUserAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonUserAnswer user = await AzmoonUserAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AzmoonUserAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AzmoonUserAnswerBiz.Instance.Remove(id);
            var azmonList = AzmoonEntityBiz.Instance.DropDown();
            ViewBag.azmoonList = azmonList;
            return RedirectToAction("Index", "AzmoonUserAnswers");
        }

        public ActionResult GetReportExcel(int EntityId)
        {

            var memoryStream = AzmoonUserAnswerBiz.Instance.GetAllReportExcel(EntityId);

            FileContentResult result = new FileContentResult(memoryStream.ToArray(), "application/vnd.ms-excel")
            {
                FileDownloadName = "Azmoon_User_Answer_" + DateTime.Now.ToShortDateString() + ".xlsx"
            };
            return result;
        }
    }
}
