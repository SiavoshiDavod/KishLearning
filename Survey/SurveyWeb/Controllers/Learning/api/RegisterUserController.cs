using SenakLearn.Biz;
using SenakLearn.Controllers.Base;
using System.Net.Http;
using SenakLearn.Models;
using System.Web.Http;

namespace SenakLearn.Controllers.api
{
    public class RegisterUserController : BaseWebApiController
    {

        [HttpPost]
        [Route("api/RegisterUser")]
        public HttpResponseMessage RegisterUser([FromBody]learn_user user)
        {
            if (user == null)
            {
                throw new System.Exception("اطلاعات کاربر را وارد نمایید");
            }
            user = UserBiz.Instance.RegisterUser(user);
            return ReturnOk();
        }
    }
}
