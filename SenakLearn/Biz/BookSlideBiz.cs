using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Biz
{
    public class BookSlideBiz : RepositoryBase<SenakLearn.Models.BookSlideModel>
    {
        public static readonly BookSlideBiz Instance = new BookSlideBiz();
    }
}