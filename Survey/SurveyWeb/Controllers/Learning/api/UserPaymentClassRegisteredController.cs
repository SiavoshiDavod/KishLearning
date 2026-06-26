using SenakLearn.Biz;
using SenakLearn.Controllers.Base;
using System.Net.Http;
using SenakLearn.Models;
using System.Web.Http;
using Newtonsoft.Json;

namespace SenakLearn.Controllers.api
{
    public class UserPaymentClassRegisteredController : BaseSecurityWebApiController
    {
        [HttpPost,HttpGet]
        [Route("api/MyPayment")]
        public HttpResponseMessage MyPayment(int skip = 0, int take = 10,bool? success=null)
        {
            return ReturnOk(JsonConvert.SerializeObject(Biz.zarinpalBiz.Instance.GetAllPagedListCurrentUser(skip, take, CurrentUser.id, success)));
        }

        [HttpPost,HttpGet]
        [Route("api/MyClass")]
        public HttpResponseMessage MyClass(int skip = 0, int take = 10)
        {
            return ReturnOk(JsonConvert.SerializeObject(OnlineClassBiz.Instance.GetAllonlineClassByUserId(skip, take, CurrentUser.id)));
        }
    }
}