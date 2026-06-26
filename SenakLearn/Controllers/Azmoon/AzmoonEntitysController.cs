using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class AzmoonEntitysController : BaseAdminController
    {
        // GET: AzmoonEntitys
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.AzmoonEntityBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonEntityData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<AzmoonEntity> AzmoonEntityColumns { get; private set; } = GetAzmoonEntityColumns();
        public static GridColumnModelList<AzmoonEntity> GetAzmoonEntityColumns()
        {
            if (AzmoonEntityColumns == null)
            {
                AzmoonEntityColumns = new GridColumnModelList<AzmoonEntity>();
                AzmoonEntityColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                AzmoonEntityColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("130");
                AzmoonEntityColumns.Add(x => x.Name).SetCaption("نام").SetWidth("200");
                AzmoonEntityColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("200");
                AzmoonEntityColumns.Add(x => x.StatusName).SetCaption("وضعیت").SetWidth("100");
                AzmoonEntityColumns.Add(x => x.IsIpRestriction).SetCaption("محدویت آی پی").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.IsShowInSinglePage).SetCaption("تک صفحه ای").SetWidth("50");

                AzmoonEntityColumns.Add(x => x.IsUserMustBeLogin).SetCaption("لاگین کاربر").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.IsFavorite).SetCaption("محبوب").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.IsImportant).SetCaption("مهم").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.QuestionCount).SetCaption("تعداد آزمون").SetWidth("100");
                AzmoonEntityColumns.Add(x => x.AnswerCount).SetCaption("تعداد پاسخنامه").SetWidth("100");
                AzmoonEntityColumns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ ایجاد").SetWidth("100");
                AzmoonEntityColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.MinScore).SetCaption("حداقل نمره قبولی").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.MaxScore).SetCaption("حداکثر نمره").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.TotalScore).SetCaption("جمع کل نمرات").SetWidth("50");
                AzmoonEntityColumns.Add(x => x.ZaribManfi).SetCaption("ضريب نمره منفي").SetWidth("50");
            }
            return AzmoonEntityColumns;
        }
        // GET: AzmoonEntitys/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonEntity obl =  AzmoonEntityBiz.Instance.GetIncludeQuestion(id.Value, HttpContext.Request.UserHostAddress, Current_learn_userId);
            var now = DateTime.Now;
            if (obl.FromDate != null && obl.FromDate > now)
            {
                SetViewBagErrorMessage("تاریخ و زمان شروع آزمون فرا نرسیده است  !");
                return View("index");
            }
            if (obl.ToDate != null && obl.ToDate < now)
            {

                SetViewBagErrorMessage("تاریخ و زمان آزمون پایان یافته است  !");
                return View("index");
            }
            if (obl == null)
            {
                return HttpNotFound();
            }

            return View(obl);
        }

        // GET: AzmoonEntitys/Create
        public async Task<ActionResult> Create(int? id)
        {
            AzmoonEntity model = null;
            if (id == null)
            {
                model = new AzmoonEntity();
                return View(model);
            }
            model = await AzmoonEntityBiz.Instance.Get(id.Value);
            if (model.FromDate != null)
                model.FromDate_l = model.FromDate.Value.GeogianToPersianString();
            if (model.ToDate != null)
                model.ToDate_l = model.ToDate.Value.GeogianToPersianString();
            if (model == null)
            {
                model = new AzmoonEntity();
                return View(model);
            }
            return View(model);
        }

        // POST: AzmoonEntitys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AzmoonEntity model, System.Web.HttpPostedFileBase File, HttpPostedFileBase CerFile)
        {
            if (!string.IsNullOrEmpty(model.FromDate_l))
            {
                model.FromDate = model.FromDate_l.PersianStringDateToDatetime();
            }
            var files=Request.Files;
            if (!string.IsNullOrEmpty(model.ToDate_l))
            {
                model.ToDate = model.ToDate_l.PersianStringDateToDatetime();
            }
            if (model.FromDate != null && model.ToDate != null && model.FromDate > model.ToDate)
            { throw new Exception("تاریخ شروع باید کوچکتر از تاریخ پایان باشد !"); }

            if (model.Id == 0)
            {
                model.AzmoonImageUrl = SaveFile(File, pathFile.Azmoon);
                model.AzmoonCerImageUrl = SaveFile(CerFile, pathFile.AzmoonCer);
            }
            else
            {
                model.AzmoonImageUrl = EditFile(File, pathFile.Azmoon, model.AzmoonImageUrl);
                model.AzmoonCerImageUrl = EditFile(CerFile, pathFile.AzmoonCer, model.AzmoonCerImageUrl);
            }
            if (model.Id == 0)
            {
                
            }
            else
            {
               
            }
            await AzmoonEntityBiz.Instance.Save(model);
            return RedirectToAction("Index", "AzmoonEntitys");
        }



        // GET: AzmoonEntitys/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzmoonEntity user = await AzmoonEntityBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AzmoonEntitys/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AzmoonEntityBiz.Instance.Remove(id);
            return RedirectToAction("Index", "AzmoonEntitys");
        }
    }
}
