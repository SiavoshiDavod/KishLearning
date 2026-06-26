using SurveyWeb.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.wrapper
{
    public class PermissionVm
    {
        public string permission { get; set; }
        public int RoleId { get; set; }
        //public List<Permisstion> Permissions=> Permission.Select(x=>(int))
    }
}