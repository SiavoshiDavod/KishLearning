using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Teacher
{
    public class TeacherOnlineClassController: BaseTeacherController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OnlineClassBiz.Instance.GetAllonlineClassByTeacherId(grid, Current_learn_userId);
            //var count = Biz.OnlineClassBiz.Instance.Count;
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                UserData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult ExcelForTeacher(int onlineClassId)
        {
            var list = Biz.OnlineClassBiz.Instance.GetAllUserOfonlineClassByTeacherId( Current_learn_userId, onlineClassId);
            if (list == null)
            {
                return HttpNotFound();
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].BREEZESESSION = (i + 1).ToString();
            }
            return PrintListToExcel<learn_user>(list, learnUserColumns.Items, "دانشجویان کلاس انلاین", false);
        }
    }
}