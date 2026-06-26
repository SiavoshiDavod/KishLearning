using AdobeConnectSDK.Model;
using System.Web.Mvc;

namespace AdobeConnectService.Controllers
{
    public class ShareableContentObjectController : BaseApiController
    {
        public ActionResult Get(bool? isMeeting)//([FromForm]PermaissionFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetSCOshotcuts(isMeeting ?? true));
        }
        public ActionResult GetMeeting(string likeName)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetAllMeetings(likeName));
        }
        public ActionResult GetMeetingDetail(long scoId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetMeetingDetail(scoId));
        }
        [HttpPost]
        public ActionResult Create(MeetingUpdateItemViewModel model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.MeetingUpdate(model));
        }
        [HttpPost]
        public ActionResult Update(MeetingUpdateItemViewModel model, long scoId)
        {
            model.ScoId = scoId;
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.MeetingUpdate(model, false));
        }
        public ActionResult Delete(long scoId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.ScoDelete(scoId));
        }
    }
}