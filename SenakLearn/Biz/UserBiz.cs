using AdobeConnectService;
using DocumentFormat.OpenXml.Wordprocessing;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using SenakLearn.Models.Security;
using SenakLearn.Models.wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Roles = SenakLearn.Models.Roles;

namespace SenakLearn.Biz
{
    public class UserBiz
    {
        public static readonly UserBiz Instance = new UserBiz();

        public int AllUserCount()
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_user.Count();
            }
        }

        public JqGrid.PagedList<UserWrapper> GetAllPagedList(GridSettings grid)
        {
            using (SWEntities db = new SWEntities())
            {
                var query = (from i in db.learn_user
                             join o in db.Orgs on i.OrgId equals o.Id into org
                             from o in org.DefaultIfEmpty()
                             select new UserWrapper
                             {
                                 id = i.id,
                                 Mobile = i.Mobile,
                                 NationaCode = i.NationaCode,
                                 PersonCode = i.PersonCode,
                                 Name = i.Name,
                                 Family = i.Family,
                                 TypeUserName = i.TypeUser == 2 ? "سازمانی" : "شخصی",
                                 TypeUser = i.TypeUser,
                                 user_name = i.user_name,
                                 OrgName = o.Title,
                                 OrgId = o.Id,
                                 date_register = i.date_register,
                                 status = i.status,
                                 RoleId = i.RoleId,
                                 PostId = i.PostId,
                                 //BREEZESESSION=i.BREEZESESSION,
                                 PassAdobe = i.PassAdobe,
                                 //date_register_Shamsi=i.date_register_Shamsi
                             }
    );
                return query.FilterAndSortJqGrid(grid).ToPagedList(grid);
            }

        }
        public JqGrid.PagedList<UserSearchWrapper> GetAllPagedSearchList(GridSettings grid, UserSearchWrapper search)
        {
            using (SWEntities db = new SWEntities())
            {
                var query = (from i in db.learn_user
                             join o in db.Orgs on i.OrgId equals o.Id into org
                             from o in org.DefaultIfEmpty()
                             select new UserSearchWrapper
                             {
                                 UserId = i.id,
                                 Mobile = i.Mobile,
                                 NationalCode = i.NationaCode,
                                 PersonCode = i.PersonCode,
                                 PersonNameSearch = i.Name + " " + i.Family,
                                 TypeUserId = i.TypeUser,
                                 UserName = i.user_name,
                                 PersonOrg = o.Title,
                                 PersonOrgId = o.Id
                             }
                    );

                if (!string.IsNullOrEmpty(search.UserName))
                {
                    query = query.Where(w => w.UserName.Contains(search.UserName));
                }
                if (!string.IsNullOrEmpty(search.PersonNameSearch))
                {
                    query = query.Where(w => w.PersonNameSearch.Contains(search.PersonNameSearch));
                }
                if (!string.IsNullOrEmpty(search.NationalCode))
                {
                    query = query.Where(w => w.NationalCode.Contains(search.NationalCode));
                }
                if (!string.IsNullOrEmpty(search.PersonCode))
                {
                    query = query.Where(w => w.PersonCode.Contains(search.PersonCode));
                }
                if (!string.IsNullOrEmpty(search.PersonOrg))
                {
                    query = query.Where(w => w.PersonOrg.Contains(search.PersonOrg));
                }
                if (!string.IsNullOrEmpty(search.Mobile))
                {
                    query = query.Where(w => w.Mobile.Contains(search.Mobile));
                }
                if (search.TypeUserId != null)
                {
                    query = query.Where(w => w.TypeUserId == search.TypeUserId);
                }
                var result = query.FilterAndSortJqGrid(grid).ToPagedList(grid);
                return result;
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public Models.learn_user Find(int id)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_user.FirstOrDefault(x => x.id == id);
            }
        }
        public void ResetPass(int id, string pass)
        {
            using (SWEntities db = new SWEntities())
            {
                var user = db.learn_user.FirstOrDefault(x => x.id == id);
                if (user == null)
                {
                    throw new Exception("کاربر نامعتبر است");
                }
                user.password = HasherPass(pass);
                db.SaveChanges();
            }
        }

        public learn_user SetTeacherAccess(int id)
        {
            using (SWEntities db = new SWEntities())
            {
                var user = db.learn_user.FirstOrDefault(x => x.id == id);
                user.RoleId = Models.Roles.Teacher;
                db.SaveChanges();
                return user;
            }
        }

        public Models.learn_user FindByUserAndPass(string username, string password)
        {
            password = HasherPass(password);
            using (SWEntities db = new SWEntities())
            {
                var user = db.learn_user.FirstOrDefault(x => x.user_name == username && x.password == password);
                if (user == null)
                {
                    return user;
                }
                if (!IsValidEmail(user.Email))
                {
                    user.Email = "Test" + DateTime.Now.Ticks + "@gmail.com";
                    db.SaveChanges();
                }
                return user;
            }
        }
        public Models.learn_user FindByUserName(string username)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_user.FirstOrDefault(x => x.user_name == username);
            }
        }
        public Models.learn_user FindByUserId(int userId)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_user.FirstOrDefault(x => x.id == userId);
            }
        }
        public Models.learn_user ChangePass(int id, string oldpassword, string newPass)
        {
            oldpassword = HasherPass(oldpassword);
            if (oldpassword == newPass)
            {
                throw new Exception("پسورد جاری با پسورد جدید برابر است");
            }
            using (SWEntities db = new SWEntities())
            {
                var user = db.learn_user.FirstOrDefault(x => x.id == id && x.password == oldpassword);
                if (user == null)
                {
                    throw new Exception("پسورد وارد شده صحیح نمی باشد");
                }
                user.password = HasherPass(newPass);
                db.SaveChanges();
                return user;
            }
        }
        public Models.learn_user ChangePassAdobi(int id, string oldpassword, string newPass)
        {
            if (oldpassword == newPass)
            {
                throw new Exception("پسورد جاری با پسورد جدید برابر است");
            }
            using (SWEntities db = new SWEntities())
            {
                var user = db.learn_user.FirstOrDefault(x => x.id == id && x.PassAdobe == oldpassword);
                if (user == null)
                {
                    throw new Exception("پسورد وارد شده صحیح نمی باشد");
                }
                var adobe = new ClassUsingSdk(user.Email, oldpassword);
                if (adobe.IsLogin && adobe.Api != null && adobe.ChangePassword(newPass))
                {
                    user.PassAdobe = newPass;
                    db.SaveChanges();
                    return user;
                }
                throw new Exception("نام کاربری برای شما در سامانه ادوبی تعریف نشده است");
            }
        }
        public learn_user ResetPass(string email, string newPass)
        {
            using (SWEntities db = new SWEntities())
            {
                var user = db.learn_user.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    throw new Exception("کاربر یافت نشد");
                }
                user.password = HasherPass(newPass);
                db.SaveChanges();
                return user;
            }
        }
        public async Task SendToAdminAsync(learn_user user, bool isRegisterToClass = false, bool isRegisterToClassByPayment = false)
        {
            var title = "اطلاع رسانی ثبت نام کاربر جدید در سایت";
            if (isRegisterToClass)
            {
                title = "اطلاع رسانی ثبت نام کاربر در کلاس";
            }
            if (isRegisterToClassByPayment)
            {
                title = "اطلاع رسانی پرداخت وجه کاربر برای ثبت نام در کلاس";
            }
            using (SWEntities db = new SWEntities())
            {
                List<string> adminEmails = await db.learn_user.Where(x => x.RoleId == Models.Roles.Admin || x.RoleId == Models.Roles.SuperAdmin).Select(x => x.Email).ToListAsync();
                foreach (var item in adminEmails)
                {
                    SenakLearn.SendEmail.AlertForClass(item, title, "<div>نام کاربری: " + user.user_name + "</div><div>نام : " + user.Name + " " + user.Family + "</div><div> ایمیل: " + user.Email + "</div><div> موبایل: " + user.Mobile + "</div>", SiteSetting.GetSetting.Instance.Get());
                }
            }
        }
        public Models.learn_user RegisterUser(learn_user user)
        {
            if (user.OrgId == 0)
                user.OrgId = null;
            if (user.PostId == 0)
                user.PostId = null;
            user.date_register = DateTime.Now;
            user.Validate();
            user.user_name = user.user_name.Trim();
            user.Email = user.Email.Trim();
            using (SWEntities db = new SWEntities())
            {
                var existuser_name = db.learn_user.Any(i => i.user_name == user.user_name);
                if (existuser_name)
                {
                    throw new Exception("نام کاربری با این عنوان قبلا ثبت شده است");
                }
                var existEmail = db.learn_user.Any(i => i.Email == user.Email);
                if (existEmail)
                {
                    throw new Exception("ایمیل با این عنوان قبلا ثبت شده است");
                }
                var usedNationalCode = db.learn_user.Where(w => w.NationaCode == user.NationaCode).Any();
                if (usedNationalCode != false)
                {
                    throw new Exception("کد ملی تکراری است !");
                }
                user.password = HasherPass(user.password);

                db.learn_user.Add(user);
                db.SaveChanges();
                return user;
            }
        }
        public Tuple<bool,string> UploadUser(HttpPostedFileBase file, int OrgId)
        {

            int fileSize = file.ContentLength;
            string fileName = file.FileName;
            string mimeType = file.ContentType;
            Stream fileContent = file.InputStream;
            MemoryStream target = new MemoryStream();
            fileContent.CopyTo(target);
            byte[] data = target.ToArray();
            var excelService = new ExcelService();
            var importDataList = excelService.ProcessExcelFile<UserImportExcelWrapper>(data).ToList();
            if (importDataList == null || importDataList.Count == 0)
                return new Tuple<bool, string>(false,"فایل اکسل نامعتبر است !");
            List<learn_user> users = new List<learn_user>();
            importDataList = importDataList.Where(w => w.CodeMeli != null).ToList();
            if (importDataList.Count == 0) return new Tuple<bool, string>(false, "فایل اکسل نامعتبر است !");
            using (SWEntities db = new SWEntities())
            {
                var existsNationalcodes = db.learn_user.Select(s => s.NationaCode).ToList();
                users = importDataList.Where(w => !existsNationalcodes.Contains(w.CodeMeli)).Select(a => new learn_user
                {
                    PersonCode = a.PersonCode,
                    user_name = a.CodeMeli,
                    Name = a.Name,
                    Family = a.Family,
                    password = string.IsNullOrEmpty(a.PersonCode) ? (string.IsNullOrEmpty(a.Mobile) ? HasherPass("1331") : HasherPass(a.Mobile.Substring(a.Mobile.Length - 4, 4))) : HasherPass(a.PersonCode),
                    Tel = a.Mobile,
                    Mobile = a.Mobile == null || a.Mobile == "" ? null : (a.Mobile.StartsWith("9") ? ("09" + a.Mobile.Substring(1)) : a.Mobile),
                    FatherName = a.Father,
                    NationaCode = a.CodeMeli,
                    Shenasname = a.Shenasname,
                    TypeUser = 2,
                    OrgId = OrgId,
                    status = true,
                    date_register = DateTime.Now,
                    RoleId = Models.Roles.User,
                    Email = "Fake@email.com",
                }).ToList();

                List<string> validations = new List<string>();
                users.ForEach(user =>
                {
                    var res = user.Validate(true);
                    if (res.Count > 0)
                        validations.AddRange(res.Select(a=>a.ErrorMessage).Distinct());
                });
                if (validations.Count > 0)
                {
                    var error = new Tuple<bool, string>(false, validations.Distinct().FirstOrDefault());
                    return error;
                }
                if (users.Count == 0)   return new Tuple<bool, string>(false, "فایل اکسل نامعتبر است !");
                
                db.learn_user.AddRange(users);
                db.SaveChanges();
            }
            return  new Tuple<bool, string>(true, ""); ;
        }
        public Models.learn_user UpdateUser(learn_user user)
        {
            using (SWEntities db = new SWEntities())
            {
                var existuser = db.learn_user.FirstOrDefault(i => i.id == user.id);
                if (existuser == null)
                {
                    throw new Exception("کاربر یافت نشد");
                }
                existuser.Name = user.Name;
                existuser.Address = user.Address;
                existuser.Family = user.Family;
                existuser.Mobile = user.Mobile;
                existuser.NationaCode = user.NationaCode;
                existuser.ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? existuser.ImageUrl : user.ImageUrl;
                existuser.Validate();
                db.SaveChanges();
                return existuser;
            }
        }
        public Models.learn_user UpdateUser(RegisterViewModel user)
        {
            using (SWEntities db = new SWEntities())
            {
                var existuser = db.learn_user.FirstOrDefault(i => i.id == user.Id);
                if (existuser == null)
                {
                    throw new Exception("کاربر یافت نشد");
                }
                var usedNationalCode = db.learn_user.Where(w => w.NationaCode == user.NationaCode);
                if(usedNationalCode!=null)
                {
                    throw new Exception("کد ملی تکراری است !");
                }
                existuser.Province = user.Province;
                existuser.City = user.City;
                existuser.Education = user.Education;
                existuser.Expertise = user.Expertise;
                existuser.BirthLocation = user.BirthLocation;
                existuser.BirthDay = user.BirthDay;
                existuser.Tel = user.Tel;
                existuser.Shenasname = user.Shenasname;
                existuser.FatherName = user.FatherName;
                existuser.Address = user.Address;
                existuser.Family = user.Family;
                existuser.Name = user.Name;
                existuser.Mobile = user.Mobile;
                existuser.NationaCode = user.NationaCode;
                existuser.ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? existuser.ImageUrl : user.ImageUrl;
                existuser.Validate();
                db.SaveChanges();
                return existuser;
            }
        }
        private string HasherPass(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("کلمه عبور راوارد نمایید");
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password?.Trim(), "MD5");
        }
        public string GenerateToken(learn_user user)
        {
            var payload = new Dictionary<string, object>
{
    { "id", user.id },
    { "user_name", user.user_name },
    { "Email", user.Email },
    { "RoleId", user.RoleId },
    { "PassAdobe", user.PassAdobe },
    { "Mobile", user.Mobile },
    { "Name", user.Name },
    { "Family", user.Family },
    { "NationaCode", user.NationaCode },
    { "Address", user.Address },
    { "ImageUrl", user.ImageUrl },
};
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, SiteSetting.GetSetting.Instance.Get().JwtTokenSecretKey);
            return token;
        }
        public learn_user GetUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token?.Trim()))
            {
                throw new Exception("توکن معتبر نیست");
            }
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, SiteSetting.GetSetting.Instance.Get().JwtTokenSecretKey, verify: true);
                return JsonConvert.DeserializeObject<learn_user>(json);
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Token has invalid signature");
            }
        }
        public List<SelectListItem> DropDown(bool isAdmin)
        {
            using (var ctx = new SWEntities())
                return ctx.learn_user.Where(x => !isAdmin || x.RoleId == Models.Roles.Admin || x.RoleId == Models.Roles.SuperAdmin).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.user_name }).ToList();
        }
        public List<SelectListItem> DropDownUsers()
        {
            var user = Roles.User;
            using (var ctx = new SWEntities())
            {
                var list = ctx.learn_user.Where(x => x.RoleId == user && x.user_name != null && x.user_name != string.Empty)
                    .Select(i => new SelectListItem()
                    {
                        Value = i.id.ToString(),
                        Text = i.user_name + (i.Name != null ? (" " + i.Name) : string.Empty) + (i.Family != null ? (" " + i.Family) : string.Empty),
                    }).ToList();
                list.Insert(0, new SelectListItem { Text = "..." });
                return list;
            }
        }



        public async Task<learn_user> UpdateAdmin(learn_user model)
        {
            using (var ctx = new SWEntities())
            {
                if (model.id == 0)
                {
                    throw new Exception(" کاربر نامعتبر");
                }
                var user = ctx.learn_user.FirstOrDefault(x => x.id == model.id);
                if (user == null)
                {
                    throw new Exception(" کاربر نامعتبر");
                }
                user.RoleId = model.RoleId;
                user.status = model.status;
                user.password = model.password;
                await ctx.SaveChangesAsync();
                return user;
            }
        }

        public JqGrid.PagedList<RoleUserVm> GetAllPagedListRoleByUserId(GridSettings grid, int id)
        {
            using (var ctx = new SWEntities())
                return ctx.RoleUser.Where(x => x.UserId == id).Select(x => new RoleUserVm { Id = x.Id, RoleName = x.Role.Name }).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public async Task SaveRoleUser(RoleUser user)
        {
            using (var ctx = new SWEntities())
            {
                if (ctx.RoleUser.Any(x => x.RoleId == user.RoleId && x.UserId == user.UserId))
                {

                }
                else
                {
                    ctx.RoleUser.Add(user);
                    await ctx.SaveChangesAsync();
                }
            }

        }

        public bool IsAccess(Permisstion permission, learn_user userId)
        {
            if (userId.RoleId == Models.Roles.SuperAdmin)
                return true;
            using (var ctx = new SWEntities())
            {
                var query = (
                    from u in ctx.RoleUser.Where(x => x.UserId == userId.id)
                    join p in ctx.RolePermission on u.RoleId equals p.RoleId //into pppp
                    where p.Permisstion == permission
                    select p
                           );
                return query.Count() > 0;
            }

        }
        public List<Permisstion> GetPermisstionsByRoleId(int roleId)
        {
            using (var ctx = new SWEntities())
                return ctx.RolePermission.Where(x => x.RoleId == roleId).Select(x => x.Permisstion).ToList();
        }
        public List<Permisstion> GetPermisstionsByUserId(learn_user userId)
        {
            if (userId.RoleId == Models.Roles.SuperAdmin)
            {
                var res = new List<Permisstion>();
                foreach (Permisstion item in Enum.GetValues(typeof(Permisstion)))
                {
                    res.Add(item);
                }
                return res;
            }
            using (var ctx = new SWEntities())
            {
                var query = (
                    from u in ctx.RoleUser.Where(x => x.UserId == userId.id)
                    join p in ctx.RolePermission on u.RoleId equals p.RoleId //into pppp
                    select p.Permisstion
                           );
                return query.ToList();
            }
        }

        public async Task RemoveRoleUser(int id)
        {
            using (var ctx = new SWEntities())
            {
                var result = ctx.RoleUser.Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    throw new System.Exception("رکورد یافت نشد");
                }
                ctx.RoleUser.Remove(result);
                await ctx.SaveChangesAsync();
            }
        }

    }
}