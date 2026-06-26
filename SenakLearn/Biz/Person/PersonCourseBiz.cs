using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models.Person;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz.Person
{
    public class PersonCourseBiz : RepositoryBase<Models.Person.Person_Course>
    {
        public static readonly PersonCourseBiz Instance = new PersonCourseBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.Person_Courses.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title + " : " + i.CourseLeader }).ToList();
        }
        public List<SelectListItem> DropDownAll()
        {
            using (var ctx = new SWEntities())
            {
                var list = ctx.Person_Courses.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Title + " : " + i.CourseLeader }).ToList();
                list.Insert(0, new SelectListItem { Text = "..." });
                return list;
            }
        }
        public List<SelectListItem> DropDownLeaders()
        {
            using (var ctx = new SWEntities())
            {
                var list = ctx.Person_Courses.Select(a => a.CourseLeader).Distinct().Select(i => new SelectListItem() { Value = i, Text = i }).ToList();
                list.Insert(0, new SelectListItem { Text = "..." });
                return list;
            }
        }
        public async Task<MemoryStream> GetAllReportExcel()
        {
            using (var ctx = new SWEntities())
            {
                var list = await ctx.Person_Courses.ToListAsync();
                var excelService = new ExcelService();
                var memoryStream = excelService.GenerateExcelFile(list);
                return memoryStream;
            }
        }
    }
}