using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class SiteSettingController : BaseAdminController
    {
        private SWEntities db = new SWEntities();
        public ActionResult Index()
        {
            return View(db.SiteSetting.FirstOrDefault()??new SiteSetting.SiteSetting());
        }
        [HttpPost]
        public ActionResult Index(SiteSetting.SiteSetting siteSetting)
        {
            if (ModelState.IsValid)
            {
                if (siteSetting.Id==0)
                {
                    db.SiteSetting.Add(siteSetting);
                }
                else
                {
                    db.Entry(siteSetting).State = EntityState.Modified;
                }
               
                db.SaveChanges();
                SiteSetting.GetSetting.Instance.Set(siteSetting);
                TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد";
            }
            return View(siteSetting);
        }
    }
}