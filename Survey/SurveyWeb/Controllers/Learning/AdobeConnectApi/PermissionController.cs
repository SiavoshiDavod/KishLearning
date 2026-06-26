using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using System.Web.Mvc;

namespace AdobeConnectService.Controllers
{
    public class PermissionController : BaseApiController
    {
        public ActionResult Get(PermaissionFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPermissionsInfoByprincipalId(model));
        }

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
        [HttpPost]
        public ActionResult Update(PermaissionFilter model, int PermissionId)
        {
            PermissionId id = (PermissionId)PermissionId;
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.PermissionsUpdate(model,id));
        }
        [HttpPost]
        public ActionResult Subscription(PermaissionFilter model, bool SubscripeUnSubscripe)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.PermissionSubscriptionUpdate(model, SubscripeUnSubscripe));
        }
    }
}