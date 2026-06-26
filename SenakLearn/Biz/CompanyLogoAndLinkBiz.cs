using SenakLearn.Models;
using System.Collections.Generic;
using System.Linq;

namespace SenakLearn.Biz
{
    public class CompanyLogoAndLinkBiz : RepositoryBase<Models.CompanyLogoAndLink>
    {
        public static readonly CompanyLogoAndLinkBiz Instance = new CompanyLogoAndLinkBiz();
        public List<CompanyLogoAndLink> GetAllSync()
        {
            using (var db = new SWEntities())
            {
                return db.CompanyLogoAndLinks.ToList();
            }
        }
    }
}