using SenakLearn.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class OrgBiz : RepositoryBase<Models.Organization>
    {
        public static readonly OrgBiz Instance = new OrgBiz();
        public List<SelectListItem> DropDown(bool isAll=false)
        {
            using (var ctx = new SWEntities())
            {
                var list = ctx.Orgs.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title }).ToList();
                if (isAll)
                    list.Insert(0, new SelectListItem { Text = "..." });
                return list;
            }
        }
    }
}