using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using SenakLearn.Biz;
using SenakLearn.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Controllers;
using static SenakLearn.Controllers.BaseController;

namespace SenakLearn.Controllers.Base
{
    [WebApiBusinessException]
    public class BaseWebApiController : ApiController
    {
        internal HttpResponseMessage ReturnOk(string desc = "عملیات با موفقیت انجام شد")
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(desc, System.Text.Encoding.UTF8, "application/json")
            };
        }
        internal HttpResponseMessage ShowVideo(Guid? videoId)
        {
            if (videoId == null || videoId == Guid.Empty || !System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp4")))
                return null;

            var video = new VideoStream(videoId);

            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue("video/mp4"));

            return response;
        }

        [HttpGet, HttpPost]
        [Route("api/ShowVideoByPath")]
        public HttpResponseMessage Video(string id)
        {
            var video = new VideoStream(id);

            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue("video/mp4"));

            return response;
        }
        [HttpGet, HttpPost]
        [Route("api/ShowAudioByPath")]
        public HttpResponseMessage Audio(string id)
        {
            var video = new AudioStream(id);

            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue("audio/mp3"));

            return response;
        }
    }

    public class BaseSecurityWebApiController : BaseWebApiController
    {
        public Models.learn_user CurrentUser { private set; get; }
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            try
            {
                CurrentUser = Biz.UserBiz.Instance.GetUserByToken(HttpContext.Current?.Request?.Headers?.GetValues("Token")?.FirstOrDefault());
            }
            catch (System.Exception e)
            {
                var exceptionMessage = e.InnerException?.Message ?? e.Message;
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(exceptionMessage),
                };
                throw new HttpResponseException(response);
            }
            base.Initialize(controllerContext);
        }
    }


    public class VideoStream
    {
        private readonly string _filename;

        public VideoStream(Guid? videoId)
        {
            _filename = System.Web.Hosting.HostingEnvironment.MapPath("/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp4");
        }
        public VideoStream(string guid)
        {
            _filename = System.Web.Hosting.HostingEnvironment.MapPath("/images/VideoFile/" + guid);
        }

        public async System.Threading.Tasks.Task WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            //await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Video);
            try
            {
                var buffer = new byte[2097152];

                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read,FileShare.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (Exception)
            {
                return;
                // throw ex;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }

    public class AudioStream
    {
        private readonly string _filename;

        public AudioStream(Guid? videoId)
        {
            _filename = System.Web.Hosting.HostingEnvironment.MapPath("/images/VideoFile/" + videoId.ToString().Replace("-", "") + ".mp3");
        }
        public AudioStream(string guid)
        {
            _filename = System.Web.Hosting.HostingEnvironment.MapPath("/images/VideoFile/" + guid);
        }

        public async System.Threading.Tasks.Task WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            //await Biz.SiteReviewCountBiz.Instanse.Update(Models.SiteReviewCountType.Video);
            try
            {
                var buffer = new byte[2097152];

                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (Exception)
            {
                return;
                // throw ex;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}
