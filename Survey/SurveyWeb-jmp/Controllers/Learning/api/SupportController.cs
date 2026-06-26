using Newtonsoft.Json;
using SenakLearn.Biz;
using SenakLearn.Controllers.Base;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SenakLearn.Controllers.api
{
    public class SupportController : BaseWebApiController
    {
        [HttpGet, HttpPost]
        [Route("api/Support/Student")]
        public HttpResponseMessage Student()
        {
            return ReturnOk(JsonConvert.SerializeObject(StudentSupportBiz.Instance.GetAll(x => x.Id != 0)));
        }

        [HttpGet, HttpPost]
        [Route("api/Support/ShowVideo")]
        public HttpResponseMessage Video(int id)
        {
            var videoId = Biz.StudentSupportBiz.Instance.Get(id)?.VideoId;

            return ShowVideo(videoId);
        }
       
    }
}