using SenakLearn.Biz;
using System;
using SenakLearn.Controllers.Base;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http;

namespace SenakLearn.Controllers.api
{
    public class TokenController : BaseWebApiController
    {
        [HttpGet, HttpPost]
        [Route("api/token")]
        public HttpResponseMessage Token([FromUri]string username, [FromUri] string password)
        {
            if (string.IsNullOrEmpty(username?.Trim()))
            {
                throw new Exception("نام کاربری معتبر نمی باشد");
            }

            if (string.IsNullOrEmpty(password?.Trim()))
            {
                throw new Exception(" کلمه عبور معتبر نمی باشد");
            }
            Models.learn_user user = UserBiz.Instance.FindByUserAndPass(username?.Trim(), password);
            if (user == null)
            {
                throw new Exception("نام کاربری یا کلمه عبور معتبر نمی باشد");
            }
            if (!user.status)
            {
                throw new Exception("حساب کاربری شما غیرفعال است");
            }
            return ReturnOk(UserBiz.Instance.GenerateToken(user));
        }
        [HttpGet, HttpPost]
        [Route("api/GetUserByToken")]
        public HttpResponseMessage GetUserByToken([FromUri]string token)
        {
            var user = UserBiz.Instance.GetUserByToken(token);
            return ReturnOk(JsonConvert.SerializeObject(user));
        }
    }
}
