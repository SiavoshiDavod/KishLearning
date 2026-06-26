using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.Models.TicketNotice;

namespace SurveyWeb.Controllers

{
    public class TicketController : BaseController
    {
        // GET: Tickets
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadList(GridSettings grid, bool? isRead)
        {
            var list = Biz.TicketBiz.Instance.GetAllCurrentUserTicket(grid, Current_UserId, isRead);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                TicketData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadLastestAnswerd()
        {
            var list = Biz.TicketBiz.Instance.GetAllCurrentUserTicket(Current_UserId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Ticket> TicketColumns { get; private set; } = GetTicketColumns();
        public static GridColumnModelList<Ticket> GetTicketColumns()
        {
            if (TicketColumns == null)
            {
                TicketColumns = new GridColumnModelList<Ticket>();
                TicketColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                TicketColumns.Add(x => x.IsRead).SetHidden(true).SetWidth("50");
                TicketColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                TicketColumns.Add(x => x.Title).SetCaption("عنوان").SetWidth("100");
                TicketColumns.Add(x => x.Content).SetCaption("متن تیکت").SetWidth("200");
                TicketColumns.Add(x => x.CreatedDateShamsi).SetCaption("تاریخ ثبت").SetWidth("100");
                TicketColumns.Add(x => x.Answer).SetCaption("متن پاسخ").SetWidth("300");
                TicketColumns.Add(x => x.ReceiverUser.UserName).SetCaption("پاسخ دهنده").SetWidth("200");
                TicketColumns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ پاسخ").SetWidth("100");
            }
            return TicketColumns;
        }
        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket cartable = await TicketBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Tickets/Create
        public ActionResult Create(int? id)
        {
            if (id != null && id>0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Ticket user)
        {
            if (user.Id > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user.CreatedDate = DateTime.Now;
            user.SenderUserId = Current_UserId;
            await TicketBiz.Instance.Save(user);
            return RedirectToAction("Index", "Ticket");
        }

        // POST: Tickets/Delete/5
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> SetIsRead(int id)
        {
            await TicketBiz.Instance.SetIsRead(id);
            return Json(new Models.wrapper.ApiJsonResult() { success = true }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "Ticket");
        }
    }
}