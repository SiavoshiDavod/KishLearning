using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class ObjCountController : BaseAdminController
    {
        public ActionResult Index()
        {
            return View();
        }
        public static GridColumnModelList<ObjCount> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<ObjCount> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<ObjCount>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetCellType(GridCellType.INT);
                Columns.Add(x => x.Count).SetCaption("تعداد دانلود").SetWidth("100").SetCellType(GridCellType.INT);
                Columns.Add(x => x.ObjName).SetCaption("عنوان").SetWidth("100").SetCellType(GridCellType.INT);
                Columns.Add(x => x.ObjType).SetCaption("نوع").SetWidth("100");
                Columns.Add(x => x.ObjId).SetCaption("شناسه").SetWidth("100");
                Columns.Add(x => x.ObjTitle).SetCaption("عنوان اصلی").SetWidth("100");
                Columns.Add(x => x.ObjDescript).SetCaption("توضیحات").SetWidth("200");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ ایجاد").SetWidth("100");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("ویرایش ").SetWidth("100");

            }
            return Columns;
        }

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.ObjCountBiz.Instance.GetAllPagedListWrapper(grid);
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
    }
}