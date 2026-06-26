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
    public class SurveyEntitysController : BaseAdminController
    {
        // GET: SurveyEntitys
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.SurveyEntityBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyEntityData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyEntity> SurveyEntityColumns { get; private set; } = GetSurveyEntityColumns();
        public static GridColumnModelList<SurveyEntity> GetSurveyEntityColumns()
        {
            if (SurveyEntityColumns == null)
            {
                SurveyEntityColumns = new GridColumnModelList<SurveyEntity>();
                SurveyEntityColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                SurveyEntityColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                SurveyEntityColumns.Add(x => x.Name).SetCaption("نام").SetWidth("300");
                SurveyEntityColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("200");
                SurveyEntityColumns.Add(x => x.StatusName).SetCaption("وضعیت").SetWidth("100");
                SurveyEntityColumns.Add(x => x.IsIpRestriction).SetCaption("محدویت آی پی").SetWidth("50");
				SurveyEntityColumns.Add(x => x.IsShowInSinglePage).SetCaption("تک صفحه ای").SetWidth("50");
                
                SurveyEntityColumns.Add(x => x.IsUserMustBeLogin).SetCaption("لاگین کاربر").SetWidth("50");
                SurveyEntityColumns.Add(x => x.IsFavorite).SetCaption("محبوب").SetWidth("50");
                SurveyEntityColumns.Add(x => x.IsImportant).SetCaption("مهم").SetWidth("50");
                SurveyEntityColumns.Add(x => x.QuestionCount).SetCaption("تعداد پرسشنامه").SetWidth("100");
                SurveyEntityColumns.Add(x => x.AnswerCount).SetCaption("تعداد پاسخنامه").SetWidth("100");
                SurveyEntityColumns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ ایجاد").SetWidth("100");
                SurveyEntityColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("200");
            }
            return SurveyEntityColumns;
        }
        // GET: SurveyEntitys/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyEntity obl = await SurveyEntityBiz.Instance.GetIncludeQuestion(id.Value, HttpContext.Request.UserHostAddress,Current_UserId);
            if (obl == null)
            {
                return HttpNotFound();
            }
            
            return View(obl);
        }

        // GET: SurveyEntitys/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            SurveyEntity user = await SurveyEntityBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: SurveyEntitys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SurveyEntity user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.SurveyImageUrl = SaveFile(File, pathFile.Survey);
            }
            else
            {
                user.SurveyImageUrl = EditFile(File, pathFile.Survey, user.SurveyImageUrl);
            }
            await SurveyEntityBiz.Instance.Save(user);
            return RedirectToAction("Index", "SurveyEntitys");
        }



        // GET: SurveyEntitys/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyEntity user = await SurveyEntityBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SurveyEntitys/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await SurveyEntityBiz.Instance.Remove(id);
            return RedirectToAction("Index", "SurveyEntitys");
        }
    }
}
