using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class JobBoardController : BaseProfileController
    {
        public ActionResult Index()
        {
            
            return View("~/Views/JobBoard/Home/Index.cshtml");
        }
    }
}