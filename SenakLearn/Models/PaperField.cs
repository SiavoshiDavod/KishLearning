using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    public class PaperField : BaseEntity
    {
        [GenericRequired]
        public string DropDownTitle { get; set; }
    }
}