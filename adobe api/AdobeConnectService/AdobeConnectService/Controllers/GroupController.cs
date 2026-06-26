using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using Microsoft.AspNetCore.Mvc;


namespace AdobeConnectService.Controllers
{
    public class GroupController : BaseApiController
    {
        [HttpGet]
        public ActionResult Get([FromForm]PrincipalFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(model, false));
        }

        [HttpGet("{groupId}")]
        public ActionResult Get(long groupId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalById(groupId));
        }
        /// <summary>
        /// Get All Group Of userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("ByUserId/{userId}")]
        public ActionResult GetByUserId(long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(new PrincipalFilter() { PrincipalId= userId },null,true));
        }

        [HttpPost]
        public ActionResult Post([FromForm] PrincipalSetupGroupViewModel model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GroupCreate(model));
        }

        //// PUT api/values/5
        //[HttpPut("{groupId}")]
        //public ActionResult Put(long groupId, [FromForm] PrincipalSetupGroupViewModel model)
        //{
        //    model.PrincipalId = groupId.ToString();
        //    ClassUsingSdk sdk = AdobeConnectSdk;
        //    return Ok(sdk.GroupUpdate(model));
        //}

        [HttpPut("AssignUserToGroup/{groupId}/{userId}")]
        public ActionResult AssignUserToGroup(long groupId, long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GroupMembershipUpdate(groupId, userId));
        }
        [HttpPut("UnAssignUserToGroup/{groupId}/{userId}")]
        public ActionResult UnAssignUserToGroup(long groupId, long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GroupMembershipUpdate(groupId, userId, false));
        }

        [HttpDelete("{groupId}")]
        public ActionResult Delete(long groupId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.UserRemove(groupId));
        }
    }
}
