using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using Microsoft.AspNetCore.Mvc;

namespace AdobeConnectService.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpGet]
        public ActionResult Get([FromForm]PrincipalFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(model));
        }

        [HttpGet("{userId}")]
        public ActionResult Get(long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalById(userId));
        }

        /// <summary>
        /// Get All User in groupId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("ByGroupId/{groupId}")]
        public ActionResult GetByGroupId(long groupId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(new PrincipalFilter() { GroupId = groupId }, null, true));
        }
        [HttpPost]
        public ActionResult Post([FromForm] PrincipalSetupViewModel model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.UserCreate(model));
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public ActionResult Put(int id, [FromForm] PrincipalSetupViewModel model)
        //{
        //    model.PrincipalId = id;
        //    ClassUsingSdk sdk = AdobeConnectSdk;
        //    System.Collections.Generic.IEnumerable<PrincipalListItem> list = sdk.GetPrincipalList(model.Email, model.PrincipalId);
        //    return Ok(sdk.UserUpdate(model));
        //}

        [HttpPut]
        [Route("ResetPass/{userId}")]
        public ActionResult ResetPass(long userId, [FromForm] string newPass)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.ResetPassword(userId, newPass));
        }
        [HttpPut]
        [Route("ChangePass")]
        public ActionResult ChangePass( [FromForm] string newPass)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.ChangePassword( newPass));
        }

        [HttpDelete("{userId}")]
        public ActionResult Delete(long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.UserRemove(userId));
        }
    }
}
