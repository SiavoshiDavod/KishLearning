using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class LearnWrapper
    {
        public int id { get; set; }
        public string name { get; set; }
        public int id_group { get; set; }
        public string group_name { get; set; }
        public int time { get; set; }
        public string doc { get; set; }
        public string doc2 { get; set; }
        public int? id_teacher { get; set; }
        public string teacher_name { get; set; }
        public string image { get; set; }
        public int? Monetary { get; set; }
        public string TypeCours { get; set; }
        public int? PriceAfterDiscount { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public  IEnumerable<learn_cours> listCourse { get; set; }
        public  IEnumerable<OnlineClass> listOnlineClass { get; set; }
    }
}