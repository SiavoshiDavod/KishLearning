using SurveyWeb.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class ResturantController : BaseProfileController
    {
        public async Task<ActionResult> Index()
        {
            if (Current_UserId > 0)
            {
                var res =await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId);
                if (res == null)
                {
                    res = new Resturant() { UserId = Current_UserId, BeneficiaryTel = Current_User.Mobile, Beneficiary = Current_User.Name + " " + Current_User.Family,Email=Current_User.UserName,Cartable=new Cartable() { IsFirstState=true} };
                }
                return View("Resturant", res);
            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> Create(Resturant model, System.Web.HttpPostedFileBase ManagerFile, System.Web.HttpPostedFileBase BeneficiaryFile)
        {
            model.UserId = Current_UserId;
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
            var res = await Biz.ResturantBiz.Instance.Save(model);
            SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            //return View("Resturant", res);
            return RedirectToAction("Index", "Resturant");
        }

        public async Task<ActionResult> ResturantCheckList()
        {
            if (Current_UserId > 0)
            {
                Resturant res =await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId, "ResturantCheckList.CheckListType");
                if (res != null)
                    return PartialView("ResturantCheckList", res);
            }
            return null;
        }
        public async Task<ActionResult> FindResturantCheckList(int ResturantId , int? id)
        {
            if (id== null)
            {
                return PartialView("ResturantCheckListSave", new ResturantCheckList() { ResturantId=ResturantId});
            }
            var res =await Biz.ResturantBiz.Instance.FindResturantCheckList(id.Value);
            if (res==null || res.ResturantId!=ResturantId)
            {
                return null;
            }
            return PartialView("ResturantCheckListSave", res);
        }

        [HttpPost]
        public async Task<ActionResult> SaveResturantCheckList(ResturantCheckList model, System.Web.HttpPostedFileBase File)
        {
            if (File==null)
            {
                return null;
            }
            model.ImageUrl = SaveFile(File, pathFile.Resturant);
            var res = await Biz.ResturantBiz.Instance.SaveResturantCheckList(model);
            //  SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            return RedirectToAction("ResturantCheckList", "Resturant");
        }

        public async Task<ActionResult> ResturantPersonel()
        {
            if (Current_UserId > 0)
            {
                Resturant res =await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId, "ResturantPersonel");
                if (res != null)
                    return PartialView("ResturantPersonel", res);
            }
            return null;
        }

        public async Task<ActionResult> FindResturantPersonel(int ResturantId, int? id)
        {
            if (id == null)
            {
                return PartialView("ResturantPersonelSave", new ResturantPersonel() { ResturantId = ResturantId });
            }
            var res = await Biz.ResturantBiz.Instance.FindResturantPersonel(id.Value);
            if (res == null || res.ResturantId != ResturantId)
            {
                return null;
            }
            return PartialView("ResturantPersonelSave", res);
        }

        public async Task<ActionResult> SaveResturantPersonel(ResturantPersonel model, System.Web.HttpPostedFileBase File)
        {
            if (model.Id==0)
            {
                model.ImageUrl = SaveFile(File, pathFile.Resturant);
            }
            else
            {
                model.ImageUrl = EditFile(File, pathFile.Resturant,model.ImageUrl);
            }
            var res = await Biz.ResturantBiz.Instance.SaveResturantPersonel(model);
            // SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            return RedirectToAction("ResturantPersonel", "Resturant");
        }

        public async Task<ActionResult> RemoveResturantPersonel(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantPersonel(id);
            //  SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            return RedirectToAction("ResturantPersonel", "Resturant");
            //return Json(res, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> RemoveResturanCheckList(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantCheckList(id);
            //  SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            return RedirectToAction("ResturantCheckList", "Resturant");
        }
    }
}