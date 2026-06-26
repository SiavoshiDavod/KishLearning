using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using System.Web.Mvc;

namespace AdobeConnectService.Controllers
{
    public class AdobeUserController : BaseApiController
    {
        public ActionResult Get(PrincipalFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(model));
        }

        public ActionResult GetByUserId(long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalById(userId));
        }

        /// <summary>
        /// Get All User in groupId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult GetAllUserInGroupId(long groupId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetPrincipalList(new PrincipalFilter() { GroupId = groupId }, null, true));
        }
        [HttpPost]
        public ActionResult Create( PrincipalSetupViewModel model)
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

        public ActionResult ResetPass(long userId,  string newPass)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.ResetPassword(userId, newPass));
        }
        public ActionResult ChangePass(string newPass)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.ChangePassword( newPass));
        }

        public ActionResult Delete(long userId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.UserRemove(userId));
        }
    }
}
