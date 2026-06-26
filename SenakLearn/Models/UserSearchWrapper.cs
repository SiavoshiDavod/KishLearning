using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
	public class UserSearchWrapper
	{
        public int UserId { get; set; }
        public string UserIds { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string PersonNameSearch { get; set; }
        public string PersonCode { get; set; }
        public string PersonOrg { get; set; }
        public int? PersonOrgId { get; set; }
        public int? TypeUserId { get; set; }
        public string TypeUser { get; set; }
        public string NationalCode { get; set; }
        public string ModalClassDiv { get; set; }
        public string FieldPushData { get; set; }
        public bool selected { get; set; }
    }
}