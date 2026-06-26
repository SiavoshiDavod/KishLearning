using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class GroupBiz : RepositoryBase<SenakLearn.Models.Group>
    {
        public static readonly GroupBiz Instance = new GroupBiz();
        public override bool Save(SenakLearn.Models.Group model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.Group.Any(x => (model.Id == 0 || x.Id != model.Id) && x.DropDownTitle == model.DropDownTitle))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<SenakLearn.Models.Group>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    public class GroupDetailBiz : RepositoryBase<SenakLearn.Models.GroupDetail>
    {
        public static readonly GroupDetailBiz Instance = new GroupDetailBiz();
        public List<string> GetAllPhoneByGroupId(int id)
        {
            using (var context = new SWEntities())
                return context.GroupDetail.Where(x => x.GroupId == id && !string.IsNullOrEmpty(x.Mobile)).Select(x => x.Mobile).ToList();
        }
        public List<string> GetAllEmailByGroupId(int id)
        {
            using (var context = new SWEntities())
                return context.GroupDetail.Where(x => x.GroupId == id && !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToList();
        }
    }
}