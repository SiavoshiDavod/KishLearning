using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Student
{
    public class MyPaymentController : BaseProfileController
    {
        // GET: MyPayment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.zarinpalBiz.Instance.GetAllPagedListCurrentUser(grid, Current_learn_userId);
            //var count = Biz.zarinpalBiz.Instance.Count;
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

        #region Get  Columns

        public static GridColumnModelList<ZarinpalPayment> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<ZarinpalPayment> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<ZarinpalPayment>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("0");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.StatusS).SetCaption("وضعیت").SetWidth("300");
                Columns.Add(x => x.RefId).SetCaption("کد رهگیری").SetCellType(GridCellType.INT).SetWidth("200");
                //Columns.Add(x => x.Autohority).SetCaption("نام خانوادگی").SetWidth("300");
                Columns.Add(x => x.Amount).SetCaption("هزینه(ریال) ").SetCellType(GridCellType.INT).SetWidth("100");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ تایید").SetWidth("100");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ").SetWidth("100");
                //_columns.Add(x => x.username).SetCaption("نوع خودرو").SetWidth("300");
                //_columns.Add(x => x.courseName).SetCaption("پلاک").SetWidth("300");
                //_columns.Add(x => x.onlineclassName).SetCaption("پلاک").SetWidth("300");
                //بابت چه کلاسی پول داده جوین می خواد
            }
            return Columns;
        }
        #endregion Get  Columns
    }
}