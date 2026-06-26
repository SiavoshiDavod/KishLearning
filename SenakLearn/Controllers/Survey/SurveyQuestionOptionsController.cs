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
    public class SurveyQuestionOptionsController : BaseAdminController
    {
        // GET: SurveyQuestionOptions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.SurveyQuestionOptionBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyQuestionOptionData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyQuestionOption> SurveyQuestionOptionColumns { get; private set; } = GetSurveyQuestionOptionColumns();
        public static GridColumnModelList<SurveyQuestionOption> GetSurveyQuestionOptionColumns()
        {
            if (SurveyQuestionOptionColumns == null)
            {
                SurveyQuestionOptionColumns = new GridColumnModelList<SurveyQuestionOption>();
                SurveyQuestionOptionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                SurveyQuestionOptionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                SurveyQuestionOptionColumns.Add(x => x.QuestionOption).SetCaption("نام").SetWidth("300");
            }
            return SurveyQuestionOptionColumns;
        }
        // GET: SurveyQuestionOptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyQuestionOption user = await SurveyQuestionOptionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: SurveyQuestionOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.4
        [HttpPost]
        public async Task<ActionResult> Create(string QuestionOption, int SurveyQuestionId)
        {
            await SurveyQuestionOptionBiz.Instance.Save(new SurveyQuestionOption() { QuestionOption = QuestionOption, SurveyQuestionId = SurveyQuestionId, CreatedDate = DateTime.Now });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateImage(int SurveyQuestionId, System.Web.HttpPostedFileBase File)
        {
            var name = File.FileName;
            var QuestionOption = SaveFile(File, pathFile.QuestionOption);
            if (string.IsNullOrEmpty(QuestionOption))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            await SurveyQuestionOptionBiz.Instance.Save(new SurveyQuestionOption() { QuestionOption = name, QuestionOptionUrl = QuestionOption, SurveyQuestionId = SurveyQuestionId, CreatedDate = DateTime.Now });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdatImage(int SurveyQuestionOptionId, short width, short height, string QuestionOption)
        {
            return Json(await SurveyQuestionOptionBiz.Instance.UpdateImageProperty(SurveyQuestionOptionId, width, height, QuestionOption), JsonRequestBehavior.AllowGet);
        }

        // POST: SurveyQuestionOptions/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await SurveyQuestionOptionBiz.Instance.Remove(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
