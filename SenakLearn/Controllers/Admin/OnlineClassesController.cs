using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class OnlineClassesController : BaseAdminController
    {
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OnlineClassBiz.Instance.GetAllPagedList(grid);
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

        #region Get  Columns

        public static GridColumnModelList<OnlineClass> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<OnlineClass> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<OnlineClass>();
                Columns.Add(x => x.act).SetCaption("").SetWidth("0");
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.GoToAdobe).SetCaption("ادوبی").SetWidth("100");
                Columns.Add(x => x.name).SetCaption("عنوان کلاس").SetWidth("300");
                Columns.Add(x => x.Amount).SetCaption("هزینه(ریال) ").SetWidth("50");
                Columns.Add(x => x.Duration).SetCaption("مدت دوره").SetWidth("50");
                Columns.Add(x => x.SessionCount).SetCaption("تعداد جلسات").SetWidth("50");
                Columns.Add(x => x.Time).SetCaption("ساعت برگزاری کلاس").SetWidth("50");
                Columns.Add(x => x.Days).SetCaption("روزهای برگزاری کلاس").SetWidth("100");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ شروع برگزاری کلاس").SetWidth("60");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ پایان برگزاری کلاس").SetWidth("60");
                Columns.Add(x => x.Capacity).SetCaption("ظرفیت").SetWidth("50");
                Columns.Add(x => x.ClassTypeString).SetCaption("وضعیت").SetWidth("50");
                Columns.Add(x => x.IsFavoriteS).SetCaption("نمایش در صفحه اصلی سایت").SetWidth("50");
                Columns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("300");
            }
            return Columns;
        }
        //public static GridColumnModelList<OnlineClassPayment> ColumnPayments { get; private set; } = GetColumnPayments();
        //private static GridColumnModelList<OnlineClassPayment> GetColumnPayments()
        //{
        //    if (ColumnPayments == null)
        //    {
        //        ColumnPayments = new GridColumnModelList<OnlineClassPayment>();
        //        ColumnPayments.Add(x => x.first_name).SetCaption("first-name").SetWidth("50");
        //        ColumnPayments.Add(x => x.last_name).SetCaption("last-name").SetWidth("50");
        //        ColumnPayments.Add(x => x.login).SetCaption("login").SetWidth("50");
        //        ColumnPayments.Add(x => x.email).SetCaption("email").SetWidth("50");
        //        ColumnPayments.Add(x => x.password).SetCaption("password").SetWidth("50");
        //    }
        //    return ColumnPayments;
        //}
        public ActionResult ExcelForAdobe(int id)
        {
            var list = Biz.OnlineClassBiz.Instance.GetAllPaymentsForAdobe(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].BREEZESESSION = (i + 1).ToString();
            }
            return PrintListToExcel<learn_user>(list, learnUserColumns.Items, "دانشجویان کلاس انلاین");
        }
        public ActionResult ShowUser(int id)
        {
            ViewBag.Url = "/OnlineClasses/ShowUserLoadList?id=" + id;
            return View("GridUser");
        }
        public ActionResult ShowUserLoadList(GridSettings grid, int id)
        {
            var list = Biz.OnlineClassBiz.Instance.GetAllPaymentsForAdobe(id).AsQueryable().FilterAndSortJqGrid(grid).ToPagedList(grid);
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
        #endregion Get  Columns
        // GET: OnlineClasss
        public ActionResult Index()
        {
            return View(Biz.OnlineClassBiz.Instance.GetAll(x => x.Id != 0));
        }

        // GET: OnlineClasss/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClass OnlineClass = Biz.OnlineClassBiz.Instance.Get(id ?? 0);// db.OnlineClasss.Find(id);
            if (OnlineClass == null)
            {
                return HttpNotFound();
            }
            return View(OnlineClass);
        }

        // GET: OnlineClasss/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnlineClasss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Create(OnlineClass onlineClass, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                onlineClass.InterviewPathImage = SaveFile(ImageFile, pathFile.Interview);
                if (Biz.OnlineClassBiz.Instance.Save(onlineClass, Current_learn_user, true, out string m))
                {
                    TempData["SuccessMessage"] = m;
                }
                else
                {
                    TempData["ErrorMessage"] = m;
                }
                return RedirectToAction("Index");
            }
            await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Online, onlineClass.id_learn_cours_group);
            await Biz.TeacherBiz.Instance.UpdateCourseCount(onlineClass.id_learn_teacher);
            return View(onlineClass);
        }

        // GET: OnlineClasss/Edit/5
        public ActionResult Edit(int? id,bool clone=false)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClass OnlineClass = Biz.OnlineClassBiz.Instance.Get(id ?? 0);// db.OnlineClasss.Find(id);
            if (OnlineClass == null)
            {
                return HttpNotFound();
            }
            if (clone)
            {
                return View("Create", OnlineClass);
            }
            return View(OnlineClass);
        }

        // POST: OnlineClasss/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(OnlineClass onlineClass, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                onlineClass.InterviewPathImage = EditFile(ImageFile, pathFile.Interview, onlineClass.InterviewPathImage);
                if (Biz.OnlineClassBiz.Instance.Save(onlineClass, Current_learn_user, false, out string m))
                {
                    TempData["SuccessMessage"] = m;
                }
                else
                {
                    TempData["ErrorMessage"] = m;
                }
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(onlineClass);
        }

        // GET: OnlineClasss/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OnlineClass OnlineClass = Biz.OnlineClassBiz.Instance.Get(id ?? 0);// db.OnlineClasss.Find(id);
            if (OnlineClass == null)
            {
                return HttpNotFound();
            }
            return View(OnlineClass);
        }

        // POST: OnlineClasss/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //OnlineClass OnlineClass = db.OnlineClasss.Find(id);
            //db.OnlineClasss.Remove(OnlineClass);
            //db.SaveChanges();
            var id_learn_cours_group = Biz.OnlineClassBiz.Instance.Remove(id);
            await Biz.CourseBiz.Instance.UpdateGroupCount(CoursGroupCountType.Online, id_learn_cours_group, false);
            //await Biz.TeacherBiz.Instance.UpdateCourseCount(onlineClass.id_learn_teacher, false);
            return RedirectToAction("Index");
        }

    }
}
