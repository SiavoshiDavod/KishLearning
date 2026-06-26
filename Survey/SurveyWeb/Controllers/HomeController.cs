using SurveyWeb.Biz;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace SurveyWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> OrgIntro(int id)
        {
            var obj = await Biz.OrgIntroBiz.Instance.Get(id);
            return View(obj);
        }

        public ActionResult UserComment(int? id, PageType type = PageType.None)
        {
            return PartialView("UserComment", new UserComment() { PageTypeId = type, TypeId = id });
        }

        [HttpPost]
        public async Task<ActionResult> UserComment(UserComment model)
        {
            model.Id = 0;
            if (Current_UserId != 0)
            {
                model.UserId = Current_UserId;
                model.Name = Current_User?.Name + " " + Current_User?.Family;
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    model.Name = Current_User?.UserName;
                }

            }
            //CheckGoogleRecapcha(model.googlerecaptcha);

            model.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                await UserCommentBiz.Instance.Save(model);

                TempData["SuccessMessage"] = "پیام شما با موفقیت ثبت شد. در اسرع وقت به آن رسیدگی می شود ";
            }
            else
            {
                try
                {
                    model.Validate();
                    ModelState.AddModelError(string.Empty, "اطلاعات را کامل پرکنید");
                }
                catch (Exception)
                {
                    //  ModelState.AddModelError(string.Empty,e.Message);
                }
            }

            return Redirect("/");
        }

        public ActionResult ShowComments(int? id, PageType type = PageType.None)
        {
            var comments = UserCommentBiz.Instance.GetAllsync(x => x.PageTypeId == type && x.TypeId == id && x.Status);
            return PartialView("ShowComment", comments);
        }

        public ActionResult News(int id)
        {
            News obj = Biz.NewsBiz.Instance.GetIncludeSync(new Models.News() { Id = id }, "Author", "NewsGroup");
            if (obj != null)
            {
                NewsBiz.Instance.AddVisitCount(id).GetAwaiter();
            }
            return View(obj);
        }

        public async Task<ActionResult> AllNews(string search, int skip = 0, int? groupId = null, string group = null, int? authorId = null)
        {
            var take = 8;
            var list = await Biz.NewsBiz.Instance.GetAllPage(x => (string.IsNullOrEmpty(search) || x.Title.Contains(search)) && (groupId == null || x.NewsGroupId == groupId), skip * take, take);
            var count = await Biz.NewsBiz.Instance.GetCount(x => (string.IsNullOrEmpty(search) || x.Title.Contains(search)) && (groupId == null || x.NewsGroupId == groupId));
            ViewBag.search = search;
            ViewBag.Count = count;
            ViewBag.groupId = groupId;
            ViewBag.group = group;
            ViewBag.currentPage = skip + 1;
            ViewBag.totalPage = (int)Math.Ceiling(count / (double)take);
            return View(list);
        }

        public ActionResult Advertising(int id)
        {
            Advertising obj = Biz.AdvertisingBiz.Instance.GetIncludeSync(new Models.Advertising() { Id = id }, "Resturant.ResturantType", "AdvertisingAttachements", "Resturant.ResturantMenu.ResturantDetailMenus");
            if (obj == null)
            {
                throw new Exception("صفحه مورد نظر شما یافت نشد");
            }
            //if (obj != null)
            //{
            //    NewsBiz.Instance.AddVisitCount(id).GetAwaiter();
            //}
            return View(obj);
        }

        public async Task<ActionResult> AllAdvertising(string search, int skip = 0, int? typeId = null, string typeName = "", bool? isMusical = null)
        {
            var take = 8;
            var list = await Biz.AdvertisingBiz.Instance.GetAllPage(x => (string.IsNullOrEmpty(search) || x.Resturant.Name.Contains(search)) && (typeId == null || x.Resturant.ResturantTypeId == typeId) && (isMusical == null || isMusical == false || x.Resturant.IsMusical == true), skip * take, take, "Resturant.ResturantType");
            var count = await Biz.ResturantBiz.Instance.GetCount(x => x.Advertising.Any(/*z => !z.Archive*/) && (string.IsNullOrEmpty(search) || x.Name.Contains(search)) && (typeId == null || x.ResturantTypeId == typeId) && (isMusical == null || isMusical == false || x.IsMusical == true));
            ViewBag.typeId = typeId;
            ViewBag.typeName = typeName;
            ViewBag.search = search;
            ViewBag.Count = count;
            ViewBag.currentPage = skip + 1;
            ViewBag.totalPage = (int)Math.Ceiling(count / (double)take);
            return View(list);
        }

        public ActionResult AllActiveSurvey()
        {
            return View();
        }

        public ActionResult AllActiveSurveyPrivate()
        {
            return View(Biz.SurveyEntityBiz.Instance.GetAllActivePrivateByUserId(Current_UserId));
        }

        public async Task<ActionResult> SubMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuSub dynamicForm = await Biz.MenuSubBiz.Instance.Get(id.Value);
            if (dynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(dynamicForm);
        }

        public ActionResult Tender()
        {
            return View();
        }


        public ActionResult SupervisionInspection()
        {
            return View();
        }

        public ActionResult EvaluationPerformance()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewsSubscription(NewsSubscription model)
        {
            //model.Ip = HttpContext.Request.UserHostAddress;
            try
            {
                var res = await NewsSubscriptionBiz.Instance.Save(model);
                TempData["SuccessMessage"] = "ایمیل شما با موفقیت ثبت شد";
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                //return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Redirect("/");
        }

        public async Task<ActionResult> Regulation()
        {
            return View(await Biz.RegulationBiz.Instance.GetAll());
        }

        public ActionResult Complaint()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Complaint(Complaint model)
        {
            try
            {
                var files = Request.Files;
                if (files != null && files[0] != null)
                {
                    model.Attachment = SaveFile(files[0], pathFile.Complaint);
                }
                model.Ip = HttpContext.Request.UserHostAddress;
                var res = await ComplaintBiz.Instance.Save(model);
                TempData["SuccessMessage"] = " کدرهگیری خود را یادداشت کنید: " + res.TrackingCode;
                return Redirect("/");
            }
            catch (Exception e)
            {
                SetLog(e);
                ViewBag.Message = e.Message;
                return View(model);
            }
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ContactUs(ContactUs model)
        {
            try
            {
                model.Ip = HttpContext.Request.UserHostAddress;
                var res = await ContactUsBiz.Instance.Save(model);
                ViewBag.Message = "پیام شما با موفقیت دخیره شد";
                TempData["SuccessMessage"] = "پیام شما با موفقیت دخیره شد";
                return Redirect("/");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View(model);
            }

        }

        public ActionResult Suggestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Suggestion(Suggestion suggestion)
        {
            try
            {
                var files = Request.Files;
                if (files != null)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                suggestion.Attachment1 = SaveFile(files[i], pathFile.Suggestion);
                                break;
                            case 1:
                                suggestion.Attachment2 = SaveFile(files[i], pathFile.Suggestion);

                                break;
                            //case 2:
                            //    suggestion.Attachment3 = SaveFile(files[i], pathFile.Suggestion);
                            //    break;
                            //case 3:
                            //    suggestion.Attachment4 = SaveFile(files[i], pathFile.Suggestion);
                            //    break;
                            //case 4:
                            //    suggestion.Attachment5 = SaveFile(files[i], pathFile.Suggestion);
                            //    break;
                            default:
                                break;
                        }
                    }
                }
                suggestion.Ip = HttpContext.Request.UserHostAddress;
                var res = await SuggestionBiz.Instance.Save(suggestion);
                TempData["SuccessMessage"] = " کدرهگیری خود را یادداشت کنید: " + res.TrackingCode;
                return Redirect("/");
            }
            catch (Exception e)
            {
                SetLog(e);
                ViewBag.Message = e.Message;
                return View(suggestion);
            }
        }
        public ActionResult Idea()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Idea(Idea idea)
        {
            try
            {
                var files = Request.Files;
                if (files != null)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                idea.Attachment1 = SaveFile(files[i], pathFile.Idea);
                                break;
                            case 1:
                                idea.Attachment2 = SaveFile(files[i], pathFile.Idea);

                                break;
                            case 2:
                                idea.Attachment3 = SaveFile(files[i], pathFile.Idea);
                                break;
                            case 3:
                                idea.Attachment4 = SaveFile(files[i], pathFile.Idea);
                                break;
                            case 4:
                                idea.Attachment5 = SaveFile(files[i], pathFile.Idea);
                                break;
                            default:
                                break;
                        }
                    }
                }
                idea.Ip = HttpContext.Request.UserHostAddress;
                var res = await IdeaBiz.Instance.Save(idea);
                TempData["SuccessMessage"] = " کدرهگیری خود را یادداشت کنید: " + res.TrackingCode;
                return Redirect("/");
            }
            catch (Exception e)
            {
                SetLog(e);
                ViewBag.Message = e.Message;
                return View(idea);
            }
        }

        public async Task<ActionResult> Faq()
        {
            return View(await Biz.FaqBiz.Instance.GetAll());
        }

        public async Task<ActionResult> Survey(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyEntity obl = await SurveyEntityBiz.Instance.GetIncludeQuestion(id.Value, HttpContext.Request.UserHostAddress, Current_UserId);
            if (obl == null)
            {
                return HttpNotFound();
            }

            return View(obl);
        }

        [HttpPost]
        public async Task<ActionResult> Answer(List<SurveyAnswer> answers, System.Web.HttpPostedFileBase File)
        {
            if (answers?.Count <= 0)
            {
                SetLog(e: new Exception("no answer was found"));
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var files = Request.Files;
                if (files != null)
                {
                    //foreach (var item in files)
                    //{
                    //    var QuestionOption = SaveFile(File, pathFile.Answer);
                    //}
                }
                await SurveyAnswerBiz.Instance.SaveBatch(answers, HttpContext.Request.UserHostAddress, Current_UserId);
                await SurveyEntityBiz.Instance.AddAnswer(answers.First().SurveyEntityId);
                return Json(true, JsonRequestBehavior.AllowGet);
                // return RedirectToAction("Index", "SurveyAnswers");
            }
            catch (Exception e)
            {
                SetLog(e);
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UserRegister(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Message = "";
            return View();
        }

        public ActionResult UserLogin(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Message = "";
            return View();
        }

        public ActionResult RegisterOrLogin(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Message = "";
            return View();
        }

        public ActionResult RegisterCustomer(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Message = "";
            return View();
        }

        public ActionResult ForgetPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPass(string email)
        {
            try
            {
                var newPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
                UserBiz.Instance.ResetPass(email, newPass);
                SendEmail.AlertForClass(email, "بازیابی کلمه عبور", "<div  dir='rtl' style='text-align: right;' >کلمه عبور شما بازیابی شد<br>" + newPass + "</div>", SiteSetting.GetSetting.Instance.Get());
                TempData["SuccessMessage"] = "کلمه عبور شما بازیابی شد و به ایمیل شما ارسال گردید";
                return Redirect("/");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }

            return View("ForgetPass");
        }

        [HttpPost]
        public ActionResult Login([System.Web.Http.FromBody]string username, [System.Web.Http.FromBody]string pass, string ReturnUrl)
        {
            username = username.Trim();
            string message = "";
            if (string.IsNullOrEmpty(username))
            {
                message = "نام کاربری خود را وارد کنید";
            }
            if (string.IsNullOrEmpty(pass))
            {
                message = "رمز عبور خود را وارد کنید";
            }
            var user = Biz.UserBiz.Instance.FindByUserName(username);
            if (user == null)
            {
                message = "نام کاربری معتبر نمی باشد";
            }
            else if (user.Pass != pass)
            {
                message = "کلمه عبور معتبر نمی باشد";
            }
            else if (user.Archive)
            {
                TempData["ErrorMessage"] = "حساب کاربری شما در حال بررسی است. پس از تایید ،می توانید اطلاعات مرکزپذیرایی خود را در داشبورد تکمیل کنید ";
                return Redirect("/");
            }
            if (string.IsNullOrEmpty(message))
            {
                FormsAuthentication.SetAuthCookie(user.UserName, true);
                SetSessionUser(user);
                return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/Dashboard/index" : ReturnUrl);
            }

            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Message = message;
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<ActionResult> Register(Models.User user, string ReturnUrl = "/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (user == null) return View("UserRegister");
            if (user.Pass != user.ConfirmPass)
            {
                ViewBag.Message = "تکرار کلمه عبور اشتباه است";
                return View("UserRegister", user);
            }
            try
            {
                user.UserName = user.UserName.Trim();
                user.Email = user.Email?.Trim();
                user.Validate();
                user.Archive = true;
                user.RoleId = Models.Roles.User;
                await Biz.UserBiz.Instance.Save(user);
                TempData["SuccessMessage"] = "ثبت نام اولیه شما با موفقیت انجام شد، منتظر تایید مدیر سیستم بمانید. جامعه مراکز پذیرایی کیش ";
                return Redirect("/");
            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
                return View("UserRegister", user);
            }
            //FormsAuthentication.SetAuthCookie(user.UserName, true);
            //SetSessionUser(user);
            //return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/Dashboard/index" : ReturnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterCustomer(Models.User user, string ReturnUrl = "/")
        {
            if (user == null) return View("UserRegister");

            try
            {
                user.Validate();
                user.Archive = false;
                user.RoleId = Models.Roles.Customer;
                await Biz.UserBiz.Instance.Save(user);
                //ViewBag.Message = "ثبت نام شما با موفقیت انجام شد. پس از تایید ،می توانید وارد محیط کاربری خود شوید ";
            }
            catch (Exception e)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                ViewBag.Message = e.Message;
                return View("UserRegister");
            }
            return View();
            //FormsAuthentication.SetAuthCookie(user.UserName, true);
            //SetSessionUser(user);
            //return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/Dashboard/index" : ReturnUrl);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            SetSessionLogout();
            return Redirect("/");
        }

        public void SetSessionLogout()
        {
            Session.Abandon();
            Session.Clear();
        }

        [AllowAnonymous]
        public ActionResult jobs()
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            ViewBag.PageTitle = "همه موقعیت های شغلی منتشر شده به صورت عمومی";

            var list = JobPositionBiz.Instance.FindAll(true);
            return View("~/Views/JobBoard/Client/PublicIndex.cshtml", list);
        }

        public ActionResult JobDetails(int id)
        {
            ViewBag.Categories = JobCategoryBiz.Instance.FindAll().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            var item = JobPositionBiz.Instance.JobPositionDetails(id, true);
            return View("~/Views/JobBoard/Client/PublicDetails.cshtml", item);
        }
        public ActionResult BoardDirectors()
        {
            var item = BoardDirectorBiz.Instance.FindAll();
            return View(item);
        }
        public ActionResult BoardDirector(int id)
        {
            var item = BoardDirectorBiz.Instance.FindById(id);
            return View(item);
        }
    }
}