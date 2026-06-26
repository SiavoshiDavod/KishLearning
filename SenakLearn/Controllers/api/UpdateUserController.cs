using SenakLearn.Biz;
using SenakLearn.Controllers.Base;
using System.Net.Http;
using SenakLearn.Models;
using System.Web.Http;

namespace SenakLearn.Controllers.api
{
    public class UpdateUserController : BaseSecurityWebApiController
    {

        [HttpPost]
        [Route("api/UpdateUser")]
        public HttpResponseMessage UpdateUser([FromBody]learn_user user)
        {
            if (user == null)
            {
                throw new System.Exception("اطلاعات کاربر را وارد نمایید");
            }
            CurrentUser.Name = user.Name;
            CurrentUser.Family = user.Family;
            CurrentUser.Mobile = user.Mobile;
            CurrentUser.NationaCode = user.NationaCode;
            CurrentUser.Address = user.Address;
            user = UserBiz.Instance.UpdateUser(CurrentUser);
            return ReturnOk();
        }
        [HttpPost]
        [Route("api/ChangePass")]
        public HttpResponseMessage ChangePass([FromBody]string oldpassword, [FromBody] string newPass)
        {
            var user = UserBiz.Instance.ChangePass(CurrentUser.id, oldpassword, newPass);
            return ReturnOk();
        }
        [HttpPost]
        [Route("api/ChangePassAdobi")]
        public HttpResponseMessage ChangePassAdobi([FromBody]string oldpassword, [FromBody] string newPass)
        {
            var user = UserBiz.Instance.ChangePassAdobi(CurrentUser.id, oldpassword, newPass);
            return ReturnOk();
        }
    }
}
