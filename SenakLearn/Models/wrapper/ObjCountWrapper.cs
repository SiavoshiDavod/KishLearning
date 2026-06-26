using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class ObjCountWrapper
    {
        
        public string ObjName { get; set; }
        public string ObjType { get; set; }
        public string ObjId { get; set; }
        public string ObjTitle { get; set; }
        public string ObjDescript { get; set; }
        public int Count { get; set; }
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedDateShamsi
        {
            get => CreatedDate.ToPersianDateTime();
            set => CreatedDate = value.ToGregorianDate();
        }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual string UpdateDateShamsi
        {
            get => UpdateDate?.ToPersianDate();
            set => UpdateDate = value.ToGregorianDate();
        }
        public virtual string act { get; set; }
    }
}