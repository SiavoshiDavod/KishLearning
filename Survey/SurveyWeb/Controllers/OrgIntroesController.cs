using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;

namespace SurveyWeb.Controllers
{
    public class OrgIntroesController : BaseAdminController
    {


        // GET: OrgIntro
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OrgIntroBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                OrgIntroesData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<OrgIntro> OrgIntroesColumns { get; private set; } = GetOrgIntroesColumns();
        public static GridColumnModelList<OrgIntro> GetOrgIntroesColumns()
        {
            if (OrgIntroesColumns == null)
            {
                OrgIntroesColumns = new GridColumnModelList<OrgIntro>();
                OrgIntroesColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                OrgIntroesColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                OrgIntroesColumns.Add(x => x.Name).SetCaption("عنوان").SetWidth("300");
            }
            return OrgIntroesColumns;
        }
        // GET: OrgIntro/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgIntro cartable = await OrgIntroBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: OrgIntro/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            OrgIntro cartable = await OrgIntroBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: OrgIntro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrgIntro user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.ImageUrl = SaveFile(File, pathFile.OrgIntro);
            }
            else
            {
                user.ImageUrl = EditFile(File, pathFile.OrgIntro, user.ImageUrl);
            }
            await OrgIntroBiz.Instance.Save(user);
            return RedirectToAction("Index", "OrgIntroes");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgIntro user = await OrgIntroBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: OrgIntro/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await OrgIntroBiz.Instance.Remove(id);
            return RedirectToAction("Index", "OrgIntroes");
        }
    }
}

