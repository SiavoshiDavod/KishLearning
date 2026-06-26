using SenakLearn.Biz;
using SenakLearn.Biz.Person;
using SenakLearn.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Person
{
    public class MyCertificateController : BaseProfileController
    {
        public ActionResult Index()
        {
            if (Current_learn_user.RoleId == Models.Roles.Admin || Current_learn_user.RoleId == Models.Roles.User)
            {
                var certificates_person = PersonCertificateBiz.Instance.FindByUserId(Current_learn_userId);
                var certificates_cours = AzmoonUserAnswerBiz.Instance.GetAllAzmoonAcceptedByUserId(Current_learn_userId);
                certificates_person.AddRange(certificates_cours.Select(a => new Person_Certificate
                {
                    Id=a.Id,
                    Person_Course=a.SurveyEntity,
                    CourseLeader="KishLearning",
                    Code = a.Id.ToString(),
                    IssueDate = a.AcceptedDatePersian,
                    FromDate = a.FromDateCourse,
                    ToDate = a.ToDateCourse,
                    Duration=a.TimeDuration,
                    TypeCourse="Course"
                }).ToList());
                var courSums = certificates_person.GroupBy(g => g.CourseLeader).Select(a => new VW_Person_Certificate
                {
                    CoursLeader = a.Key,
                    CoursSumDuration = a.Sum(s => s.Duration)
                }).ToList();
                ViewBag.CoursSumDuration = courSums;
                return View(certificates_person);
            }
            return View("Dashboard");
        }
        public ActionResult LoadFile(int PersonCertificateId)
        {
            var certificates = PersonCertificateBiz.Instance.Get(PersonCertificateId);
            if (certificates == null) return null;
            var path = Request.RequestContext.HttpContext.Server.MapPath("/images/" + pathFile.PersonCertificate + "/" + certificates.UrlCertificate);
            var content = GetFile(path);

            return File(content, "image/jpeg");
        }
        public ActionResult Details(int certificateId,string typeCer)
        {
            Person_Certificate certificat;
            if (typeCer == "Person")
                certificat = PersonCertificateBiz.Instance.GetDetail(certificateId);
            else
            {
             var   azmoon = AzmoonUserAnswerBiz.Instance.GetAzmoonById(certificateId);
                certificat = new Person_Certificate
                {
                    Id = azmoon.Id,
                    Code = azmoon.Id.ToString(),
                    IssueDate = azmoon.AcceptedDatePersian,
                    FromDate = azmoon.FromDateCourse,
                    ToDate = azmoon.ToDateCourse,
                    Duration = azmoon.TimeDuration,
                    TypeCourse = "Course"
                };
            }
                return View(certificat);
        }
    }
}