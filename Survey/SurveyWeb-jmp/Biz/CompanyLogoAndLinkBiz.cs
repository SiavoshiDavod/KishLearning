using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using SurveyWeb.Models;

namespace SurveyWeb.Biz
{
    public class CompanyLogoAndLinkBiz : RepositoryBase<Models.CompanyLogoAndLink>
    {
        public static readonly CompanyLogoAndLinkBiz Instance = new CompanyLogoAndLinkBiz();
        public List<CompanyLogoAndLink> GetAllSync()
        {
            using (var db = new Context())
            {
                return db.CompanyLogoAndLink.ToList();
            }
        }
    }
}