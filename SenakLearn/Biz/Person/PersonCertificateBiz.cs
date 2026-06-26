using SenakLearn.JqGrid.Common;
using SenakLearn.JqGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SenakLearn.Models.Person;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace SenakLearn.Biz.Person
{
    public class PersonCertificateBiz : RepositoryBase<Person_Certificate>
    {
        public static readonly PersonCertificateBiz Instance = new PersonCertificateBiz();
        public PagedList<Person_Certificate> GetAllPagedList(GridSettings grid, int userId)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                var list = context.Person_Certificates.Where(w => w.UserId == userId).ToList().Select(a => new Person_Certificate
                {
                    Code = a.Code,
                    IssueDate = a.IssueDate,
                    Person_Course = context.Person_Courses.FirstOrDefault(i => i.Id == a.Person_CourseId)?.Title,
                    CourseLeader = context.Person_Courses.FirstOrDefault(i => i.Id == a.Person_CourseId)?.CourseLeader,
                    Person_Teacher = context.Person_Teachers.FirstOrDefault(i => i.Id == a.Person_TeacherId)?.TeacherName,
                    FromDate = a.FromDate,
                    ToDate = a.ToDate,
                    Duration = a.Duration,
                    Id = a.Id,
                    InOutTitle = a.InOut == true ? "داخلی" : "خارجی",
                    InOut = a.InOut,
                    TypeCourse = "Person"
                }).ToList();
                var certificates_cours = AzmoonUserAnswerBiz.Instance.GetAllAzmoonAcceptedByUserId(userId);
                list.AddRange(certificates_cours.Select(a => new Person_Certificate
                {
                    Id = a.Id,
                    Person_Course = a.SurveyEntity,
                    CourseLeader = "KishLearning",
                    Code = a.Id.ToString(),
                    IssueDate = a.AcceptedDatePersian,
                    FromDate = a.FromDateCourse,
                    ToDate = a.ToDateCourse,
                    Duration = a.TimeDuration,
                    InOutTitle =  "داخلی" ,
                    TypeCourse = "Course"
                }).ToList());
                return list.AsQueryable().FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
        public PagedList<PersonCertificateReportSearach> GetAllPagedListReport(GridSettings grid, PersonCertificateReportSearach search)
        {
            var list = GetAllList(search);
            var pagedLists = list.AsQueryable().FilterAndSortJqGrid(grid).ToPagedList(grid);
            return pagedLists;

        }
        public MemoryStream GetAllReportExcel(PersonCertificateReportSearach search)
        {
            var list = GetAllList(search);
            list.Add(new PersonCertificateReportSearach { CourseLeader = "جمع", Duration = list.Sum(s => s.Duration) });
            var excelService = new ExcelService();
            var memoryStream = excelService.GenerateExcelFile(list);
            return memoryStream;

        }
        public List<PersonCertificateReportSearach> GetAllList(PersonCertificateReportSearach search)
        {
            List<PersonCertificateReportSearach> Result = new List<PersonCertificateReportSearach>();
            IQueryable<PersonCertificateReportSearach> query = null;
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                if (search.ShowPersonDetail == false)
                {
                    query = (from i in context.VW_Person_Certificates
                             join u in context.learn_user on i.UserId equals u.id
                             join o in context.Orgs on u.OrgId equals o.Id into org
                             from o in org.DefaultIfEmpty()
                             join p in context.Person_Courses on i.Person_CourseId equals p.Id into cours
                             from p in cours.DefaultIfEmpty()
                             join t in context.Person_Teachers on i.Person_TeacherId equals t.Id into teacher
                             from t in teacher.DefaultIfEmpty()
                             join m in context.EntityMasterDatas.Where(w => w.TypeEntity == 1) on t.CertificateId equals m.Id into master
                             from m in master.DefaultIfEmpty()

                             select new PersonCertificateReportSearach
                             {
                                 Code = i.Code,
                                 IssueDate = i.IssueDate,
                                 IssueDatePersian = i.IssueDatePersian,
                                 Person_Course = p.Title,
                                 Person_CourseId = p.Id,
                                 UserId = i.UserId,
                                 CourseLeader = p.CourseLeader,
                                 TeacherName = t == null ? string.Empty : t.TeacherName,
                                 CourseFromDate = p.FromDate,
                                 CourseToDate = p.ToDate,
                                 Duration = i.Duration, //p.Duration,
                                 PersonCertificateId = i.Id,
                                 PersonName = u.Name + " " + u.Family + "(" + u.user_name + ")",
                                 PersonCode = u.PersonCode,
                                 PersonOrg = o.Title,
                                 PersonOrgId = o.Id,
                                 Teacher_Certificate = m != null ? m.Title : string.Empty,
                                 Teacher_Email = t != null ? t.Email : string.Empty,
                                 Teacher_Mobile = t != null ? t.Mobile : string.Empty,
                                 Course_Code = p != null ? p.Code : string.Empty,
                                 Teacher_Expertise = t != null ? t.Expertise : string.Empty,
                                 Course_Description = p.Description,
                                 InOut = i.InOut == true ? "خارجی" : "داخلی"
                             }
                                );
                }
                else
                {
                    var VW_Person_CertificatesQuery = context.VW_Person_Certificates.AsQueryable();
                    if (!string.IsNullOrEmpty(search.CourseFromDate))
                    {
                        int fromdate = int.Parse(search.CourseFromDate.Replace("/", "").ToEnglishNumber());
                        VW_Person_CertificatesQuery = VW_Person_CertificatesQuery.Where(w => w.IssueDate >= fromdate);
                    }
                    if (!string.IsNullOrEmpty(search.CourseToDate))
                    {
                        int todate = int.Parse(search.CourseToDate.Replace("/", "").ToEnglishNumber());
                        VW_Person_CertificatesQuery = VW_Person_CertificatesQuery.Where(w => w.IssueDate <= todate);
                    }
                    query = (from i in VW_Person_CertificatesQuery
                             join u in context.learn_user on i.UserId equals u.id
                             join o in context.Orgs on u.OrgId equals o.Id into org
                             from o in org.DefaultIfEmpty()
                             join p in context.Person_Courses on i.Person_CourseId equals p.Id
                             //join t in context.Person_Teachers on i.Person_TeacherId equals t.Id into teacher
                             //from t in teacher.DefaultIfEmpty()
                             select new
                             {
                                 //Code = i.Code,
                                 //IssueDate = i.IssueDate,
                                 //IssueDatePersian = i.IssueDatePersian,

                                 UserId = u.id,

                                 Duration = p.Duration,
                                 //PersonCertificateId = i.Id,
                                 PersonName = u.Name + " " + u.Family + "(" + u.user_name + ")",
                                 PersonCode = u.PersonCode,
                                 PersonOrg = o.Title,
                                 PersonOrgId = o.Id,
                                 i.InOut,
                                 //p.Title,
                                 //p.Code,
                                 //p.Description,
                                 p.CourseLeader
                             }
             ).GroupBy(g => new
             {
                 g.UserId,
                 g.PersonName,
                 g.PersonCode,
                 g.PersonOrg,
                 g.PersonOrgId,
                 g.InOut
             //,courseCode=g.Code,g.Title,g.Description
             ,
                 g.CourseLeader
             })
             .Select(a => new PersonCertificateReportSearach
             {
                 UserId = a.Key.UserId,
                 Duration = a.Sum(s => s.Duration),
                 PersonName = a.Key.PersonName,
                 PersonCode = a.Key.PersonCode,
                 PersonOrg = a.Key.PersonOrg,
                 PersonOrgId = a.Key.PersonOrgId,
                 InOut = a.Key.InOut == true ? "خارجی" : "داخلی",
                 //Course_Code=a.Key.courseCode,
                 CourseLeader = a.Key.CourseLeader,
                 //Course_Description=a.Key.Description,
                 //CourseTitle=a.Key.Title,
                 //Person_Course=a.Key.Title
             });
                }
                if (search.ShowPersonDetail == false)
                {
                    if (search.Person_CourseId != 0 && search.Person_CourseId != null)
                        query = query.Where(w => w.Person_CourseId == search.Person_CourseId);
                    if (!string.IsNullOrEmpty(search.CourseFromDate))
                    {
                        int fromdate = int.Parse(search.CourseFromDate.Replace("/", "").ToEnglishNumber());
                        query = query.Where(w => w.IssueDate >= fromdate);
                    }
                    if (!string.IsNullOrEmpty(search.CourseToDate))
                    {
                        int todate = int.Parse(search.CourseToDate.Replace("/", "").ToEnglishNumber());
                        query = query.Where(w => w.IssueDate <= todate);
                    }
                }
                if (!string.IsNullOrEmpty(search.CourseLeader))
                    query = query.Where(w => w.CourseLeader == search.CourseLeader);
                if (!string.IsNullOrEmpty(search.UserIds))
                {
                    //search.UserIds=search.UserIds.Replace()
                    query = query.Where(w => search.UserIds.Contains(w.UserId.ToString()));
                }
                if (search.PersonOrgId != null && search.PersonOrgId != 0)
                {
                    query = query.Where(w => w.PersonOrgId == search.PersonOrgId);
                }
                if (!string.IsNullOrEmpty(search.InOut))
                    query = query.Where(w => w.InOut == search.InOut);
                if (!string.IsNullOrEmpty(search.CourseLeader))
                    query = query.Where(w => w.CourseLeader == search.CourseLeader);

                Result = query.ToList();
                return Result;
            }
        }
        public int? GetCourseDuration(int userId, string year = null)
        {
            using (SWEntities db = new SWEntities())
            {

                var duration = (from i in db.Person_Certificates
                                join c in db.Person_Courses on i.Person_CourseId equals c.Id
                                where i.UserId == userId && (year == null || i.IssueDate.Contains(year))
                                select c.Duration).DefaultIfEmpty(0).Sum();
                return duration;
            }
        }
        public Person_Certificate GetDetail(int id)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                var personCer = context.Person_Certificates.SingleOrDefault(a => a.Id == id);
                var course = context.Person_Courses.FirstOrDefault(a => a.Id == personCer.Person_CourseId);
                var teacher = context.Person_Teachers.FirstOrDefault(a => a.Id == personCer.Person_TeacherId);
                var user = context.learn_user.FirstOrDefault(a => a.id == personCer.UserId);

                personCer.Person_Course = course?.Title;
                personCer.CourseLeader = course?.CourseLeader;
                personCer.Person_Teacher = teacher?.TeacherName;
                personCer.Teacher_Email = teacher?.Email;
                personCer.Teacher_Mobile = teacher?.Mobile;
                personCer.Teacher_Expertise = teacher?.Expertise;
                personCer.Teacher_Certificate = context.EntityMasterDatas.FirstOrDefault(a => a.Id == teacher.CertificateId)?.Title;
                personCer.Course_Code = course.Code;
                personCer.CourseName = course.Title;
                personCer.Course_Duration = course.Duration;
                personCer.Course_Description = course.Description;
                personCer.UserName = user?.user_name;
                personCer.FromDate = course?.FromDate;
                personCer.ToDate = course?.ToDate;
                return personCer;
            }
        }

        public List<Person_Certificate> FindByUserId(int current_userId)
        {
            try
            {
                using (var context = new SWEntities())
                {

                    var certificates = context.Person_Certificates.Where(w => w.UserId == current_userId).ToList();
                    if (!certificates.Any())
                        return new List<Person_Certificate>();
                    certificates = certificates.Select(a => new Person_Certificate
                    {
                        Code = a.Code,
                        IssueDate = a.IssueDate,
                        Person_Course = context.Person_Courses.FirstOrDefault(i => i.Id == a.Person_CourseId)?.Title,
                        CourseLeader = context.Person_Courses.FirstOrDefault(i => i.Id == a.Person_CourseId)?.CourseLeader,
                        Person_Teacher = context.Person_Teachers.FirstOrDefault(i => i.Id == a.Person_TeacherId)?.TeacherName,
                        Duration = a.Duration,
                        FromDate = context.Person_Courses.FirstOrDefault(i => i.Id == a.Person_CourseId)?.FromDate,
                        ToDate = context.Person_Courses.FirstOrDefault(i => i.Id == a.Person_CourseId)?.ToDate,
                        Id = a.Id,
                        TypeCourse = "Person"
                    }).ToList();
                    return certificates;


                }
            }
            catch (Exception ex)
            {
                return new List<Person_Certificate>();

            }
        }
    }
}