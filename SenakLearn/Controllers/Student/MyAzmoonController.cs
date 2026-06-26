using AdobeConnectService;
using DocumentFormat.OpenXml.Wordprocessing;
using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using SenakLearn.Models.wrapper;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Student
{
    public class MyAzmoonController : BaseProfileController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = AzmoonUserAnswerBiz.Instance.GetAllAzmoonByUserId(grid, Current_learn_userId);
            //var count = Biz.AzmoonBiz.Instance.Count;
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                UserData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonUserAnswer user = await AzmoonUserAnswerBiz.Instance.GetInclude(new AzmoonUserAnswer() { Id = id.Value }, "AzmoonAnswers", "AzmoonEntity.AzmoonQuestions.AzmoonQuestionOptions");
            if (user == null)
            {
                return HttpNotFound();
            }
            var azmoonEntity = await AzmoonEntityBiz.Instance.Get(user.AzmoonEntityId);
            if (azmoonEntity == null)
            {
                return HttpNotFound();
            }
            var now = DateTime.Now;
            if(azmoonEntity.FromDate!=null && azmoonEntity.ToDate!=null && azmoonEntity.FromDate<=now && azmoonEntity.ToDate>=now)
            {
                throw new WebException("تا پایان زمان آزمون امکان مشاهده آزمون را ندارید!");
            }
            user.TotalCount = await AzmoonUserAnswerBiz.Instance.GetTotalCount(user.AzmoonEntityId);
            return View(user);
        }
        [HttpGet]
        public async Task<ActionResult> LoadAttach(int questionId)
        {
            var result=await AzmoonQuestionBiz.Instance.Get(questionId);
            if (result.QuestionImageUrl != null || result.AnswerImageUrl!=null)
            {
                return View("~/Views/MyAzmoon/Attachment.cshtml", result);
            }
            return Json(new{ },JsonRequestBehavior.AllowGet);
        }

        public static GridColumnModelList<SurveyUserAnswerVM> AzmoonUserAnswerColumns { get; private set; } = GetColumns();
        public static GridColumnModelList<SurveyUserAnswerVM> GetColumns()
        {
            if (AzmoonUserAnswerColumns == null)
            {
                AzmoonUserAnswerColumns = new GridColumnModelList<SurveyUserAnswerVM>();
                AzmoonUserAnswerColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.Ip).SetCaption("آی پی").SetWidth("200");
                AzmoonUserAnswerColumns.Add(x => x.SurveyEntity).SetCaption("آزمون").SetWidth("200");
                AzmoonUserAnswerColumns.Add(x => x.NoAnswerd).SetCaption("پاسخ داده نشده").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.WrongAnswerd).SetCaption("پاسخ غلط").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.CorrectAnswerd).SetCaption("پاسخ صحيح").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.TotalRank).SetCaption("رتبه").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.maxScore).SetCaption("حداکثر نمره ").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.TotalCorrectScore).SetCaption("جمع کل نمرات ").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.minScore).SetCaption("حداقل نمره قبولی").SetWidth("50");
                AzmoonUserAnswerColumns.Add(x => x.zaribManfi).SetCaption("ضریب منفی").SetWidth("50").SetCellType(GridCellType.DECIMAL).SetColumnRenderer(new NumberColumnRenderer(2));
                AzmoonUserAnswerColumns.Add(x => x.TotalScore).SetCaption("نمره نهایی").SetWidth("50").SetCellType(GridCellType.DECIMAL).SetColumnRenderer(new NumberColumnRenderer(2));
                AzmoonUserAnswerColumns.Add(x => x.AzmounDatePersian).SetCaption("تاریخ آزمون ").SetWidth("100");
                AzmoonUserAnswerColumns.Add(x => x.AcceptedDatePersian).SetCaption("تاریخ گواهینامه ").SetWidth("100");
                AzmoonUserAnswerColumns.Add(x => x.act2).SetCaption("گواهینامه").SetWidth("100");
            }
            return AzmoonUserAnswerColumns;
        }
        public async Task<FileResult> Print(int userAnswerId)
        {
            string cerPath = Server.MapPath("/images/" + pathFile.AzmoonCer + "/" );
            string fontPath = Server.MapPath("/fonts/" );
            //پرینت مدرک
            var result =await AzmoonUserAnswerBiz.Instance.GetCertificate(userAnswerId, cerPath, fontPath);
            return File(result.Content,  result.FileName);

        }
    }
}