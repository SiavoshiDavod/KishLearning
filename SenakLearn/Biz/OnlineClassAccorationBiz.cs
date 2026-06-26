using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Biz
{
    public class OnlineClassAccorationBiz : RepositoryBase<SenakLearn.Models.OnlineClassAccoration>
    {
        public static readonly OnlineClassAccorationBiz Instance = new OnlineClassAccorationBiz();
    }
}