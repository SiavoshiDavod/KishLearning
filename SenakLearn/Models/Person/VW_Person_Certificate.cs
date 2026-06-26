using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Person
{

	public class VW_Person_Certificate : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        public int Person_CourseId { get; set; }
        public int Person_TeacherId { get; set; }
        public string Code { get; set; }
        public int IssueDate { get; set; }
        public string IssueDatePersian { get; set; }
        public int Duration { get; set; }
        public string CertificateFile { get; set; }
        public int UserId { get; set; }
        public string UrlCertificate { get; set; }
        public string CoursLeader { get; set; }
        public int CoursSumDuration { get; set; }
        public bool InOut { get; set; }


    }
}