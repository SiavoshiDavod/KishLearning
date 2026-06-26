using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using System.Web.Mvc;

namespace AdobeConnectService.Controllers
{
    public class AdobeGroupController : BaseApiController
    {
        
        public ActionResult Get(PrincipalFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(model, false));
        }

        public ActionResult GetPrincipalByIdGroupId(long groupId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalById(groupId));
        }
        /// <summary>
        /// Get All Group Of userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult GetGroupsOfUserId(long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(new PrincipalFilter() { PrincipalId= userId },null,true));
        }
        [HttpPost]
        public ActionResult Create(PrincipalSetupGroupViewModel model)
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

        public ActionResult AssignUserToGroup(long groupId, long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GroupMembershipUpdate(groupId, userId));
        }
        public ActionResult UnAssignUserToGroup(long groupId, long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GroupMembershipUpdate(groupId, userId, false));
        }

        public ActionResult Delete(long groupId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.UserRemove(groupId));
        }
    }
}
