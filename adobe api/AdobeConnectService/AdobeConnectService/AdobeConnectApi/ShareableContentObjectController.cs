using AdobeConnectSDK.Model;
using Microsoft.AspNetCore.Mvc;

namespace AdobeConnectService.Controllers
{
    public class ShareableContentObjectController : BaseApiController
    {
        [HttpGet("{isMeeting}")]
        public ActionResult Get(bool? isMeeting)//([FromForm]PermaissionFilter model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetSCOshotcuts(isMeeting ?? true));
        }
        [HttpGet("Meeting/{likeName}")]
        public ActionResult Get(string likeName)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetAllMeetings(likeName));
        }
        [HttpGet("MeetingDetail/{scoId}")]
        public ActionResult Get(long scoId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.GetMeetingDetail(scoId));
        }
        [HttpPost]
        public ActionResult Post([FromForm]MeetingUpdateItemViewModel model)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.MeetingUpdate(model));
        }
        [HttpPut("{scoId}")]
        public ActionResult Put([FromForm]MeetingUpdateItemViewModel model, long scoId)
        {
            model.ScoId = scoId;
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.MeetingUpdate(model, false));
        }
        [HttpDelete("{scoId}")]
        public ActionResult Delete(long scoId)
        {
            ClassUsingSdk sdk = AdobeConnectSdk;
            return Ok(sdk.ScoDelete(scoId));
        }
    }
}