using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SenakLearn.Models;
using SenakLearn.Biz;
using System.Net;
using System.Web.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using SenakLearn.Models.wrapper;
using System.Linq.Dynamic;
using System.IO;
using System.Web.Hosting;
using SenakLearn.Models.Common;
using System.Web.Http.Results;
using System.Text;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SenakLearn.Controllers
{
    public class HomeController : BaseController
    {
        readonly SWEntities _db = new SWEntities();
        public ActionResult Index()
        {
            ViewBag.isHomePage = true;

            return View();
        }
        public ActionResult News(int id)
        {
            News obj = Biz.NewsBiz.Instance.GetInclude(new Models.News() { Id = id }, "Author", "NewsGroup");
            if (obj != null)
            {
                NewsBiz.Instance.AddVisitCount(id);
            }
            return View(obj);
        }
        public ActionResult AllNews(string search, int skip = 0, int? groupId = null, string group = null, int? authorId = null)
        {
            var take = 8;
            var list = Biz.NewsBiz.Instance.GetAllPage(x => (string.IsNullOrEmpty(search) || x.Title.Contains(search)) && (groupId == null || x.NewsGroupId == groupId), skip * take, take);
            var count = Biz.NewsBiz.Instance.GetCount(x => (string.IsNullOrEmpty(search) || x.Title.Contains(search)) && (groupId == null || x.NewsGroupId == groupId));
            ViewBag.search = search;
            ViewBag.Count = count;
            ViewBag.groupId = groupId;
            ViewBag.group = group;
            ViewBag.currentPage = skip + 1;
            ViewBag.totalPage = (int)Math.Ceiling(count / (double)take);
            return View(list);
        }
        [AllowAnonymous]
        public async Task FetchVideo(Guid videoId)
        {
            try
            {
                string serverPathVideo = HostingEnvironment.MapPath("/images/" + pathFile.VideoFile + "/");
                var bytes = VideoFileBiz.Instance.GetBineryVideo(videoId, serverPathVideo);
                //VideoFile VideoFile = db.VideoFiles.Find(videoId);
                //var videoDir = Server.MapPath("/images/" + pathFile.VideoFile + "/" + VideoFile.myFile);
                //byte[] bytes = System.IO.File.ReadAllBytes(videoDir);

                long fileSize = bytes.Length;
                long totalByte = fileSize - 1;
                long startByte = 0;
                long endByte = totalByte;
                int bufferSize = 1024 * 1024; // 24KB buffer size

                if (!string.IsNullOrEmpty(Request.Headers["X-Playback-Session-Id"]))
                    Response.AddHeader("X-Playback-Session-Id", Request.Headers["X-Playback-Session-Id"]);

                if (!string.IsNullOrEmpty(Request.Headers["Range"]))
                {
                    //Range: <unit>=<range-start>
                    string range = Request.Headers["Range"].Replace("bytes=", "");
                    string[] rangeParts = range.Split('-');
                    startByte = long.Parse(rangeParts[0]);
                    if (rangeParts.Length > 1 && !string.IsNullOrEmpty(rangeParts[1]))
                        endByte = long.Parse(rangeParts[1]);
                }

                // recalculate after range has been interpreted
                int bytesToRead = Math.Min((int)(endByte - startByte + 1), bufferSize);

                Response.AddHeader("Content-Range", $"bytes {startByte}-{endByte}/{fileSize}");
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AddHeader("Content-Type", "video/mp4");
                Response.AddHeader("Connection", "Keep-Alive");
                Response.AddHeader("Content-Name", "");
                Response.AddHeader("Content-Version", "1.0");
                Response.AddHeader("Content-Vendor", "XMP");
                Response.AddHeader("Content-Size", bytesToRead.ToString());
                Response.AddHeader("Content-Length", bytesToRead.ToString());

                Response.StatusCode = 206;
                Response.ContentType = "video/mp4";

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    memoryStream.Seek(startByte, SeekOrigin.Begin);

                    byte[] buffer = new byte[bufferSize];
                    long bytesRemaining = bytesToRead;

                    while (bytesRemaining > 0)
                    {
                        int bytesRead = await memoryStream.ReadAsync(buffer, 0, bytesToRead);

                        if (bytesRead == 0)
                            break;

                        if (Response.IsClientConnected)
                        {
                            await Response.OutputStream.WriteAsync(buffer, 0, bytesRead);
                            await Response.OutputStream.FlushAsync();
                            bytesRemaining -= bytesRead;
                        }
                        else
                        {
                            break; // Client disconnected
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Contract()
        {
            var setting = SiteSetting.GetSetting.Instance.Get();
            ViewBag.Contract = setting.Contract;
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicForm dynamicForm = _db.DynamicForms.Find(id);
            if (dynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(dynamicForm);
        }
        public ActionResult Step(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicForm dynamicForm = _db.DynamicForms.Find(id);
            if (dynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(dynamicForm);
        }
        public ActionResult ShowAllClasses(int? groupId, int? teacherId, string keyword,int page=1)
        {
            ViewBag.groupId = groupId;
            ViewBag.teacherId = teacherId;
            ViewBag.keyword = keyword;
            
            Pagination<Book> model = new Pagination<Book>();
            model.Data = BookBiz.Instance.GetAllBooks(i => (groupId == null || groupId == i.GroupId) && (string.IsNullOrEmpty(keyword) || i.Keyword.Contains(keyword)) );
            model.CurrentPage = page;
            return View(model);
        }
        public ActionResult joinUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> joinUs(JoinUs model, HttpPostedFileBase files)
        {
            var message = "بزودی در صورت تائید همکاری ایمیل و اس ام اسی جهت تکمیل مراحل کار خدمتتان ارسال خواهد شد";
            try
            {
                if (ModelState.IsValid)
                {
                    if (Current_learn_userId > 0)
                    {
                        if (Current_learn_user.RoleId != Models.Roles.User)
                        {
                            throw new Exception("لطفا با کاربری ادمین یا استاد فرم همکاری با ما را پر نکنید");
                        }
                        model.UserId = Current_learn_userId;
                    }
                    else
                    {
                        var user = Biz.UserBiz.Instance.RegisterUser(new learn_user() { Email = model.Email, Mobile = model.Mobile, Name = model.Name, Family = model.Family, NationaCode = model.NationalCode, user_name = model.NationalCode, password = model.Mobile });
                        model.UserId = user.id;
                        message += ".همچنین در سامانه آموزش مجازی ، نام کاربری شما کدملی و رمز عبور، شماره موبایلتان است";
                        FormsAuthentication.SetAuthCookie(user.user_name, true);
                        SetSessionUser(user);
                        await Biz.UserBiz.Instance.SendToAdminAsync(user);
                    }
                    model.ResumeFile = SaveFile(files, pathFile.JoinUs);
                    JoinUsBiz.Instance.Save(model);
                    SetViewBagSuccessMessage(message);
                    ViewBag.isHomePage = true;
                    return View("Index");
                    // return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }
        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        [OutputCache(Duration = 15)]
        public ActionResult Cours(int? groupId, int? teacherId, string keyword)
        {
            ViewBag.groupId = groupId;
            ViewBag.teacherId = teacherId;
            ViewBag.keyword = keyword;
            var cours = _db.learn_cours.Where(i => i.status && (groupId == null || groupId == i.id_group) && (teacherId == null || teacherId == i.id_teacher) && (string.IsNullOrEmpty(keyword) || i.name.Contains(keyword)) && i.TypeCours == null)
                .OrderByDescending(o => o.id).ToList();

            return PartialView(cours);
        }
        [OutputCache(Duration = 15)]
        public ActionResult Podcast(int? groupId, int? teacherId, string keyword)
        {
            ViewBag.groupId = groupId;
            ViewBag.teacherId = teacherId;
            ViewBag.keyword = keyword;
            var podcast = _db.learn_cours.Where(i => i.status && (groupId == null || groupId == i.id_group) && (teacherId == null || teacherId == i.id_teacher) && (string.IsNullOrEmpty(keyword) || i.name.Contains(keyword)) && i.TypeCours == 2)
                .OrderByDescending(o => o.id).ToList();

            return PartialView(podcast);
        }
        [OutputCache(Duration = 15)]
        public ActionResult OnlineClass(int? groupId, int? teacherId, string keyword)
        {
            ViewBag.groupId = groupId;
            ViewBag.teacherId = teacherId;
            ViewBag.keyword = keyword;
            var cours = _db.OnlineClasses.Where(i => i.ClassType != Enums.OnlineClassType.Archived && (groupId == null || groupId == i.id_learn_cours_group) && (teacherId == null || teacherId == i.id_learn_teacher) && (string.IsNullOrEmpty(keyword) || i.name.Contains(keyword))).ToList();

            return PartialView(cours);
        }
        //[OutputCache(Duration = 30)]
        //public ActionResult Slider()
        //{
        //    var slide = _db.learn_Slider.Where(i => i.Status).ToList();

        //    return PartialView(slide);
        //}
        public ActionResult CoursDetail(int id)
        {
            var cours = _db.learn_cours.SingleOrDefault(i => i.status && i.id == id);
            return PartialView(cours);
        }
        [OutputCache(Duration = 25)]
        public ActionResult Teacher()
        {
            var teacher = _db.learn_teacher.Where(i => i.status).ToList();

            return PartialView(teacher);
        }
        public ActionResult TeacherCours(int id)
        {
            // var cours = (_db.learn_cours.Where(i => i.id_teacher == id && i.status)).ToList();
            var teacher = _db.learn_teacher.Where(i => i.id == id).FirstOrDefault();
            return PartialView(teacher);
        }

        public ActionResult AllUserCount()
        {
            var count = Biz.UserBiz.Instance.AllUserCount();

            return Json(new
            {
                count
            },
          JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SiteReviewCount()
        {
            return Json(await Biz.SiteReviewCountBiz.Instanse.GetAllSiteReviewCountForHomePageAsync(),
          JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Accept()
        //{
        //    var list = (_db.learn_certficate.Where(i=> i.status).OrderByDescending(i=>i.date).DistinctBy(i=>i.id_cours)).ToList();

        //    return PartialView(list);
        //}
        //[OutputCache(Duration = 15)]
        //public ActionResult ListNews()
        //{
        //    var list = _db.learn_News.Where(i => i.Status).ToList();
        //    return
        //        PartialView(
        //            list.OrderByDescending(i => i.CreateDate)
        //                .Where(i => i.CreateDate.AddMonths(3) >= DateTime.Now)
        //                .ToList());
        //}
        //[Route("News/{id}/{title}")]
        //public ActionResult ShowNews(int id, string title)
        //{


        //    var news = _db.learn_News.Find(id);
        //    if (news != null)
        //    {
        //        news.NewsSee += 1;
        //        _db.SaveChanges();
        //    }
        //    return news == null ? View("Index") : View(news);
        //}

        //public ActionResult ShowNews(int id)
        //{
        //    var news = _db.learn_News.SingleOrDefault(i => i.NewsID == id);
        //    return news==null ? View("Index") : View(news);
        //}
        //[OutputCache(Duration = 15)]
        //public ActionResult Paper()
        //{
        //    var list = _db.learn_Paper.Where(i => i.Status);
        //    return PartialView(list);
        //}
        //[Route("Paper/{id}/{title}")]
        //public ActionResult ShowPaper(int id, string title)
        //{
        //    var item = _db.learn_Paper.SingleOrDefault(i => i.IdPaper == id);
        //    if (item != null)
        //    {
        //        item.SeePaper += 1;
        //        _db.SaveChanges();
        //    }
        //    return item == null ? View("Index") : View(item);
        //}
        //public ActionResult ShowComments(int id, int type)
        //{
        //    return PartialView(_db.learn_Comment.Where(c => c.RelatedId == id && c.RelatedType == type).ToList());
        //}

        //public ActionResult CreateComment(int idRelated, int type, int? parentid)
        //{
        //    return PartialView(new learn_Comment()
        //    {
        //        ParentId = parentid,
        //        RelatedId = idRelated,
        //        RelatedType = type
        //    });
        //}

        //public HtmlString ListCoursGroup()
        //{
        //    string result = String.Empty;

        //    var list = _db.learn_cours_group.ToList();
        //    list.ForEach(i =>
        //    {
        //        result += "<li><a href='/DownloadBook/Index?pageIndex=1&id_coursGroup=" + i.id + "'>" + i.name + "</a>" + "\n";
        //    });
        //    HtmlString ret = new HtmlString(result);
        //    return ret;
        //}
        public ActionResult AllActiveSurvey()
        {
            return View();
        }
        public ActionResult AllActiveSurveyPrivate()
        {
            return View(Biz.SurveyEntityBiz.Instance.GetAllActivePrivateByUserId(Current_learn_userId));
        }
        public async Task<ActionResult> Survey(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurveyEntity obl = await SurveyEntityBiz.Instance.GetIncludeQuestion(id.Value, HttpContext.Request.UserHostAddress, Current_learn_userId);
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
                //SetLog(e: new Exception("no answer was found"));
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
                await SurveyAnswerBiz.Instance.SaveBatch(answers, HttpContext.Request.UserHostAddress, Current_learn_userId);
                await SurveyEntityBiz.Instance.AddAnswer(answers.First().SurveyEntityId);
                return Json(true, JsonRequestBehavior.AllowGet);
                // return RedirectToAction("Index", "SurveyAnswers");
            }
            catch (Exception e)
            {
                // SetLog(e);
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public async Task<ActionResult> AnswerCheckIsRequir(List<SurveyAnswer> answers)
        {
            var result = new List<SurveyUserAnswerVM>();
            if (answers?.Count <= 0)
            {
                return Json(new { status = false, result = result }, JsonRequestBehavior.AllowGet);
            }
            try
            {

                result= await SurveyAnswerBiz.Instance.CheckRequiredQuestion(answers);

                return Json(new { status = true, result = result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                // SetLog(e);
                return Json(new { status = false, result = result }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult AllActiveAzmoon(AzmoonEntityType? id)
        {
            ViewBag.azmoonEntityType = id;
            return View(id);
        }
        public ActionResult AllActiveAzmoonPrivate(AzmoonEntityType? id)
        {
            return View(Biz.AzmoonEntityBiz.Instance.GetAllActivePrivateByUserId(id, Current_learn_userId));
        }
        public ActionResult Azmoon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                AzmoonEntity obl =  AzmoonEntityBiz.Instance.GetIncludeQuestion(id.Value, HttpContext.Request.UserHostAddress, Current_learn_userId);
                if (obl.Status == false)
                {

                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "آزمون غیر فعال است !");
                  
                    SetViewBagErrorMessage("آزمون غیر فعال است !");
                    //return Json(new { error = "آزمون غیر فعال است !" }, JsonRequestBehavior.AllowGet);
                    return View("index");
                }
                var now = DateTime.Now;
                if (obl.FromDate!=null && obl.FromDate > now)
                {
                    SetViewBagErrorMessage("تاریخ و زمان شروع آزمون فرا نرسیده است  !");
                    return View("index");
                }
                if (obl.ToDate != null && obl.ToDate < now)
                {

                    SetViewBagErrorMessage("تاریخ و زمان آزمون پایان یافته است  !");
                    return View("index");
                }
                if (obl.IsUserMustBeLogin)
                {
                    if (Current_learn_userId <= 0)
                    {
                        return RedirectToAction("login", "Users", new { ReturnUrl="/Home/Azmoon?id="+id });
                    }
                }
                    if (obl == null)
                {
                    return HttpNotFound();
                }
                    obl.UserIdCurrent= Current_learn_userId;
                return View(obl);
            }
            catch (ExceptionHandel.LoginReqException)
            {
                return Redirect("/Users/RegisterOrLogin?ReturnUrl=" + HttpContext.Request.RawUrl);
            }

        }

        [HttpPost]
        public async Task<ActionResult> AzmoonAnswer(List<AzmoonAnswer> answers, System.Web.HttpPostedFileBase File)
        {
            if (answers?.Count <= 0)
            {
                //SetLog(e: new Exception("no answer was found"));
                return Json(new ApiJsonResult { success = false, ErrorMessage = "no answer was found" }, JsonRequestBehavior.AllowGet);
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
                string res = await AzmoonAnswerBiz.Instance.SaveBatch(answers, HttpContext.Request.UserHostAddress, Current_learn_userId);
                await AzmoonEntityBiz.Instance.AddAnswer(answers.First().AzmoonEntityId);
                return Json(new ApiJsonResult { success = true, Message = res }, JsonRequestBehavior.AllowGet);
                // return RedirectToAction("Index", "AzmoonAnswers");
            }
            catch (Exception e)
            {
                // SetLog(e);
                return Json(new ApiJsonResult { success = false, ErrorMessage = e.Message, InnerExceptionMessage = ExceptionExtensions.GetStackTraceWithMessage(e) }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> ObjCount(ObjCount model)
        {
            try
            {
                var res = await ObjCountBiz.Instance.SaveAsync(model);
                if (res)
                    return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = "NOK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}