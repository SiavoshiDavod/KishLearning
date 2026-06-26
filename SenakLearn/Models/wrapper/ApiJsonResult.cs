using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class ApiJsonResult
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool state => success;
        public string InnerExceptionMessage { get; set; }
        public string getMessage => (Message ?? "") + " " + (ErrorMessage ?? "");
    }
}