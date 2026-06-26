
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using System;
using MVC.Controls.Grid;

namespace SenakLearn.Controllers
{
    public class GroupSurveysController : BaseAdminController
    {
        // GET: GroupSurveys
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.GroupSurveyBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                GroupSurveyData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<GroupSurvey> GroupSurveyColumns { get; private set; } = GetGroupSurveyColumns();
        public static GridColumnModelList<GroupSurvey> GetGroupSurveyColumns()
        {
            if (GroupSurveyColumns == null)
            {
                GroupSurveyColumns = new GridColumnModelList<GroupSurvey>();
                GroupSurveyColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                GroupSurveyColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                GroupSurveyColumns.Add(x => x.Name).SetCaption("نام").SetWidth("300");
            }
            return GroupSurveyColumns;
        }
        // GET: GroupSurveys/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupSurvey user = await GroupSurveyBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: GroupSurveys/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            GroupSurvey user = await GroupSurveyBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return View();
            }
            return View(user);
        }

        // POST: GroupSurveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroupSurvey user)
        {
            await GroupSurveyBiz.Instance.Save(user);
            return RedirectToAction("Index", "GroupSurveys");
        }



        // GET: GroupSurveys/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupSurvey user = await GroupSurveyBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: GroupSurveys/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await GroupSurveyBiz.Instance.Remove(id);
            return RedirectToAction("Index", "GroupSurveys");
        }
    }
}
