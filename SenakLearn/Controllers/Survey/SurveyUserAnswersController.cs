using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;
using SenakLearn.Models.wrapper;

namespace SenakLearn.Controllers
{
    public class SurveyUserAnswersController : BaseAdminController
    {
        // GET: SurveyUserAnswers
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.SurveyUserAnswerBiz.Instance.GetAllPagedListVm(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyUserAnswerData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<SurveyUserAnswerVM> SurveyUserAnswerColumns { get; private set; } = GetSurveyUserAnswerColumns();
        public static GridColumnModelList<SurveyUserAnswerVM> GetSurveyUserAnswerColumns()
        {
            if (SurveyUserAnswerColumns == null)
            {
                SurveyUserAnswerColumns = new GridColumnModelList<SurveyUserAnswerVM>();
               // SurveyUserAnswerColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                SurveyUserAnswerColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                SurveyUserAnswerColumns.Add(x => x.Ip).SetCaption("آی پی").SetWidth("200");
                SurveyUserAnswerColumns.Add(x => x.SurveyEntity).SetCaption("نظرسنجی").SetWidth("200");
                SurveyUserAnswerColumns.Add(x => x.User).SetCaption("نام کاربر").SetWidth("200");
                SurveyUserAnswerColumns.Add(x => x.UserName).SetCaption("نام کاربري").SetWidth("200");
            }
            return SurveyUserAnswerColumns;
        }
        // GET: SurveyUserAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyUserAnswer user = await SurveyUserAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: SurveyUserAnswers/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            SurveyUserAnswer user = await SurveyUserAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: SurveyUserAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SurveyUserAnswer user, System.Web.HttpPostedFileBase File)
        {
            //if (user.Id == 0)
            //{
            //    user.UserImageUrl = SaveFile(File, pathFile.User);
            //}
            //else
            //{
            //    user.UserImageUrl = EditFile(File, pathFile.User, user.UserImageUrl);
            //}
            await SurveyUserAnswerBiz.Instance.Save(user);
            return RedirectToAction("Index", "SurveyUserAnswers");
        }



        // GET: SurveyUserAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyUserAnswer user = await SurveyUserAnswerBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SurveyUserAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await SurveyUserAnswerBiz.Instance.Remove(id);
            return RedirectToAction("Index", "SurveyUserAnswers");
        }
    }
}
