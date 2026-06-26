using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class PaperPublisher : BaseEntity
    {
        [GenericRequired]
        public string DropDownTitle { get; set; }
        [GenericRequired]
        public string DropDownTitleE { get; set; }
        public string IconPath { get; set; }
    }
}