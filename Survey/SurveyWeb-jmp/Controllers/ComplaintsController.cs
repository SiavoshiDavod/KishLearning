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
    public class ComplaintsController : BaseAdminController
    {
        // GET: Complaints
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.Complaint);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid, int cartableId)
        {
            var list = Biz.ComplaintBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                CartableId = cartableId
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Complaint> ComplaintColumns { get; private set; } = GetComplaintColumns();
        public static GridColumnModelList<Complaint> GetComplaintColumns()
        {
            if (ComplaintColumns == null)
            {
                ComplaintColumns = new GridColumnModelList<Complaint>();
                ComplaintColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ComplaintColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                ComplaintColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                ComplaintColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                ComplaintColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("100");
                ComplaintColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                ComplaintColumns.Add(x => x.Mobile).SetCaption("تلفن همراه").SetWidth("100");
                ComplaintColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
                ComplaintColumns.Add(x => x.Title).SetCaption("عنوان شکایت").SetWidth("100");
                ComplaintColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("100");
                ComplaintColumns.Add(x => x.TrackingCode).SetCaption("کدرهگیری").SetWidth("100");
            }
            return ComplaintColumns;
        }
        // GET: Complaints/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint cartable = await ComplaintBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return PartialView(cartable);
        }

        // GET: Complaints/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Complaint cartable = await ComplaintBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Complaint user)
        {
            await ComplaintBiz.Instance.Save(user);
            return RedirectToAction("Index", "Complaints");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint user = await ComplaintBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ComplaintBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Complaints");
        }
    }
}
