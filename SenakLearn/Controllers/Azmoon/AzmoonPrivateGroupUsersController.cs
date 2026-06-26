using System.Threading.Tasks;
using System.Web.Mvc;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;
using SenakLearn.Models.Security;
using SenakLearn.Models.wrapper;

namespace SenakLearn.Controllers
{
    public class AzmoonPrivateGroupUsersController : BaseAdminController
    {


        // GET: UserAzmoonPrivateGroupLogs
        public async Task<ActionResult> Index(int id)
        {
            ViewBag.Name = (await Biz.AzmoonPrivateGroupBiz.Instance.Get(id))?.Name;
            return PartialView(id);
        }
        public ActionResult LoadList(GridSettings grid, int id)
        {
            var list = Biz.AzmoonPrivateGroupBiz.Instance.GetAllPagedListUserByAzmoonPrivateGroupId(grid, id);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                AzmoonEntityData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        // GET: UserAzmoonPrivateGroupLogs/Details/5
        public static GridColumnModelList<SurveyPrivateGroupUserVm> UserAzmoonPrivateGroupColumns { get; private set; } = GetUserAzmoonPrivateGroupColumns();
        public static GridColumnModelList<SurveyPrivateGroupUserVm> GetUserAzmoonPrivateGroupColumns()
        {
            if (UserAzmoonPrivateGroupColumns == null)
            {
                UserAzmoonPrivateGroupColumns = new GridColumnModelList<SurveyPrivateGroupUserVm>();
                UserAzmoonPrivateGroupColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                UserAzmoonPrivateGroupColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                UserAzmoonPrivateGroupColumns.Add(x => x.UserName).SetCaption("دسترسی").SetWidth("300");
            }
            return UserAzmoonPrivateGroupColumns;
        }
        // GET: UserAzmoonPrivateGroups/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserAzmoonPrivateGroup cartable = await UserAzmoonPrivateGroupBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartable);
        //}

        // GET: UserAzmoonPrivateGroups/Create
        //public async Task<ActionResult> Create(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View();
        //    }
        //    UserAzmoonPrivateGroup cartable = await UserAzmoonPrivateGroupBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return View();
        //    }
        //    return View(cartable);
        //}

        // POST: UserAzmoonPrivateGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(AzmoonPrivateGroupUser AzmoonPrivateGroup)
        {
            await AzmoonPrivateGroupBiz.Instance.SaveAzmoonPrivateGroupUser(AzmoonPrivateGroup);
            return RedirectToAction("Index", "AzmoonPrivateGroupUsers", new { id = AzmoonPrivateGroup.AzmoonPrivateGroupId });
        }

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserAzmoonPrivateGroup AzmoonPrivateGroup = await UserAzmoonPrivateGroupBiz.Instance.Get(id.Value);
        //    if (AzmoonPrivateGroup == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(AzmoonPrivateGroup);
        //}

        // POST: UserAzmoonPrivateGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AzmoonPrivateGroupBiz.Instance.RemoveAzmoonPrivateGroupUser(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
