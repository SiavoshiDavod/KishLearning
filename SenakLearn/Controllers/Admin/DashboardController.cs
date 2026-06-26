using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class DashboardController : BaseAdminController
    {
        public ActionResult Index()
        {
            if (Current_learn_user.RoleId == Models.Roles.Teacher)
            {
                var teacher = Biz.TeacherBiz.Instance.FindByUserId(Current_learn_userId);
                ViewBag.Teacher = teacher;
            }
            return View("Dashboard");
        }
        private static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }
        public ActionResult Capacity()
        {
            long totalFreeSpace = 0, totalSize = 0;
            //foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                try
                {
                    //totalFreeSpace += drive.TotalFreeSpace;
                    //totalSize += drive.TotalSize;
                    var size = DirSize(new DirectoryInfo(Server.MapPath("/")));
                    totalSize += 10737418240;//total size of esfahanHost plesk for kishkearning
                    totalFreeSpace = totalSize- size;
                }
                catch (System.Exception)
                {

                }

            }
            return Json(new
            {
                totalFreeSpace,
                totalSize
            },
          JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnlineUserCount()
        {
            string count = HttpContext.Application["TotalOnlineUsers"].ToString();
            return Json(new
            {
                count
            },
          JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> MultiLineSite()
        {
            return Json(await Biz.SiteReviewCountBiz.Instanse.GetAllAsync(),
          JsonRequestBehavior.AllowGet);
        }
    }
}