using System;
using System.Web.Mvc;
using MVC.Controls.Grid;
using SurveyWeb.Controllers;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;

namespace SenakLearn.Controllers
{
    public class UserCommentController : BaseAdminController
    {
   
        public ActionResult LoadList(GridSettings grid)
        {
            {
                var list = UserCommentBiz.Instance.GetAllPagedList(grid);
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

        #region Get  Columns

        public static GridColumnModelList<UserComment> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<UserComment> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<UserComment>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("30");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.Status).SetCaption("وضعیت").SetWidth("30");
                Columns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                Columns.Add(x => x.Mobile).SetCaption("تلفن").SetWidth("100");
                Columns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("150");
                Columns.Add(x => x.Title).SetCaption("عنوان").SetWidth("150");
                Columns.Add(x => x.Description).SetCaption("پیام").SetWidth("300");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ").SetWidth("100");
                Columns.Add(x => x.PageTypeName).SetCaption("نوع").SetWidth("100");
                Columns.Add(x => x.PageTypeLinkId).SetCaption("لینک").SetWidth("100");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ تایید").SetWidth("100");
            }
            return Columns;
        }
        #endregion Get  Columns
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Accept(int id)
        {
            UserCommentBiz.Instance.Accept(id);
            return RedirectToAction("Index");

        }
        
    }
}