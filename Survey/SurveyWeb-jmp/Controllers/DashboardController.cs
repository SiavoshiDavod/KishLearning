using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class DashboardController: BaseAdminController
    {
        public ActionResult Index()
        {
            return View("Dashboard");
        }
    }
}