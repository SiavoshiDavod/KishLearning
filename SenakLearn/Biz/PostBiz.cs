using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class PostBiz : RepositoryBase<Models.Post>
    {
        public static readonly PostBiz Instance = new PostBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.Posts.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title }).ToList();
        }
    }
}