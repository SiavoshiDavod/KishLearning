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
                catch (Exception e)
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

        [HttpPost]
        public ActionResult Login([System.Web.Http.FromBody]string email, [System.Web.Http.FromBody]string pass, string ReturnUrl)
        {
            string message = "";
            if (string.IsNullOrEmpty(email))
            {
                message = "ایمیل خود را وارد کنید";
            }
            if (string.IsNullOrEmpty(pass))
            {
                message = "رمز عبور خود را وارد کنید";
            }
            var user = Biz.UserBiz.Instance.FindByUserName(email);
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
                message = "حساب کاربری شما غیرفعال است";
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
            if (user == null) return View("UserRegister");

            try
            {
                user.Archive = true;
                await Biz.UserBiz.Instance.Save(user);
            }
            catch (Exception e)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                ViewBag.Message = e.Message;
                return View("UserRegister");
            }

            FormsAuthentication.SetAuthCookie(user.UserName, true);
            SetSessionUser(user);
            return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/Dashboard/index" : ReturnUrl);
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
    }
}