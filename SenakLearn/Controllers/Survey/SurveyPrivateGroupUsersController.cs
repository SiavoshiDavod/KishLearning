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
    public class SurveyPrivateGroupUsersController : BaseAdminController
    {


        // GET: UserSurveyPrivateGroupLogs
        public async Task<ActionResult> Index(int id)
        {
            ViewBag.Name = (await Biz.SurveyPrivateGroupBiz.Instance.Get(id))?.Name;
            return PartialView(id);
        }
        public ActionResult LoadList(GridSettings grid, int id)
        {
            var list = Biz.SurveyPrivateGroupBiz.Instance.GetAllPagedListUserBySurveyPrivateGroupId(grid, id);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                SurveyEntityData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        // GET: UserSurveyPrivateGroupLogs/Details/5
        public static GridColumnModelList<SurveyPrivateGroupUserVm> UserSurveyPrivateGroupColumns { get; private set; } = GetUserSurveyPrivateGroupColumns();
        public static GridColumnModelList<SurveyPrivateGroupUserVm> GetUserSurveyPrivateGroupColumns()
        {
            if (UserSurveyPrivateGroupColumns == null)
            {
                UserSurveyPrivateGroupColumns = new GridColumnModelList<SurveyPrivateGroupUserVm>();
                UserSurveyPrivateGroupColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                UserSurveyPrivateGroupColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                UserSurveyPrivateGroupColumns.Add(x => x.UserName).SetCaption("دسترسی").SetWidth("300");
            }
            return UserSurveyPrivateGroupColumns;
        }
        // GET: UserSurveyPrivateGroups/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserSurveyPrivateGroup cartable = await UserSurveyPrivateGroupBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartable);
        //}

        // GET: UserSurveyPrivateGroups/Create
        //public async Task<ActionResult> Create(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View();
        //    }
        //    UserSurveyPrivateGroup cartable = await UserSurveyPrivateGroupBiz.Instance.Get(id.Value);
        //    if (cartable == null)
        //    {
        //        return View();
        //    }
        //    return View(cartable);
        //}

        // POST: UserSurveyPrivateGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(SurveyPrivateGroupUser SurveyPrivateGroup)
        {
            await SurveyPrivateGroupBiz.Instance.SaveSurveyPrivateGroupUser(SurveyPrivateGroup);
            return RedirectToAction("Index", "SurveyPrivateGroupUsers", new { id = SurveyPrivateGroup.SurveyPrivateGroupId });
        }

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserSurveyPrivateGroup SurveyPrivateGroup = await UserSurveyPrivateGroupBiz.Instance.Get(id.Value);
        //    if (SurveyPrivateGroup == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(SurveyPrivateGroup);
        //}

        // POST: UserSurveyPrivateGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await SurveyPrivateGroupBiz.Instance.RemoveSurveyPrivateGroupUser(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
