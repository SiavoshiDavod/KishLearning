using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz.Person
{
    public class PersonTeacherBiz : RepositoryBase<Models.Person.Person_Teacher>
    {
        public static readonly PersonTeacherBiz Instance = new PersonTeacherBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new SWEntities())
                return ctx.Person_Teachers.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.TeacherName }).ToList();
        }
        public PagedList<Person_Teacher> GetAllPagedList(GridSettings grid)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                return context.Person_Teachers.ToList().Select(a => new Person_Teacher
                {
                    TeacherName = a.TeacherName,
                    Expertise = a.Expertise,
                    CertificateId = a.CertificateId,
                    CertificateName = context.EntityMasterDatas.FirstOrDefault(i => i.Id == a.CertificateId)?.Title,
                    Mobile = a.Mobile,
                    Email = a.Email,
                    Id = a.Id,
                }).ToList().AsQueryable().FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
    }
}