using MVC.Controls.Grid;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class CompanyLogoAndLinksController : BaseAdminController
    {


        // GET: CompanyLogoAndLink
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.CompanyLogoAndLinkBiz.Instance.GetAllPagedList(grid);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                CompanyLogoAndLinksData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<CompanyLogoAndLink> CompanyLogoAndLinksColumns { get; private set; } = GetCompanyLogoAndLinksColumns();
        public static GridColumnModelList<CompanyLogoAndLink> GetCompanyLogoAndLinksColumns()
        {
            if (CompanyLogoAndLinksColumns == null)
            {
                CompanyLogoAndLinksColumns = new GridColumnModelList<CompanyLogoAndLink>();
                CompanyLogoAndLinksColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                CompanyLogoAndLinksColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                CompanyLogoAndLinksColumns.Add(x => x.Name).SetCaption("نام").SetWidth("300");
                CompanyLogoAndLinksColumns.Add(x => x.Link).SetCaption("لینک").SetWidth("300");
            }
            return CompanyLogoAndLinksColumns;
        }
        // GET: CompanyLogoAndLink/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyLogoAndLink cartable =  CompanyLogoAndLinkBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: CompanyLogoAndLink/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            CompanyLogoAndLink cartable =  CompanyLogoAndLinkBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: CompanyLogoAndLink/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public ActionResult Create(CompanyLogoAndLink user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.ImageUrl = SaveFile(File, pathFile.Logo);
            }
            else
            {
                user.ImageUrl = EditFile(File, pathFile.Logo, user.ImageUrl);
            }
             CompanyLogoAndLinkBiz.Instance.Save(user);
            return RedirectToAction("Index", "CompanyLogoAndLinks");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyLogoAndLink user =  CompanyLogoAndLinkBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: CompanyLogoAndLink/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             CompanyLogoAndLinkBiz.Instance.Remove(id);
            return RedirectToAction("Index", "CompanyLogoAndLinks");
        }
    }

}