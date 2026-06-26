using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web;
using System.Linq;
using System.Threading;
using System.IO;

namespace SenakLearn
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected Timer _timer;
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.EnsureInitialized();
            HttpContext.Current.Application["TotalOnlineUsers"] = 0;
            using (SWEntities db = new SWEntities())
            {
                SiteSetting.GetSetting.Instance.Set(db.SiteSetting.FirstOrDefault() ?? new SiteSetting.SiteSetting());
            }
            var dueTime = DateTime.Today.AddDays(1.0) - DateTime.Now;
            _timer = new Timer(DoWork, null, dueTime, TimeSpan.FromHours(24));
            DoWork(null);
        }
        private void DoWork(object sender)
        {
            SetLog(" CalculateClassTypeForJob is runnig");
            Biz.OnlineClassBiz.Instance.CalculateClassTypeForJob();
            SetLog(" CalculateClassTypeForJob is complete");
        }
        private void SetLog(string log)
        {
            try
            {
                string fileName = Server.MapPath("/LogException.log");
                using (var streamWriter = new StreamWriter(fileName, true, System.Text.Encoding.Unicode))
                {
                    streamWriter.WriteLine(System.DateTime.Now.ToString() + log);
                    // streamWriter.WriteLine("---------------------------------------------------");
                }

            }
            catch (Exception)
            {//
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }
        protected void Session_Start()
        {
            HttpContext.Current.Application["TotalOnlineUsers"] = ((int)HttpContext.Current.Application["TotalOnlineUsers"] + 1);
            Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Site).GetAwaiter();
        }

        protected void Session_End()
        {
            try
            {
                var oldValue = HttpContext.Current!=null ?(int)HttpContext.Current?.Application["TotalOnlineUsers"]:0;
                if (oldValue > 0)
                {
                    HttpContext.Current.Application["TotalOnlineUsers"] = (oldValue - 1);
                }
            }
            catch (Exception e)
            {
                SetLog("Session_End: (int)HttpContext.Current.Application[TotalOnlineUsers]" + e.Message);
                // HttpContext.Current.Application["TotalOnlineUsers"] = 0;
            }

        }

    }
}
