using System;
using System.Linq;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;
using System.Threading.Tasks;

namespace SenakLearn.Controllers.Admin
{
    public class JoinUsController : BaseAdminController
    {
        // GET: JoinUs
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Accept(int id)
        {
            var obj = Biz.JoinUsBiz.Instance.Accept(id);
            if (obj == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            string userpass = "همچنین نام کاربری شما در سامانه کدملی و رمز عبور، شماره موبایل شماست.";
            if (obj.UserId==null)
            {
                userpass = "لطفا ابتدا در سایت لاگین کرده و سپس لینک زیر را کلیک کنید";
            }
            //else if (obj.Username != obj.NationalCode)
            //{

            //}
            var setting = SiteSetting.GetSetting.Instance.Get();
            string body = $"<div dir='rtl' style='text-align: right; '> جناب آقای / خانم <b>{obj.Name} {obj.Family}</b><br>به {setting.NameFa} خوش آمدید. <br>شما باید ابتدا اطلاعات خود را از طریق لینک زیر در سایت تکمیل کرده و قرارداد را مطالعه کنید. سپس می توانید ویدیوهای آموزشی خود را آپلود کنید. ما پس از مشاهده و بررسی آنها با شما تماس خواهیم گرفت  <br>{userpass}<br>برای همکاری با آموزش مجازی روی لینک زیر کلیک کنید.<br> <a href='{setting.SiteUrl}users/TeacherLogin/{obj.Id}'>همکاری با ما</a></div>";
            Biz.EmaiSmslBiz.Instance.AlertForClass(obj.Email, "آموزش مجازی کیش", body, setting);
            await Biz.EmaiSmslBiz.Instance.SendSms(obj.Mobile, $"لطفا جهت ادامه همکاری با {setting.NameFa} .ایمیلتان را چک کنید. در صورت عدم دریافت ایمیل در inbox،پوشه اسپم اکانت خود را هم بررسی کنید. ");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.JoinUsBiz.Instance.GetAllPagedList(grid);
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

        public static GridColumnModelList<JoinUs> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<JoinUs> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<JoinUs>();
                Columns.Add(x => x.act).SetCaption("رزومه").SetWidth("80");
                Columns.Add(x => x.IsAccept).SetHidden(true);
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                Columns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                Columns.Add(x => x.NationalCode).SetCaption("کدملی").SetWidth("100");
                Columns.Add(x => x.Mobile).SetCaption("موبایل").SetWidth("100");
                Columns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("150");
                Columns.Add(x => x.GroupIds).SetCaption("گروه").SetWidth("100");
                // Columns.Add(x => x.ResumeFile).SetCaption("عکس").SetWidth("100");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ").SetWidth("100");
                Columns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("300");
            }
            return Columns;
        }
        #endregion Get  Columns
        // GET: OfflineVideos/Edit/5
        public ActionResult Edit(int? id)
        {

            JoinUs obj = Biz.JoinUsBiz.Instance.Get(id ?? 0);// db.OfflineVideos.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            if (obj.TeacherId!=null)
            {
                var teacher = Biz.TeacherBiz.Instance.FindById(obj.TeacherId.Value);
                ViewBag.Teacher = teacher;
            }
            return PartialView(obj);
        }


        [HttpPost]
        public ActionResult Edit(JoinUs JoinUs)
        {
            if (ModelState.IsValid)
            {
                Biz.JoinUsBiz.Instance.Save(JoinUs);
                return RedirectToAction("Index");
            }
            return View(JoinUs);
        }

    }
}