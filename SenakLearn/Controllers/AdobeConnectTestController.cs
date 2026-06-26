using AdobeConnectSDK.Model;
using AdobeConnectService;
using AdobeConnectService.AdobeConnect.Model;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class AdobeConnectTestController : BaseController
    {
        ClassUsingSdk adob = null;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                GetSessionUser();
                adob = new ClassUsingSdk(Current_learn_user.Email, Current_learn_user.PassAdobe, Current_learn_user.BREEZESESSION);
                Session["BREEZESESSION"] = adob.BREEZESESSION;
            }
            catch (System.Exception)
            {
            }
           
            base.OnActionExecuting(filterContext);
        }
        public async Task<ActionResult> GoToAdobe(string url, long scoId, int classId)
        {
            var admin = ClassUsingSdkAdmin.Instance.GetAdminAdobe();
            var permission = PermissionId.Host;
            if (adob != null && adob.IsLogin && adob.Api != null)
            {
                //var detail = adob.GetMeetingDetail(scoId);
                //if (detail.DateClosed!=null)
                //{
                //    var x = 0;
                //}

                if (Current_learn_user.RoleId ==Models.Roles.Admin || Current_learn_user.RoleId == Models.Roles.SuperAdmin)//admin
                {
                }
                else if (Current_learn_user.RoleId ==Models.Roles.Teacher && Biz.OnlineClassBiz.Instance.ExistByTeacher(classId, Current_learn_userId))//teacher
                {
                }
                else if (Biz.OnlineClassBiz.Instance.ExistByUser(classId, Current_learn_userId))//user
                {
                    permission = PermissionId.View;
                }
                else
                {
                    TempData["ErrorMessage"] = "شما در این کلاس ثبت نام نکرده اید";
                    return Redirect("/");
                }
                try
                {
                    admin.PermissionsUpdate(new PermaissionFilter() { AclId = scoId, PrincipalId = adob.GetCurrentUserInfoViewModel().UserIdVm }, permission);
                }
                catch (System.Exception)
                {

                }
                await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Adobe);
                return Redirect(url + "?session=" + adob.BREEZESESSION + "&proto=true");
            }
            else
            {
                try
                {
                    var Principal = admin.UserCreate(new PrincipalSetupViewModel() { FirstName = string.IsNullOrEmpty(Current_learn_user.Name) ? Current_learn_user.user_name : Current_learn_user.Name, LastName = string.IsNullOrEmpty(Current_learn_user.Family) ? Current_learn_user.user_name : Current_learn_user.Family, Email = Current_learn_user.Email, Password = string.IsNullOrEmpty(Current_learn_user.PassAdobe) ? "123456" : Current_learn_user.PassAdobe, Description = Current_learn_user.Mobile + " " + Current_learn_user.NationaCode + " " + Current_learn_user.Address });
                    var PrincipalId = long.Parse(Principal.PrincipalId);
                    if (Current_learn_user.RoleId ==Models.Roles.Admin || Current_learn_user.RoleId == Models.Roles.SuperAdmin)//admin
                    {
                        admin.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.AdministratorsOfGroupMembership, PrincipalId); //add admin to Administrators group
                                                                         // admin.GroupMembershipUpdate(21021, PrincipalId); //add admin to Administrators- Limited group
                        admin.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.HostsOfGroupMembership, PrincipalId); //add admin to Meeting Hosts group
                    }
                    else if (Current_learn_user.RoleId == Models.Roles.Teacher && Biz.OnlineClassBiz.Instance.ExistByTeacher(classId, Current_learn_userId))//teacher
                    {
                        ////  teacher.PrincipalId = long.Parse(Principal.PrincipalId);
                        //  db.SaveChanges();
                        admin.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.TrainingManagersOfGroupMembership, PrincipalId); //add teacher to Training Managers group
                    }
                    else if (Biz.OnlineClassBiz.Instance.ExistByUser(classId, Current_learn_userId))//user
                    {
                        admin.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.LearnersOfGroupMembership, PrincipalId);//add user to Learners Group
                        permission = PermissionId.View;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "شما در این کلاس ثبت نام نکرده اید";
                        return Redirect("/");
                    }
                    admin.PermissionsUpdate(new PermaissionFilter() { AclId = scoId, PrincipalId = adob.GetCurrentUserInfoViewModel().UserIdVm }, permission);
                    adob = new ClassUsingSdk(Current_learn_user.Email, Current_learn_user.PassAdobe, Current_learn_user.BREEZESESSION);
                    Session["BREEZESESSION"] = adob.BREEZESESSION;
                    await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Adobe);
                    return Redirect(url + "?session=" + adob.BREEZESESSION + "&proto=true");
                }
                catch (System.Exception e)
                {
                    
                    TempData["ErrorMessage"] = e.Message;
                    return Redirect("/");
                }
            }
           // TempData["ErrorMessage"] = "نام کاربری شما در سامانه ادوبی تعریف نشده است";
           // return Redirect("/");
        }
        // GET: AdobeConnectTest
        public ActionResult Index()
        {
            if (adob.IsLogin && adob.Api != null)
                return View(adob.GetCurrentUserInfoViewModel());
            return View();
        }
        public ActionResult GetAllMeetings()
        {
            if (adob.IsLogin && adob.Api != null)
                return View(adob.GetAllMeetings());
            return View();
        }
        public ActionResult GetMyMeetings()
        {
            if (adob.IsLogin && adob.Api != null)
                return View("GetAllMeetings", adob.GetMyMeetings());
            return View("GetAllMeetings");
        }
        public ActionResult GetPrincipalList()
        {
            if (adob.IsLogin && adob.Api != null)
                return View("GetPrincipalList", adob.GetPrincipalList(new PrincipalFilter() { }));
            return View("GetPrincipalList");
        }
        public ActionResult GetSCOshotcuts()
        {
            if (adob.IsLogin && adob.Api != null)
                return View("GetSCOshotcuts", adob.GetSCOshotcuts());
            return View("GetSCOshotcuts");
        }
        public ActionResult GetMeetingShotcuts()
        {
            if (adob.IsLogin && adob.Api != null)
                return View("GetSCOshotcuts", adob.GetSCOshotcuts(true));
            return View("GetSCOshotcuts");
        }
        public ActionResult ReportMyEvents()
        {
            if (adob.IsLogin && adob.Api != null)
                return View("ReportMyEvents", adob.ReportMyEvents());
            return View("ReportMyEvents");
        }
    }
}