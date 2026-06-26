using Newtonsoft.Json;
using SenakLearn.Biz;
using SenakLearn.Controllers.Base;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SenakLearn.Controllers.api
{
    public class GetOnlineClassController : BaseWebApiController
    {
        [HttpGet, HttpPost]
        [Route("api/OnlineClass")]
        public HttpResponseMessage OnlineClass([FromUri]int? groupid, [FromUri]int skip, [FromUri]int take)
        {
            return ReturnOk(JsonConvert.SerializeObject(OnlineClassBiz.Instance.GetAllPage(x => groupid == null || x.id_learn_cours_group == groupid, skip, take)));
        }

        [HttpGet, HttpPost]
        [Route("api/OfflineCoursDetail")]
        public HttpResponseMessage OfflineCoursDetail([FromUri]int id)
        {
            return ReturnOk(JsonConvert.SerializeObject(CourseBiz.Instance.GetInclude(new Models.learn_cours() { id = id }, new string[] { "OfflineVideo" })));
        }

        [HttpGet, HttpPost]
        [Route("api/OnlineClassDetail")]
        public HttpResponseMessage OnlineClassDetail([FromUri]int id)
        {
            return ReturnOk(JsonConvert.SerializeObject(OnlineClassBiz.Instance.GetInclude(new Models.OnlineClass() { Id = id }, new string[] { "OnlineClassAccoration.Details", "learn_teacher" })));
        }

        [HttpGet, HttpPost]
        [Route("api/OfflineCours")]
        public HttpResponseMessage OfflineCoursDetail([FromUri]int? groupid, [FromUri]int skip, [FromUri]int take)
        {
            return ReturnOk(JsonConvert.SerializeObject(CourseBiz.Instance.FindAll(take, skip, groupid)));
        }

        [HttpGet, HttpPost]
        [Route("api/CoursGroup")]
        public HttpResponseMessage CoursGroup()
        {
            return ReturnOk(JsonConvert.SerializeObject(CourseBiz.Instance.FindAllGroup()));
        }
        [HttpGet, HttpPost]
        [Route("api/Teacher")]
        public HttpResponseMessage Teacher([FromUri]int skip, [FromUri]int take)
        {
            return ReturnOk(JsonConvert.SerializeObject(TeacherBiz.Instance.FindAll(take, skip)));
        }
    }
}