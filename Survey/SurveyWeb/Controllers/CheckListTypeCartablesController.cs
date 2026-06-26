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
    public class CheckListTypeCartablesController : BaseAdminController
    {


        // GET: Resturants
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid,bool? LastDateExtendedLicenseFilter=null)
        {
            var list = Biz.ResturantBiz.Instance.GetAllPagedListByCartable(grid,0, LastDateExtendedLicenseFilter);
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                ResturantData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }


        // GET: Resturants/Create
        public async Task<ActionResult> Create(int? id,int? userId)
        {
            if (id != null)
            {
                Resturant cartable = await ResturantBiz.Instance.Get(id.Value);
                if (cartable!=null)
                {
                    return View(cartable);
                }
            }
            if (userId == null)
            {
                return HttpNotFound();
            }

            Resturant resturant = await ResturantBiz.Instance.FindByUserId(userId.Value);
            if (resturant == null)
            {
                resturant = new Resturant() { UserId = userId.Value };
            }
            return View(resturant);
        }

        // POST: Resturants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Resturant model, System.Web.HttpPostedFileBase ManagerFile, System.Web.HttpPostedFileBase BeneficiaryFile)
        {
            model.Ip = HttpContext.Request.UserHostAddress;
            if (model.Id == 0)
            {
                model.ManagerImageUrl = SaveFile(ManagerFile, pathFile.Resturant);
                model.BeneficiaryImageUrl = SaveFile(BeneficiaryFile, pathFile.Resturant);
            }
            else
            {
                model.ManagerImageUrl = EditFile(ManagerFile, pathFile.Resturant, model.ManagerImageUrl);
                model.BeneficiaryImageUrl = EditFile(BeneficiaryFile, pathFile.Resturant, model.BeneficiaryImageUrl);
            }
            await ResturantBiz.Instance.Save(model);
            return RedirectToAction("Index", "CheckListTypeCartables");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resturant user = await ResturantBiz.Instance.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Resturants/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await ResturantBiz.Instance.Remove(id);
            return RedirectToAction("Index", "CheckListTypeCartables");
        }

        public async Task<ActionResult> CheckList(int ResturantId)
        {
            Resturant res = await Biz.ResturantBiz.Instance.GetInclude(new Resturant() { Id= ResturantId }, "ResturantCheckList.CheckListType");
            if (res != null)
                return View(res);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public async Task<ActionResult> FindResturantCheckList(int ResturantId, int? id)
        {
            if (id == null)
            {
                return PartialView("CheckListSave", new ResturantCheckList() { ResturantId = ResturantId });
            }
            var res = await Biz.ResturantBiz.Instance.FindResturantCheckList(id.Value);
            if (res == null || res.ResturantId != ResturantId)
            {
                return null;
            }
            return PartialView("CheckListSave", res);
        }

        [HttpPost]
        public async Task<ActionResult> SaveResturantCheckList(ResturantCheckList model, System.Web.HttpPostedFileBase File)
        {
            if (File == null)
            {
                return null;
            }
            model.ImageUrl = SaveFile(File, pathFile.Resturant);
            var res = await Biz.ResturantBiz.Instance.SaveResturantCheckList(model);
            //return RedirectToAction("CheckList", "CheckListTypeCartables");
            return Json(new ApiJsonResult() { success = true, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ResturantPersonel(int ResturantId)
        {
            Resturant res = await Biz.ResturantBiz.Instance.GetInclude(new Resturant() { Id = ResturantId }, "ResturantPersonel");
            if (res != null)
                return View("Personel", res);
            return null;
        }

        public async Task<ActionResult> FindResturantPersonel(int ResturantId, int? id)
        {
            if (id == null)
            {
                return PartialView("PersonelSave", new ResturantPersonel() { ResturantId = ResturantId });
            }
            var res = await Biz.ResturantBiz.Instance.FindResturantPersonel(id.Value);
            if (res == null || res.ResturantId != ResturantId)
            {
                return null;
            }
            return PartialView("PersonelSave", res);
        }

        public async Task<ActionResult> SaveResturantPersonel(ResturantPersonel model, System.Web.HttpPostedFileBase File)
        {
            model.Validate();
            if (model.Id == 0)
            {
                model.ImageUrl = SaveFile(File, pathFile.Resturant);
            }
            else
            {
                model.ImageUrl = EditFile(File, pathFile.Resturant, model.ImageUrl);
            }
            var res = await Biz.ResturantBiz.Instance.SaveResturantPersonel(model);
            //return RedirectToAction("Personel", "CheckListTypeCartables");
            return Json(new ApiJsonResult() { success = true, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RemoveResturantPersonel(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantPersonel(id,true);
            //return RedirectToAction("Personel", "CheckListTypeCartables");
            return Json(new ApiJsonResult() {success=true,Message="حذف با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> RemoveResturanCheckList(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantCheckList(id,true);
            //return RedirectToAction("CheckList", "CheckListTypeCartables");
            return Json(new ApiJsonResult() { success = true, Message = "حذف با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> AddorEditnoteChange(int id)
        {
            var res = await Biz.ResturantBiz.Instance.FindandResturantChanges(id,ResturantAddorEditnote.none);
            //return RedirectToAction("CheckList", "CheckListTypeCartables");
            return Json(new ApiJsonResult() { success = true, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }
    }
}