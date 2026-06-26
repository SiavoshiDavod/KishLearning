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
    public class RegulationsController : BaseAdminController
    {


        // GET: Regulations
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.RegulationBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                RegulationData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Regulation> RegulationColumns { get; private set; } = GetRegulationColumns();
        public static GridColumnModelList<Regulation> GetRegulationColumns()
        {
            if (RegulationColumns == null)
            {
                RegulationColumns = new GridColumnModelList<Regulation>();
                RegulationColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                RegulationColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                RegulationColumns.Add(x => x.Question).SetCaption("سوال").SetWidth("300");
                RegulationColumns.Add(x => x.Answer).SetCaption("پاسخ").SetWidth("300");
                RegulationColumns.Add(x => x.File).SetCaption("فایل").SetWidth("300");
            }
            return RegulationColumns;
        }
        // GET: Regulations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regulation cartable = await RegulationBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Regulations/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Regulation cartable = await RegulationBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Regulations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Regulation user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.File = SaveFile(File, pathFile.Regulation);
            }
            else
            {
                user.File = EditFile(File, pathFile.Regulation, user.File);
            }
            await RegulationBiz.Instance.Save(user);
            return RedirectToAction("Index", "Regulations");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regulation user = await RegulationBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Regulations/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await RegulationBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Regulations");
        }
    }
}

