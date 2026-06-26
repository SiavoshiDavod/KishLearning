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
    public class ContactUsController : BaseAdminController
    {
        // GET: ContactUs
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.ContactUs);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid, int cartableId)
        {
            var list = Biz.ContactUsBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
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
        public static GridColumnModelList<ContactUs> ContactUsColumns { get; private set; } = GetContactUsColumns();
        public static GridColumnModelList<ContactUs> GetContactUsColumns()
        {
            if (ContactUsColumns == null)
            {
                ContactUsColumns = new GridColumnModelList<ContactUs>();
                ContactUsColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ContactUsColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                ContactUsColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                ContactUsColumns.Add(x => x.Name).SetCaption("نام").SetWidth("100");
                ContactUsColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("100");
                ContactUsColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
                ContactUsColumns.Add(x => x.Title).SetCaption("عنوان ایده").SetWidth("100");
                ContactUsColumns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("100");
            }
            return ContactUsColumns;
        }
        // GET: ContactUs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs cartable = await ContactUsBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return PartialView(cartable);
        }

        // GET: ContactUs/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            ContactUs cartable = await ContactUsBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return View();
            }
            return View(cartable);
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContactUs user)
        {
            await ContactUsBiz.Instance.Save(user);
            return RedirectToAction("Index", "ContactUs");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs user = await ContactUsBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ContactUsBiz.Instance.Remove(id);
            return RedirectToAction("Index", "ContactUs");
        }
    }
}
