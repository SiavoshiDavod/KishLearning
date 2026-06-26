using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class MenuWrapper
    {
        public int Id { get; set; }
        public int Order { get; set; }

        public string Title { get; set; }
        public bool Status { get; set; }
        public ICollection<DynamicForm> DynamicForms { get; set; }
    }
}