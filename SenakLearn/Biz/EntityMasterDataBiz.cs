using SenakLearn.Biz.Person;
using SenakLearn.Models.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class EntityMasterDataBiz : RepositoryBase<EntityMasterData>
    {
        public static readonly EntityMasterDataBiz Instance = new EntityMasterDataBiz();
        public List<SelectListItem> DropDown(int entityType)
        {
            using (var ctx = new SWEntities())
                return ctx.EntityMasterDatas.Where(w=>w.TypeEntity==entityType).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title }).ToList();
        }
    }
}