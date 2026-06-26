using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class OfflineVideoWrapper: ParentChildEntity
    {
        public Guid VideoId { get; set; }
        public bool IsFree { get; set; }
        public int learn_coursId { get; set; }
        public string Title { get; set; }
    }
}