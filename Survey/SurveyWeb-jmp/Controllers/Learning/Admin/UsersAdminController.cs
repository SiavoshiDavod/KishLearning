using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using SenakLearn.Models;
using SenakLearn.JqGrid;
using MVC.Controls.Grid;
using System.Threading.Tasks;

namespace SenakLearn.Controllers.Admin
{
    public class UsersAdminController : SenakLearn.Controllers.BaseAdminController
    {
        private SWEntities db = new SWEntities();

        // GET: Admin/UsersAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list =Biz.UserBiz.Instance.GetAllPagedList(grid);
            Parallel.ForEach(list, x => x.BREEZESESSION=x.RoleName);
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

        public static GridColumnModelList<learn_user> UsersAdminColumns { get; private set; } = GetUsersAdminColumns();
        public static GridColumnModelList<learn_user> GetUsersAdminColumns()
        {
            if (UsersAdminColumns == null)
            {
                UsersAdminColumns = new GridColumnModelList<learn_user>();
                UsersAdminColumns.Add(x => x.id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                UsersAdminColumns.Add(x => x.PassAdobe).SetCaption("عملیات").SetWidth("50").SetSortable(false).SetSearchable(false);
                UsersAdminColumns.Add(x => x.BREEZESESSION).SetCaption("نقش").SetWidth("50").SetSortable(false).SetSearchable(false);
                UsersAdminColumns.Add(x => x.Name).SetCaption("نام").SetWidth("150");
                UsersAdminColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("150");
                UsersAdminColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("300");
                UsersAdminColumns.Add(x => x.NationaCode).SetCaption("کدملی").SetWidth("50");
                UsersAdminColumns.Add(x => x.Mobile).SetCaption("موبایل").SetWidth("50");
                UsersAdminColumns.Add(x => x.user_name).SetCaption("نام کاربری").SetWidth("100");
                UsersAdminColumns.Add(x => x.date_register_Shamsi).SetCaption("تاریخ ثبت").SetWidth("100");
                UsersAdminColumns.Add(x => x.status).SetCaption("وضعیت").SetWidth("50");
            }
            return UsersAdminColumns;
        }
        // GET: Admin/UsersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_user learn_user = Biz.UserBiz.Instance.Find(id.Value);// db.learn_user.Find(id);
            if (learn_user == null)
            {
                return HttpNotFound();
            }
            return View(learn_user);
        }

        // GET: Admin/UsersAdmin/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Admin/UsersAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // 
        public ActionResult Create([Bind(Include = "id_person,user_name,password,status,RoleId")] learn_user learn_user)
        {
            if (ModelState.IsValid)
            {
                var userName = db.learn_user.SingleOrDefault(i => i.user_name == learn_user.user_name);
                if (userName != null)
                {
                    ModelState.AddModelError("user_name", "نام کاربری تکراری است");
                    return View();
                }
                learn_user.date_register = DateTime.Now;
                learn_user.password = FormsAuthentication.HashPasswordForStoringInConfigFile(learn_user.password, "MD5");
                db.learn_user.Add(learn_user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(learn_user);
        }

        // GET: Admin/UsersAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_user learn_user = Biz.UserBiz.Instance.Find(id.Value);
            if (learn_user == null)
            {
                return HttpNotFound();
            }
            

            return View(learn_user);
        }
        public ActionResult ResetPass(int id,string pass)
        {
            try
            {
                Biz.UserBiz.Instance.ResetPass(id, pass);
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
        }

        // POST: Admin/UsersAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // 
        public ActionResult Edit( learn_user learn_user)
        {
            //if (ModelState.IsValid)
            //{
            var userNameExist = db.learn_user.SingleOrDefault(i => i.id == learn_user.id);
            if (userNameExist == null)
            {
                
                ModelState.AddModelError("user_name", " کاربر معتبر نیست");
                return View(learn_user);
            }
            if (db.learn_user.Any(x=>x.Email== learn_user.Email&& userNameExist.id != x.id))
            {
                
                ModelState.AddModelError("Email", "ایمیل تکراری است");
                return View(learn_user);
            }
            if (db.learn_user.Any(x => x.user_name == learn_user.user_name && userNameExist.id != x.id))
            {
                
                ModelState.AddModelError("user_name", "نام کاربری تکراری است");
                return View(learn_user);
            }
            //learn_user.password = FormsAuthentication.HashPasswordForStoringInConfigFile(learn_user.password, "MD5");
            //db.Entry(learn_user).State = EntityState.Modified;
            userNameExist.user_name = learn_user.user_name;
            userNameExist.Name = learn_user.Name;
            userNameExist.Family = learn_user.Family;
            userNameExist.Email = learn_user.Email;
            userNameExist.status = learn_user.status;
            userNameExist.RoleId = learn_user.RoleId;
            //userNameExist.password = FormsAuthentication.HashPasswordForStoringInConfigFile(learn_user.password, "MD5");
            userNameExist.Validate();
            db.SaveChanges();
            return RedirectToAction("Index");
            //}

        }

        // GET: Admin/UsersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learn_user learn_user = Biz.UserBiz.Instance.Find(id.Value);
            if (learn_user == null)
            {
                return HttpNotFound();
            }
            return View(learn_user);
        }

        // POST: Admin/UsersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
       // 
        public ActionResult DeleteConfirmed(int id)
        {
            learn_user learn_user = db.learn_user.Find(id);
            db.learn_user.Remove(learn_user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
