using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using System;
using MVC.Controls.Grid;

namespace SurveyWeb.Controllers
{
    public class UsersController : BaseAdminController
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.UserBiz.Instance.GetAllPagedList(grid);
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
        public static GridColumnModelList<User> UserColumns { get; private set; } = GetUserColumns();
        public static GridColumnModelList<User> GetUserColumns()
        {
            if (UserColumns == null)
            {
                UserColumns = new GridColumnModelList<User>();
                UserColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                UserColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                UserColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                UserColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                UserColumns.Add(x => x.Mobile).SetCaption("موبایل").SetWidth("100");
                UserColumns.Add(x => x.UserName).SetCaption("نام کاربری").SetWidth("100");
                UserColumns.Add(x => x.OldYear).SetCaption("سن").SetWidth("50");
                UserColumns.Add(x => x.RoleName).SetCaption("نقش").SetWidth("50");
                UserColumns.Add(x => x.ProvinceName).SetCaption("استان").SetWidth("50");
            }
            return UserColumns;
        }
        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await UserBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            User user = await UserBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.UserImageUrl = SaveFile(File, pathFile.User);
                await UserBiz.Instance.Save(user);
            }
            else
            {
                // user.UserImageUrl = EditFile(File, pathFile.User, user.UserImageUrl);
                await UserBiz.Instance.UpdateAdmin(user);
            }
            return RedirectToAction("Index", "Users");
        }



        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await UserBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await UserBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Users");
        }
    }
}
