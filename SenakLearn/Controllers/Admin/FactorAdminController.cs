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
    public class FactorAdminController : BaseAdminController
    {
        // GET: FactorAdmin
        public ActionResult Index()
        {
            return View();
        }
        public static GridColumnModelList<FactorModel> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<FactorModel> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<FactorModel>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.ServiceName).SetCaption("عنوان").SetWidth("300");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ").SetWidth("100");
                Columns.Add(x => x.Mobile).SetCaption("همراه").SetWidth("100");
                Columns.Add(x => x.Amount).SetCaption("مبلغ").SetWidth("150");
                Columns.Add(x => x.StatusName).SetCaption("وضعیت").SetWidth("150");
                Columns.Add(x => x.Descript).SetCaption("شرح").SetWidth("300");
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var isAdmin = Current_learn_user.RoleId == Roles.Admin || Current_learn_user.RoleId == Roles.SuperAdmin;
            var list = Biz.FactorBiz.Instance.GetAllPagedList(grid, Current_learn_userId, isAdmin);
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
        public ActionResult Delete(long id)
        {
            var isAdmin = Current_learn_user.RoleId == Roles.Admin || Current_learn_user.RoleId == Roles.SuperAdmin;
            var res = Biz.FactorBiz.Instance.RemovedFactor(id);
            return Json(new { status = res }, JsonRequestBehavior.AllowGet);
        }
    }

}