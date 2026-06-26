using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class ChartVm
    {
        public string Id { get; set; }
        public string QuestionOption { get; set; }
        public string QuestionOptionUrl { get; set; }
        public int Count { get; set; }
    }
}