using System;
using System.Web.Mvc;
using System.Web.Security;
using SenakLearn.Models;
using SenakLearn.Biz;
using System.Web;
using System.Threading.Tasks;

namespace SenakLearn.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult TeacherLogin(int id)
        {
            var user = Current_learn_user;
            var obj = Biz.JoinUsBiz.Instance.Get(id);
            if (obj == null)
            {
                throw new Exception("خطا. فرم همکاری شما با ما یافت نشد");
            }
            if (Current_learn_userId <= 0)
            {
                if (obj.UserId != null)
                {
                    user = Biz.UserBiz.Instance.Find(obj.UserId.Value);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(obj.NationalCode, false);
                        SetSessionUser(user);
                        ViewBag.User = user;
                    }
                    else
                    {
                        throw new Exception("خطا. لطفا ابتدا در سامانه لاگین کرده و سپس لینک را کلیک کنید");
                    }
                }
                else
                {
                    throw new Exception("خطا. لطفا ابتدا در سامانه لاگین کرده و سپس لینک را کلیک کنید");
                }

            }
            else// if (Current_learn_userId > 0)
            {
                if (obj.UserId == null)
                {
                    var res = Biz.JoinUsBiz.Instance.SetUserId(id, user.id);
                    if (!res)
                    {
                        throw new Exception("خطا. لطفا با پشتیبنی تماس بگیرید");
                    }
                }
                else if(obj.UserId.Value!= user.id)
                {
                    throw new Exception("خطا. شما با کاربری لاگین کرده اید که فرم همکاری با ما را ارسال نکرده است. لطفا از این حساب خارج شده و مجددا تلاش کنید");
                }
            }

            if (user.RoleId == Models.Roles.Admin|| user.RoleId == Models.Roles.SuperAdmin)
            {
                throw new Exception("کاربران ادمین امکان تعریف به عنوان استادجدید را ندارند");
            }
            ViewBag.JoinUsid = id;

            if (obj.TeacherId == null)
            {
                return View(new learn_teacher() { meli = obj.NationalCode, name = obj.Name, family = obj.Family, email = obj.Email, mobile = obj.Mobile });
            }

            var teacher = Biz.TeacherBiz.Instance.FindById(obj.TeacherId.Value);
            ViewBag.Teacher = teacher;
            if (teacher.status)
            {
                return View("Dashboard");
            }
            else
            {
                // upload step
                //or agree step
            }

            throw new Exception("خطا. لطفا با پشتیبنی تماس بگیرید");
        }

        [Authorize]
        [HttpPost]
        public ActionResult TeacherLogin(learn_teacher teacher, int JoinUsid)
        {
            teacher.status = false;
            teacher.date_register = DateTime.Now;
            teacher.UserId = Current_learn_userId;
            teacher.code = JoinUsid.ToString();
            var t = Biz.TeacherBiz.Instance.Create(teacher);
            ViewBag.Teacher = t;
            var obj = Biz.JoinUsBiz.Instance.SetTeacherId(JoinUsid, t.id);
            if (obj)
            {
                var user = Biz.UserBiz.Instance.SetTeacherAccess(Current_learn_userId);
                SetSessionUser(user);
                ViewBag.User = user;
                TempData["SuccessMessage"] = "اطلاعات شما با موفقیت ثبت شد. لطفا قرارداد را مطالعه کرده و ویدیوهای آموزشی خود را آپلود کنید";
                return View("Dashboard");
            }
            throw new Exception("خطا. لطفا با پشتیبنی تماس بگیرید");
        }

        public void SetSessionLogout()
        {
            Session.Abandon();
            Session.Clear();
        }
        public ActionResult SendAlertEmail()
        {
            var setting = SiteSetting.GetSetting.Instance.Get();
            Biz.EmaiSmslBiz.Instance.AlertForClass(Current_learn_user.Email, setting.AdobeServerUrl, Current_learn_user.NameForEmail + " سلام", SiteSetting.GetSetting.Instance.Get());
            return new ContentResult();
        }

        //readonly SWEntities _db = new SWEntities();
        // GET: Users
        public ActionResult RegisterOrLogin(string ReturnUrl)
        {
            if (ReturnUrl != null)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                ModelState.AddModelError(string.Empty, "کاربر محترم لطفا ابتدا در سایت ثبت نام کرده یا وارد حساب کاربری خود شوید");
            }
            return View();
        }
        public ActionResult Register(string ReturnUrl)
        {
            if (ReturnUrl != null)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                ModelState.AddModelError(string.Empty, "کاربر محترم لطفا ابتدا در سایت ثبت نام کنید");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel register, string ReturnUrl = "/")
        {
            if (register == null) return View();
            CheckGoogleRecapcha(register.googlerecaptchaRegister);
            try
            {
                //var person = _db.Person.SingleOrDefault(i => i.meli == register.CodeMeli);
                //if (person == null)
                //{
                //    ModelState.AddModelError("CodeMeli", "کد ملی وارد شده در سامانه ثبت نشده است");
                //    return View();
                //}
                var Register = new learn_user()
                {
                    //id_person = person.id,
                    user_name = register.UserName,
                    password = register.Password,
                    Name = register.Name,
                    Family = register.Family,
                    NationaCode = register.NationaCode,
                    RoleId = Models.Roles.User,
                    //date_register = DateTime.Now,
                    //status = true,
                    Email = register.Email,
                    Mobile = register.Mobile,
                };
                Register = Biz.UserBiz.Instance.RegisterUser(Register);

                FormsAuthentication.SetAuthCookie(Register.user_name, true);
                SetSessionUser(Register);
                TempData["SuccessMessage"] = "شما با موفقیت در سایت ثبت نام کردید";
                await Biz.UserBiz.Instance.SendToAdminAsync(Register);
                return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/Dashboard" : ReturnUrl);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View(register);
        }
        public ActionResult ResetPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPass(string email, string googlerecaptchaResetPass)
        {
            try
            {
                CheckGoogleRecapcha(googlerecaptchaResetPass);

                var newPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
                UserBiz.Instance.ResetPass(email, newPass);
                Biz.EmaiSmslBiz.Instance.AlertForClass(email, "بازیابی کلمه عبور", "<div  dir='rtl' style='text-align: right;' >کلمه عبور شما بازیابی شد<br>" + newPass + "</div>", SiteSetting.GetSetting.Instance.Get());
                TempData["SuccessMessage"] = "کلمه عبور شما بازیابی شد و به ایمیل شما ارسال گردید";
                return Redirect("/");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return View();
        }
        [Authorize]
        public ActionResult ShowProfile()
        {
            return View(GetSessionUser());
        }
        [Authorize]
        public ActionResult EditeProfile()
        {
            var register = GetSessionUser();
            return View(new Models.RegisterViewModel()
            {
                UserName = register.user_name,
                PassAdobe = register.PassAdobe,
                Name = register.Name,
                Family = register.Family,
                NationaCode = register.NationaCode,
                Email = register.Email,
                Mobile = register.Mobile,
                Address = register.Address,
                ImageUrl = register.ImageUrl,
                Province = register.Province ?? Province.Tehran,
                City = register.City,
                Education = register.Education,
                Expertise = register.Expertise,
                BirthLocation = register.BirthLocation,
                BirthDay = register.BirthDay,
                Tel = register.Tel,
                Shenasname = register.Shenasname,
                FatherName = register.FatherName,
            });
        }
        [HttpPost]
        [Authorize]
        public ActionResult EditeProfile(RegisterViewModel register, HttpPostedFileBase ImageFile)
        {
            try
            {
                register.ImageUrl = EditFile(ImageFile, pathFile.Users, GetSessionUser().ImageUrl);
                register.Id = Current_learn_userId;
                var updatedUser = UserBiz.Instance.UpdateUser(register);
                SetSessionUser(updatedUser);
                TempData["SuccessMessage"] = "ویرایش با موفقیت انجام شد";
                return Redirect("/Dashboard");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return View(register);

        }
        public ActionResult Login(string ReturnUrl)
        {
            if (ReturnUrl != null)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                ModelState.AddModelError("userName", "کاربر محترم بدلیل عدم دسترسی به صفحه مورد نظرتان به بخش ورود کاربران هدایت شده اید");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel login, string ReturnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                CheckGoogleRecapcha(login.googlerecaptchaLogin);

                var pass = login.Pass;
                var user = Biz.UserBiz.Instance.FindByUserAndPass(login.userNameLogin, pass);
                //var user = _db.learn_user.SingleOrDefault(i => i.user_name == login.userNameLogin && i.password == pass);
                if (user == null)
                {
                    ModelState.AddModelError("userNameLogin", "نام کاربری یا کلمه عبور معتبر نمی باشد");
                }
                else
                {
                    if (user.status)
                    {
                        FormsAuthentication.SetAuthCookie(login.userNameLogin, login.Remember);
                        SetSessionUser(user);
                        //TempData["SuccessMessage"]="شما با موفقیت وارد سایت شدید";
                        return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/Dashboard" : ReturnUrl);

                    }
                    else
                        ModelState.AddModelError("userName", "حساب کاربری شما غیرفعال است");
                }
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View(login);
        }
        [Authorize]
        public ActionResult CheangPass()
        {
            return View("CheangPass", new ChangePassViewModel());
        }
        [Authorize]
        public ActionResult CheangPassAdobe()
        {
            return View("CheangPass", new ChangePassViewModel() { IsAdobe = true });
        }
        [Authorize]
        [HttpPost]
        public ActionResult CheangPass(ChangePassViewModel change)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((int?)Session["id"]) ?? 0;
                    if (userId<=0)
                    {
                        userId = Current_learn_userId;
                    }
                    //var pass = FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPass, "MD5");
                    //var NewPass = FormsAuthentication.HashPasswordForStoringInConfigFile(change.Pass, "MD5");
                    learn_user user = null;
                    if (change.IsAdobe)
                    {
                        user = Biz.UserBiz.Instance.ChangePassAdobi(userId, change.OldPass, change.Pass);
                    }
                    else
                    {
                        user = Biz.UserBiz.Instance.ChangePass(userId, change.OldPass, change.Pass);
                    }
                    //var user =
                    //    _db.learn_user.SingleOrDefault(i => i.user_name == User.Identity.Name && i.password == pass);
                    //if (user != null)
                    //{

                    //    user.password = NewPass;
                    //    _db.SaveChanges();
                    ViewBag.isOk = true;
                    TempData["SuccessMessage"] = "پسورد شما با موفقیت تغییر یافت";
                    return Redirect("/Dashboard");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("OldPass", "پسورد وارد شده صحیح نمی باشد");
                    //}
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("OldPass", e.Message);
            }
            return View(change);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            SetSessionLogout();
            return Redirect("/");
        }

        //[Authorize]
        //[Route("Dashboard")]
        //public ActionResult Dashboard()
        //{
        //    if (Current_learn_user.RoleId==Models.Roles.Teacher)
        //    {
        //        var teacher = Biz.TeacherBiz.Instance.FindByUserId(Current_learn_userId);
        //        ViewBag.Teacher = teacher;
        //    }
        //    return View("Dashboard");
        //}
    }
}