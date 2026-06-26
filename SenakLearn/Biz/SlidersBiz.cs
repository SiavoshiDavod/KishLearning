using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class SlidersBiz : RepositoryBase<SenakLearn.Models.Slider>
    {
        public static readonly SlidersBiz Instance = new SlidersBiz();
        public override bool Save(Slider model, bool changeDate = true)
        {
            using (var context = new SWEntities())
                if (context.Sliders.Any(x => (model.Id == 0 || x.Id != model.Id) && x.DropDownTitle == model.DropDownTitle))
            {
                throw new Exception("عنوان وارد شده تکراریست");
            }
            return base.Save(model, changeDate);
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<Slider>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
        public override IEnumerable<Slider> GetAll()
        {
            using (var context = new SWEntities())
                return context.Sliders.Where(x => x.Archive).ToList();
        }
    }
}