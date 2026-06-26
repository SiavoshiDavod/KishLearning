using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SenakLearn.Controllers
{
    public class DetailsOnlineClassController : BaseController
    {
        private SWEntities db = new SWEntities();

        // GET: DetailsCours
        public async Task<ActionResult> Index(int id, int type = 2)
        {
            var learn_cours = db.OnlineClasses.Include("OnlineClassAccoration.Details").FirstOrDefault(x => x.Id == id );
            if (learn_cours == null)
            {
                return HttpNotFound();
            }
            await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Online);
            return PartialView(learn_cours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}