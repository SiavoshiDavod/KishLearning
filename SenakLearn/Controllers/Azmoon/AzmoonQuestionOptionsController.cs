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
    public class AzmoonQuestionOptionsController : BaseAdminController
    {
        // GET: AzmoonQuestionOptions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.AzmoonQuestionOptionBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonQuestionOptionData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<AzmoonQuestionOption> AzmoonQuestionOptionColumns { get; private set; } = GetAzmoonQuestionOptionColumns();
        public static GridColumnModelList<AzmoonQuestionOption> GetAzmoonQuestionOptionColumns()
        {
            if (AzmoonQuestionOptionColumns == null)
            {
                AzmoonQuestionOptionColumns = new GridColumnModelList<AzmoonQuestionOption>();
                AzmoonQuestionOptionColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                AzmoonQuestionOptionColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AzmoonQuestionOptionColumns.Add(x => x.QuestionOption).SetCaption("نام").SetWidth("300");
            }
            return AzmoonQuestionOptionColumns;
        }
        // GET: AzmoonQuestionOptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonQuestionOption user = await AzmoonQuestionOptionBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: AzmoonQuestionOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.4
        [HttpPost]
        public async Task<ActionResult> Create(string QuestionOption, int AzmoonQuestionId)
        {
            await AzmoonQuestionOptionBiz.Instance.Save(new AzmoonQuestionOption() { QuestionOption = QuestionOption, AzmoonQuestionId = AzmoonQuestionId, CreatedDate = DateTime.Now });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateImage(int AzmoonQuestionId, System.Web.HttpPostedFileBase File)
        {
            var name = File.FileName;
            var QuestionOption = SaveFile(File, pathFile.QuestionOption);
            if (string.IsNullOrEmpty(QuestionOption))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            await AzmoonQuestionOptionBiz.Instance.Save(new AzmoonQuestionOption() { QuestionOption = name, QuestionOptionUrl = QuestionOption, AzmoonQuestionId = AzmoonQuestionId, CreatedDate = DateTime.Now });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdatImage(int AzmoonQuestionOptionId, short width, short height, string QuestionOption)
        {
            return Json(await AzmoonQuestionOptionBiz.Instance.UpdateImageProperty(AzmoonQuestionOptionId, width, height, QuestionOption), JsonRequestBehavior.AllowGet);
        }

        // POST: AzmoonQuestionOptions/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var res = await AzmoonQuestionOptionBiz.Instance.Remove(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // POST: AzmoonQuestionOptions/SetCorrect/5
        [HttpPost]
        public async Task<ActionResult> SetCorrect(int id, bool iscorrect)
        {
            var res = await AzmoonQuestionOptionBiz.Instance.SetCorrect(id, iscorrect);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
