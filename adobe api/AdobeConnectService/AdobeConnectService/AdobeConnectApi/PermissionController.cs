using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using Microsoft.AspNetCore.Mvc;

namespace AdobeConnectService.Controllers
{
    public class PermissionController : BaseApiController
    {
        [HttpGet]
        public ActionResult Get([FromForm]PermaissionFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPermissionsInfo(model));
        }

        [HttpDelete("Reset/{aclId}")]
        public ActionResult Reset(long aclId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.PermissionsReset(aclId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aclId"></param>
        /// <param name="special">0= ViewHidden(public),1=Remove(protected),2=Denied(private)</param>
        /// <returns></returns>
        [HttpPut("SpecialUpdate/{aclId}/{special}")]
        public ActionResult PublicAccessUpdate(long aclId, int special)
        {
            SpecialPermissionId id = (SpecialPermissionId)special;
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.SpecialPermissionsUpdate(aclId,id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="PermissionId">0= None,1=Admin,2=Author,3=Learner,4=View,5=ViewHidden,6=PublicAccess,7=Host,8= MiniHost,9= Remove,10=Publish,11= Manage,12=Denied</param>
        /// <returns></returns>
        [HttpPut("{PermissionId}")]
        public ActionResult Put([FromForm]PermaissionFilter model, int PermissionId)
        {
            PermissionId id = (PermissionId)PermissionId;
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.PermissionsUpdate(model,id));
        }

        [HttpPut("SubscriptionCourse")]
        public ActionResult Subscription([FromForm]PermaissionFilter model, [FromForm] bool SubscripeUnSubscripe)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.PermissionSubscriptionUpdate(model, SubscripeUnSubscripe));
        }
    }
}