using SurveyWeb.Biz;
using SurveyWeb.Models;
using SurveyWeb.Models.wrapper;
using System.Collections.Generic;
using System.Linq;
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
                Resturant res = await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId);
                if (res == null)
                {
                    res = new Resturant() { UserId = Current_UserId, BeneficiaryTel = Current_User.Mobile, Beneficiary = Current_User.Name + " " + Current_User.Family, Email = Current_User.Email, Cartable = new Cartable() { IsFirstState = true } };
                }
                else
                {
                    List<SurveyWeb.Models.CheckListTypeCartable> notAccepted = await CheckListTypeCartableBiz.Instance.GetAll(x => x.Accepted == false && x.ResturantId == res.Id);
                    if (notAccepted?.Count > 0)
                    {
                        res.act = "";
                        var list = Biz.ResturantBiz.Instance.CheckListType();
                        foreach (var item in notAccepted)
                        {
                            item.DropDownTitle = list.First(x => x.Value == item.CheckListId.ToString()).Text;
                            res.act = item.DropDownTitle + " : " + item.CartableCheckListType;
                        }
                        //res.act = string.Join(",", notAccepted.Select(x => x.CartableCheckListType));
                    }
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
            var res = await Biz.ResturantBiz.Instance.SaveByCurrentUser(model);
            SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            //return View("Resturant", res);
            return RedirectToAction("Index", "Resturant");
        }


        #region ResturantCheckList
        public async Task<ActionResult> ResturantCheckList()
        {
            if (Current_UserId > 0)
            {
                Resturant res = await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId, "ResturantCheckList.CheckListType");
                if (res != null)
                    return PartialView("ResturantCheckList", res);
            }
            return null;
        }
        public async Task<ActionResult> FindResturantCheckList(int ResturantId, int? id)
        {
            if (id == null)
            {
                return PartialView("ResturantCheckListSave", new ResturantCheckList() { ResturantId = ResturantId });
            }
            var res = await Biz.ResturantBiz.Instance.FindResturantCheckList(id.Value);
            if (res == null || res.ResturantId != ResturantId)
            {
                return null;
            }
            return PartialView("ResturantCheckListSave", res);
        }

        [HttpPost]
        public async Task<ActionResult> SaveResturantCheckList(ResturantCheckList model, System.Web.HttpPostedFileBase File)
        {
            if (File == null)
            {
                return Json(new ApiJsonResult() { success = false, Message = "یک فایل انتخاب کنید" }, JsonRequestBehavior.AllowGet);
            }
            model.ImageUrl = SaveFile(File, pathFile.Resturant);
            var res = await Biz.ResturantBiz.Instance.SaveResturantCheckList(model);
            await Biz.ResturantBiz.Instance.FindandResturantChanges(model.ResturantId, ResturantAddorEditnote.AddCheckList);
            //  SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            // return RedirectToAction("ResturantCheckList", "Resturant");
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> RemoveResturanCheckList(int id)
        {
            var ResturantId = await Biz.ResturantBiz.Instance.RemoveResturantCheckList(id);
            await Biz.ResturantBiz.Instance.FindandResturantChanges(ResturantId, ResturantAddorEditnote.RemoveCheckList);
            //  SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            //return RedirectToAction("ResturantCheckList", "Resturant");
            return Json(new ApiJsonResult() { success = ResturantId > 0, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        #endregion ResturantCheckList
        #region ResturantPersonel
        public async Task<ActionResult> ResturantPersonel()
        {
            if (Current_UserId > 0)
            {
                Resturant res = await Biz.ResturantBiz.Instance.FindByUserId(Current_UserId, "ResturantPersonel");
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
            await Biz.ResturantBiz.Instance.FindandResturantChanges(model.ResturantId, model.Id == 0 ? ResturantAddorEditnote.AddPersonel : ResturantAddorEditnote.EditPersonel);

            // SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            // return RedirectToAction("ResturantPersonel", "Resturant");
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RemoveResturantPersonel(int id)
        {
            var ResturantId = await Biz.ResturantBiz.Instance.RemoveResturantPersonel(id);
            await Biz.ResturantBiz.Instance.FindandResturantChanges(ResturantId, ResturantAddorEditnote.RemovePersonel);

            //  SetViewBagSuccessMessage("اطلاعات شما با موفقیت ثبت شد. ");
            //return RedirectToAction("ResturantPersonel", "Resturant");
            return Json(new ApiJsonResult() { success = ResturantId > 0, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        #endregion ResturantPersonel

        #region ResturantPersonelEducation
        public async Task<ActionResult> ResturantPersonelEducation(int personelId)
        {
            if (Current_UserId > 0)
            {
                ViewBag.personelId = personelId;
                List<ResturantPersonelEducation> res = await Biz.ResturantBiz.Instance.ResturantPersonelEducations(personelId);
                return PartialView(res);
            }
            return null;
        }
        public ActionResult ResturantPersonelEducationSave(int personelId)
        {
            return PartialView(new ResturantPersonelEducation() { ResturantPersonelId = personelId });
        }

        [HttpPost]
        public async Task<ActionResult> ResturantPersonelEducationSave(ResturantPersonelEducation model)
        {
            model.Validate();
            model.CreatedDate = System.DateTime.Now;
            var res = await Biz.ResturantBiz.Instance.SaveResturantPersonelEducation(model);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RemoveResturantPersonelEducation(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantPersonelEducation(id);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        #endregion ResturantPersonel
        #region ResturantPersonelCourse
        public async Task<ActionResult> ResturantPersonelCourse(int personelId)
        {
            if (Current_UserId > 0)
            {
                ViewBag.personelId = personelId;
                List<ResturantPersonelCourse> res = await Biz.ResturantBiz.Instance.ResturantPersonelCourses(personelId);
                return PartialView(res);
            }
            return null;
        }
        public ActionResult ResturantPersonelCourseSave(int personelId)
        {
            return PartialView(new ResturantPersonelCourse() { ResturantPersonelId = personelId });
        }

        [HttpPost]
        public async Task<ActionResult> ResturantPersonelCourseSave(ResturantPersonelCourse model)
        {
            model.Validate();
            model.CreatedDate = System.DateTime.Now;
            var res = await Biz.ResturantBiz.Instance.SaveResturantPersonelCourse(model);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RemoveResturantPersonelCourse(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantPersonelCourse(id);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        #endregion ResturantPersonel
        #region ResturantPersonelLanguage
        public async Task<ActionResult> ResturantPersonelLanguage(int personelId)
        {
            if (Current_UserId > 0)
            {
                ViewBag.personelId = personelId;
                List<ResturantPersonelLanguage> res = await Biz.ResturantBiz.Instance.ResturantPersonelLanguages(personelId);
                return PartialView(res);
            }
            return null;
        }
        public ActionResult ResturantPersonelLanguageSave(int personelId)
        {
            return PartialView(new ResturantPersonelLanguage() { ResturantPersonelId = personelId });
        }

        [HttpPost]
        public async Task<ActionResult> ResturantPersonelLanguageSave(ResturantPersonelLanguage model)
        {
            model.Validate();
            model.CreatedDate = System.DateTime.Now;
            var res = await Biz.ResturantBiz.Instance.SaveResturantPersonelLanguage(model);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RemoveResturantPersonelLanguage(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantPersonelLanguage(id);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        #endregion ResturantPersonel
        #region ResturantPersonelJob
        public async Task<ActionResult> ResturantPersonelJob(int personelId)
        {
            if (Current_UserId > 0)
            {
                ViewBag.personelId = personelId;
                List<ResturantPersonelJob> res = await Biz.ResturantBiz.Instance.ResturantPersonelJobs(personelId);
                return PartialView(res);
            }
            return null;
        }
        public ActionResult ResturantPersonelJobSave(int personelId)
        {
            return PartialView(new ResturantPersonelJob() { ResturantPersonelId = personelId });
        }

        [HttpPost]
        public async Task<ActionResult> ResturantPersonelJobSave(ResturantPersonelJob model)
        {
            model.Validate();
            model.CreatedDate = System.DateTime.Now;
            var res = await Biz.ResturantBiz.Instance.SaveResturantPersonelJob(model);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RemoveResturantPersonelJob(int id)
        {
            var res = await Biz.ResturantBiz.Instance.RemoveResturantPersonelJob(id);
            return Json(new ApiJsonResult() { success = res, Message = "عملیات با موفقیت انجام شد" }, JsonRequestBehavior.AllowGet);
        }

        #endregion ResturantPersonel
    }
}