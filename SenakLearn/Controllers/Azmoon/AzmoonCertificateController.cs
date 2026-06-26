using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models.Azmoon;
using SenakLearn.Models.wrapper;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class AzmoonCertificateController : BaseAdminController
    {
        public async Task<ActionResult> Index(int entityId)
        {
            var entity =await AzmoonEntityBiz.Instance.GetAzmoon(entityId);
            return View(entity);
        }

        public ActionResult LoadList(GridSettings grid, int azmoonEntityId)
        {
            var list = Biz.AzmoonCertificateBiz.Instance.GetAllPagedList(grid, azmoonEntityId);
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
        public static GridColumnModelList<AzmoonEntityWrapper> AzmoonCertificateColumns { get; private set; } = GetAzmoonUserAnswerColumns();
        public static GridColumnModelList<AzmoonEntityWrapper> GetAzmoonUserAnswerColumns()
        {
            if (AzmoonCertificateColumns == null)
            {
                AzmoonCertificateColumns = new GridColumnModelList<AzmoonEntityWrapper>();
                AzmoonCertificateColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("80");
                AzmoonCertificateColumns.Add(x => x.AzmoonUserAnswerId).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AzmoonCertificateColumns.Add(x => x.AzmoonEntityName).SetCaption("آزمون").SetWidth("200");
                AzmoonCertificateColumns.Add(x => x.TimeDuration).SetCaption("مدت آزمون").SetWidth("200");
                AzmoonCertificateColumns.Add(x => x.UserName).SetCaption("نام کاربري").SetWidth("200");
                AzmoonCertificateColumns.Add(x => x.NameFamily).SetCaption("نام").SetWidth("200");
                AzmoonCertificateColumns.Add(x => x.TotalScore).SetCaption("نمره").SetWidth("50").SetCellType(GridCellType.DECIMAL).SetColumnRenderer(new NumberColumnRenderer(2));
                AzmoonCertificateColumns.Add(x => x.TotalRank).SetCaption("رتبه").SetWidth("50");
                AzmoonCertificateColumns.Add(x => x.AzmounDatePersian).SetCaption("تاریخ آزمون ").SetWidth("80");
                AzmoonCertificateColumns.Add(x => x.AcceptedDatePersian).SetCaption("تاریخ گواهینامه ").SetWidth("80");
                AzmoonCertificateColumns.Add(x => x.act2).SetCaption("گواهینامه").SetWidth("100");

            }
            return AzmoonCertificateColumns;
        }

        public async Task<FileResult> Print( int userAnswerId)
        {
            string cerPath = Server.MapPath("/images/" + pathFile.AzmoonCer + "/");
            string fontPath = Server.MapPath("/fonts/");
            //پرینت مدرک
            var result = await AzmoonUserAnswerBiz.Instance.GetCertificate(userAnswerId, cerPath, fontPath);
            return File(result.Content, result.FileName);
        }
        public async Task<ActionResult> Accept(int userAnswerId)
        {
            var result = await AzmoonUserAnswerBiz.Instance.Accept(userAnswerId,Current_learn_userId);
            return Json( result,JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Reject(int userAnswerId)
        {
            var result = await AzmoonUserAnswerBiz.Instance.Reject(userAnswerId, Current_learn_userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

}