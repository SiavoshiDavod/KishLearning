using AdobeConnectService;
using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Student
{
    public class MyClassController : BaseProfileController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OnlineClassBiz.Instance.GetAllonlineClassByUserId(grid, Current_learn_userId);
            //var count = Biz.OnlineClassBiz.Instance.Count;
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


        public static GridColumnModelList<OnlineClass> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<OnlineClass> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<OnlineClass>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.GoToAdobe).SetCaption("ورود به کلاس").SetWidth("100");
                Columns.Add(x => x.name).SetCaption("عنوان کلاس").SetWidth("300");
                Columns.Add(x => x.Amount).SetCaption("هزینه(ریال) ").SetWidth("50");
                Columns.Add(x => x.Duration).SetCaption("مدت دوره").SetWidth("50");
                Columns.Add(x => x.SessionCount).SetCaption("تعداد جلسات").SetWidth("50");
                Columns.Add(x => x.Time).SetCaption("ساعت برگزاری کلاس").SetWidth("50");
                Columns.Add(x => x.Days).SetCaption("روزهای برگزاری کلاس").SetWidth("100");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ شروع برگزاری کلاس").SetWidth("70");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ پایان برگزاری کلاس").SetWidth("70");
                Columns.Add(x => x.Capacity).SetCaption("ظرفیت").SetWidth("50");
                Columns.Add(x => x.ClassTypeString).SetCaption("وضعیت").SetWidth("50");
                Columns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("300");
            }
            return Columns;
        }
        // GET: MyClass
        public ActionResult Adobe()
        {
            ClassUsingSdk adob = new ClassUsingSdk(Current_learn_user.Email, Current_learn_user.PassAdobe, Current_learn_user.BREEZESESSION);
            Session["BREEZESESSION"] = adob.BREEZESESSION;
            if (adob.IsLogin && adob.Api != null)
                return View(adob.GetMyMeetings());
            return View();
        }

    }
}