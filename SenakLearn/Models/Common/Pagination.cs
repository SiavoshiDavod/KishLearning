using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Common
{
    public class Pagination<T>
    {
        public List<PageEntity> Pages { get; set; }
        public int InPage { get; set; } = 9;
        public int CurrentPage { get; set; } = 1;
        public int Next { get; set; }
        public int Previous { get; set; }
        public string NextClass { get; set; }
        public string PreviousClass { get; set; }
        public bool Display { get; set; }
        public string Query { get; set; }
        public int CountAll { get { return Data.Count; } set { } }
        public int CountPage { get { return (int)Math.Ceiling((double)Data.Count / (double)InPage); } set { } }
        public ICollection<T> Data { get; set; }
        public IList<T> DataPage
        {
            get
            {
                var dataPage = Data.Skip<T>((CurrentPage-1)*InPage).Take<T>(InPage).ToList();
                return dataPage;
            }
            set { }
        }

    }
}