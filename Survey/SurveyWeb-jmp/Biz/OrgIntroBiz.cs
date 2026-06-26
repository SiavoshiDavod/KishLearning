using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Biz
{
    public class OrgIntroBiz : RepositoryBase<Models.OrgIntro>
    {
        public static readonly OrgIntroBiz Instance = new OrgIntroBiz();
        public List<OrgIntro> GetAllSync()
        {
            using (var db = new Context())
            {
                return db.OrgIntro.ToList();
            }
        }
    }
}