using SenakLearn.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;
using System.Web.Mvc;
using MVC.Controls;
using SenakLearn.Models.wrapper;

namespace SenakLearn.Biz
{
    public class MenuBiz
    {
        public static readonly MenuBiz Instance = new MenuBiz();
        public List<MenuWrapper> GetActiveMenu()
        {
            using (SWEntities db = new SWEntities())
            {
                var list = (from i in db.Menus.Where(x => x.Status && !x.Title.Contains("6Step")).OrderBy(o=>o.Order)
                                //join d in db.DynamicForms on i.Id equals d.MenuId
                                //let details=from x in db.DynamicForms where x.MenuId==i.Id
                            select new MenuWrapper
                            {
                                Id = i.Id,
                                Title = i.Title,
                                Order = i.Order,
                                Status = i.Status,
                                DynamicForms=i.DynamicForms.Where(w=>w.Status).OrderBy(o=>o.Order).ToList(),
                            }).ToList();
                //return db.Menus.Where(x => x.Status && !x.Title.Contains("6Step")).Include(x => x.DynamicForms).ToList();
                return list;
            }
        }
        public string[] Get6StepMenu()
        {
            using (SWEntities db = new SWEntities())
            {
                var result = db.DynamicForms.Where(x => x.Menu.Title.Contains("6Step")).Select(x => new { x.Id, x.Order }).ToList();
                if (result.Count == 0)
                {
                    return new string[6];
                }
                var res = new string[result.Count > 6 ? result.Count : 6];
                for (int i = 0; i < result.Count; i++)
                {
                    var order = result[i].Order;
                    res[order == 0 ? 0 : order - 1] = "/Home/step/" + result[i].Id;
                }
                return res;
            }
        }

        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.Menus.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title }).ToList();
        }
    }
}