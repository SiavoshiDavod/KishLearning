using Newtonsoft.Json;
using SenakLearn.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
	public class UserWrapper
	{
        public int id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public DateTime date_register { get; set; } = DateTime.Now;
        public bool status { get; set; } = true;
        public Roles RoleId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        ////[RegularExpression(@"\d",ErrorMessage = "کد ملی را صحیح وارد کنید")]
        public string NationaCode { get; set; }
        public string PassAdobe { get; set; } = "123456";

         
        public string BREEZESESSION { get; set; }
         
        public string NameForEmail => string.IsNullOrWhiteSpace(Name + " " + Family) ? user_name : Name + " " + Family;
        public Province? Province { get; set; }
        public string City { get; set; }
        public string Education { get; set; }
        public string Expertise { get; set; }
        public string FatherName { get; set; }
        public string BirthLocation { get; set; }
        public string Tel { get; set; }
        public DateTime? BirthDay { get; set; }

         
        public virtual string BirthDayShamsi
        {
            get => BirthDay == null ? "" : BirthDay.Value.ToPersianDate();
            set => BirthDay = value.ToGregorianDate();
        }

        public virtual string date_register_Shamsi
        {
            get => date_register.ToPersianDate();
            set => date_register = value.ToGregorianDate();
        }
        public string Shenasname { get; set; }

         
        public string ProvinceName { get { return this.Province.ToString(); } set { } }
         
        public string RoleName { get { return this.RoleId.ToString(); } set { } }
         
        public List<Permisstion> Permisstions { get; set; }
        public int? TypeUser { get; set; }
        public string TypeUserName { get; set; }
        public int? PostId { get; set; }
        public string PostName { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public string PersonCode { get; set; }
        public int? CourseDurationSum { get; set; }
        public int? CourseDurationYear { get; set; }
    }
}