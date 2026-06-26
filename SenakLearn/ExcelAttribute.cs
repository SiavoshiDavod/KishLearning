using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn
{
	public class ExcelAttribute:Attribute
	{
        public bool IsColumnOut{ get; set; }
        public bool IsColumnIn{ get; set; }
        public string Title { get; set; }
    }
}