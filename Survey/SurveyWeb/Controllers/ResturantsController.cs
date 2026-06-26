using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using SurveyWeb.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class ResturantsController : BaseAdminController
    {
        // GET: Resturants
        public async Task<ActionResult> Index()
        {
            var cartable = await Biz.CartableUserAccessBiz.Instance.GetAllAccess(Current_UserId, CartableType.Resturant);
            return View(cartable);
        }
        public ActionResult LoadList(GridSettings grid, int cartableId)
        {
            var list = Biz.ResturantBiz.Instance.GetAllPagedListByCartable(grid, cartableId);
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
        public static GridColumnModelList<Resturant> ResturantColumns { get; private set; } = GetResturantColumns();
        public static GridColumnModelList<Resturant> GetResturantColumns()
        {
            if (ResturantColumns == null)
            {
                ResturantColumns = new GridColumnModelList<Resturant>();
                ResturantColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                ResturantColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("200");
                ResturantColumns.Add(x => x.Cartable.Name).SetCaption("وضعیت کارتابل").SetWidth("100");
                ResturantColumns.Add(x => x.Code).SetCaption("کد مرکز").SetWidth("50");
                ResturantColumns.Add(x => x.Name).SetCaption("نام مرکزپذیرایی").SetWidth("100");
                ResturantColumns.Add(x => x.Manager).SetCaption("مدیر").SetWidth("100");
                ResturantColumns.Add(x => x.SalonManager).SetCaption("مدیر سالن").SetWidth("100");
                ResturantColumns.Add(x => x.Tel).SetCaption("شماره تماس").SetWidth("100");
                ResturantColumns.Add(x => x.Beneficiary).SetCaption("بهره بردار").SetWidth("100");
                ResturantColumns.Add(x => x.Owner).SetCaption("مالک").SetWidth("100");
                ResturantColumns.Add(x => x.LastDateExtendedLicenseShamsi).SetCaption("تاریخ انقضاء مجوز").SetWidth("100");
                ResturantColumns.Add(x => x.AddorEditnote).SetHidden(true).SetWidth("0");
                ResturantColumns.Add(x => x.AddorEditnoteDesc).SetCaption("آخرین تغییر").SetWidth("100");
            }
            return ResturantColumns;
        }
        // GET: Resturants/Details/5
        public async Task<ActionResult> Details(int id, int cartableId)
        {
            if (id == 0 || cartableId == 0)
            {
                return null;
            }
            ViewBag.CartableId = cartableId;
            ViewBag.Id = id;
            List<SurveyWeb.Models.CheckListTypeCartable> model = await CheckListTypeCartableBiz.Instance.GetAll(x => x.CartableId == cartableId && x.ResturantId == id);
            var list = Biz.ResturantBiz.Instance.CheckListType();
            list.AddRange(EnumExtention.GetEnumsProperty<CartableCheckListType>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList());
            foreach (var item in list)
            {
                var res = model.FirstOrDefault(x => x.CheckListId.ToString() == item.Value);
                if (res == null)
                {
                    model.Add(new CheckListTypeCartable() { CartableId = cartableId, DropDownTitle = item.Text, CheckListId = int.Parse(item.Value), ResturantId = id, UserId = Current_UserId });
                }
                else
                {
                    res.DropDownTitle = item.Text;
                }
            }

            return PartialView(model);
        }
        public async Task<ActionResult> ShowDetail(int id, int CheckListId)
        {
            if (Enum.IsDefined(typeof(CartableCheckListType), CheckListId))
            {
                CartableCheckListType result = (CartableCheckListType)CheckListId;
                if (result == CartableCheckListType.Personels)
                {
                    List<ResturantPersonel> personels = await ResturantBiz.Instance.FindPersonelsByResturantId(id);
                    return View("ShowPersonels", personels);
                }
                else
                {
                    ViewBag.CartableCheckListType = result;
                    Resturant resturant = await ResturantBiz.Instance.Get(id);
                    return View("ShowDetail", resturant);
                }
            }
            else
            {
                List<ResturantCheckList> attach = await ResturantBiz.Instance.FindResturantCheckListByResturantIdandType(id, CheckListId);
                return View("ShowAttachment", attach);
            }
        }
        public async Task<ActionResult> AcceptedDetail(CheckListTypeCartable model)
        {
            model.UserId = Current_UserId;
            if (string.IsNullOrEmpty(model.CartableCheckListType))
            {
                if (model.Accepted == true)
                {
                    model.CartableCheckListType = "تایید شد";
                }
                if (model.Accepted == false)
                {
                    model.CartableCheckListType = "تایید نشد";
                }
            }
            await Biz.CheckListTypeCartableBiz.Instance.Save(model);
            return Json(new ApiJsonResult() { success=true}, JsonRequestBehavior.AllowGet);
        }
    }
}
