using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class SendSmsController : BaseAdminController
    {
        public ActionResult Index()
        {
            var credit = Sms.Send.Instance.GetCredit();
            return View(credit);
        }
        public async Task<ActionResult> SendSms(string mobile, string content)
        {
            var res = await Biz.EmaiSmslBiz.Instance.SendSms(mobile, content);
            return Json(res.Item1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendEmail(EmailSms model)
        {
            var res = Biz.EmaiSmslBiz.Instance.AlertForClass(model.To, model.Subject, model.Body, SiteSetting.GetSetting.Instance.Get());
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SendSmsBatch(int GroupId, string content)
        {
            var to = Biz.GroupDetailBiz.Instance.GetAllPhoneByGroupId(GroupId);
            var count = 0;
            foreach (var mobile in to)
            {
                var res = await Biz.EmaiSmslBiz.Instance.SendSms(mobile, content);
                if (res.Item1)
                {
                    count++;
                }
            }
            //Parallel.ForEach(to, async mobile =>
            //{
            //    var res = await Biz.EmaiSmslBiz.Instance.SendSms(mobile, content);
            //    if (res.Item1)
            //    {
            //        count++;
            //    }
            //});
            return Json(count > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendEmailBatch(EmailSms model)
        {
            var to = Biz.GroupDetailBiz.Instance.GetAllPhoneByGroupId(model.GroupId);
            var count = 0;
            foreach (var i in to)
            {
                if (Biz.EmaiSmslBiz.Instance.AlertForClass(i, model.Subject, model.Body, SiteSetting.GetSetting.Instance.Get()))
                {
                    count++;
                }
            }
            //Parallel.ForEach(to, i => {
            //    if (Biz.EmaiSmslBiz.Instance.AlertForClass(i, model.Subject, model.Body))
            //    {
            //        count++;
            //    }
            //});
            return Json(count>0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.EmaiSmslBiz.Instance.GetAllPagedList(grid);
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


        public static GridColumnModelList<EmailSms> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<EmailSms> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<EmailSms>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("0");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.EmailSmsTypeName).SetCaption("نوع").SetWidth("100");
                Columns.Add(x => x.From).SetCaption("فرستنده").SetWidth("100");
                Columns.Add(x => x.To).SetCaption("گیرنده").SetWidth("100");
                Columns.Add(x => x.IsSend).SetCaption("ارسال شده؟").SetWidth("100");
                Columns.Add(x => x.SendResult).SetCaption("وضعیت ارسال").SetWidth("100");
                Columns.Add(x => x.Subject).SetCaption("تیتر ایمیل").SetWidth("100");
                Columns.Add(x => x.Body).SetCaption("متن").SetWidth("300");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ شروع برگزاری کلاس").SetWidth("60");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ پایان برگزاری کلاس").SetWidth("60");
            }
            return Columns;
        }
    }
}