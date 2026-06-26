using MVC.Controls.Grid;
using SurveyWeb.Models;
using SurveyWeb.Models.Security;
using SurveyWeb.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class BaseAdminController : BaseController
    {
        
        public static GridColumnModelList<BaseEntity> BaseEntityColumns { get; private set; } = GetBaseEntityColumns();
        public static GridColumnModelList<BaseEntity> GetBaseEntityColumns()
        {
            if (BaseEntityColumns == null)
            {
                BaseEntityColumns = new GridColumnModelList<BaseEntity>();
                BaseEntityColumns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                BaseEntityColumns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetWidth("50");
                BaseEntityColumns.Add("DropDownTitle").SetCaption("عنوان").SetWidth("50");
            }
            return BaseEntityColumns;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                User Current_learn_user = GetSessionUser();

                var controller = filterContext.RouteData.Values.FirstOrDefault(x => x.Key == "controller").Value?.ToString();
                if (controller.ToLower()=="dashboard")
                {
                    SetViewBagMenu(Current_learn_user);
                    base.OnActionExecuting(filterContext);
                    return;
                }
                try
                {
                    Permisstion p = (Permisstion)Enum.Parse(typeof(Permisstion), controller, true);
                    //if (System.Enum.TryParse(controller, out Permisstion p))
                    if (Biz.UserBiz.Instance.IsAccess(p, Current_learn_user.Id))
                    {
                        SetViewBagMenu(Current_learn_user);
                        base.OnActionExecuting(filterContext);
                        return;
                    }
                }
                catch (Exception e)
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }

            }
            TempData["ErrorMessage"] = "شما دسترسی لازم به این صفحه را ندارید";
            filterContext.Result = new RedirectResult("/Home/UserLogin?ReturnUrl="+ filterContext.HttpContext.Request.RawUrl);
            base.OnActionExecuting(filterContext);
        }
        private void SetViewBagMenu(User Current_learn_user)
        {
            List<PermissionParentChild> AccessMenu = new List<PermissionParentChild>();

            foreach (var item in GetTreeJsonModel.PermissionParentChildStaticList)
            {
                bool hasChild = false;
                foreach (var sub in item.Childs)
                {
                    if (Current_learn_user.Permisstions.Any(x => x == sub.Permisstion))
                    {
                        hasChild = true;
                        break;
                    }
                }
                if (hasChild)
                {
                    PermissionParentChild parent = new PermissionParentChild() { Childs = new List<PermissionParentChild>(), Description = item.Description, Permisstion = item.Permisstion };
                    foreach (var sub in item.Childs)
                    {
                        if (Current_learn_user.Permisstions.Any(x => x == sub.Permisstion))
                            parent.Childs.Add(sub);
                    }
                    AccessMenu.Add(parent);
                }
            }

            ViewBag.AccessMenu = AccessMenu;
        }
    }
}
