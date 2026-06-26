using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using MVC.Controls.Grid;
using SurveyWeb.Biz;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Controllers
{
    public class AdvertisingsController : BaseAdminController
    {
        // GET: Advertisings
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid, bool Archive = true)
        {
            var list = Biz.AdvertisingBiz.Instance.GetAllPagedList(grid,Archive);
            //foreach (var item in list)
            //{
            //    item.Resturant.Advertising = null;
            //}
            PagedList<Advertising> obj = CloneUsingJsonConvertExtension.Clone(list);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = obj.ToArray(),
                AdvertisingData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        public static GridColumnModelList<Advertising> AdvertisingColumns { get; private set; } = GetAdvertisingColumns();
        public static GridColumnModelList<Advertising> GetAdvertisingColumns()
        {
            if (AdvertisingColumns == null)
            {
                AdvertisingColumns = new GridColumnModelList<Advertising>();
                AdvertisingColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                AdvertisingColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("100");
                AdvertisingColumns.Add(x => x.ResturantName).SetCaption("مرکزپذیرایی").SetWidth("300");
                AdvertisingColumns.Add(x => x.LinkReserve).SetCaption("رزرو").SetWidth("300");
            }
            return AdvertisingColumns;
        }

        public ActionResult Accept(int id)
        {
            try
            {
                Biz.AdvertisingBiz.Instance.Accept(id);
                return Json(new ApiJsonResult { success = true, Message = "ok", ErrorMessage = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ApiJsonResult { success = false, ErrorMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Advertisings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertising cartable = await AdvertisingBiz.Instance.Get(id.Value);
            if (cartable == null)
            {
                return HttpNotFound();
            }
            return View(cartable);
        }

        // GET: Advertisings/Create
        public async Task<ActionResult> Create(int? id, int? ResturantId)
        {
            if (id != null)
            {
                Advertising cartable = await AdvertisingBiz.Instance.Get(id.Value);
                if (cartable != null)
                {
                    return View(cartable);
                }
            }
            if (ResturantId == null)
            {
                return HttpNotFound();
            }

            Advertising resturant = await AdvertisingBiz.Instance.FindByResturantId(ResturantId.Value);
            if (resturant == null)
            {
                resturant = new Advertising() { ResturantId = ResturantId.Value,Archive=true };
            }
            return View(resturant);
        }

        // POST: Advertisings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Advertising user, System.Web.HttpPostedFileBase File)
        {
            if (user.Id == 0)
            {
                user.ImageUrl = SaveFile(File, pathFile.Advertising);
            }
            else
            {
                user.ImageUrl = EditFile(File, pathFile.Advertising, user.ImageUrl);
            }
            await AdvertisingBiz.Instance.Save(user);
            return RedirectToAction("Index", "Advertisings");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertising user = await AdvertisingBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Advertisings/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await AdvertisingBiz.Instance.Remove(id);
            return RedirectToAction("Index", "Advertisings");
        }


        public async Task<ActionResult> AdvertisingAttachement(int AdvertisingId)
        {
            Advertising res = await Biz.AdvertisingBiz.Instance.GetInclude(new Advertising() { Id = AdvertisingId }, "AdvertisingAttachements");
            if (res != null)
                return View(res);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public async Task<ActionResult> FindAdvertisingAttachements(int AdvertisingId, int? id)
        {
            if (id == null)
            {
                return PartialView("AdvertisingAttachementSave", new AdvertisingAttachement() { AdvertisingId = AdvertisingId });
            }
            AdvertisingAttachement res = await Biz.AdvertisingBiz.Instance.FindAdvertisingAttachement(id.Value);
            if (res == null || res.AdvertisingId != AdvertisingId)
            {
                return null;
            }
            return PartialView("AdvertisingAttachementSave", res);
        }

        [HttpPost]
        public async Task<ActionResult> SaveAdvertisingAttachement(AdvertisingAttachement model, System.Web.HttpPostedFileBase File)
        {
            if (File == null)
            {
                return null;
            }
            
            var type = File.ContentType.ToLower();
            if (type.Contains("image"))
            {
                model.IsVideo = false;
            }
            else if (type.Contains("video"))
            {
                model.IsVideo = true;
            }
            else
            {
                return Json(new ApiJsonResult() { success = false, ErrorMessage = "نوع فایل معتبر نیست" }, JsonRequestBehavior.AllowGet);
            }

            model.ImageUrl = SaveFile(File, pathFile.Advertising);
            var res = await Biz.AdvertisingBiz.Instance.SaveAdvertisingAttachement(model);
            //return RedirectToAction("CheckList", "CheckListTypeCartables");
            return Json(new ApiJsonResult() { success = true, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> RemoveAdvertisingAttachement(int id)
        {
            string oldFileName = await Biz.AdvertisingBiz.Instance.RemoveAdvertisingAttachement(id, true);
            if (!string.IsNullOrEmpty(oldFileName) && System.IO.File.Exists("/images/" + pathFile.Advertising + "/" + oldFileName))
                System.IO.File.Delete(Server.MapPath("/images/" + pathFile.Advertising + "/" + oldFileName));
            return Json(new ApiJsonResult() { success = true, Message = "حذف با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }
    }
}

