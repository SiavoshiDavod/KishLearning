using System.Net;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Base
{
    public class HttpStatusContentResultForApi : JsonResult
    {
        private readonly HttpStatusCode _httpStatus;

        public HttpStatusContentResultForApi(object data, HttpStatusCode httpStatus)
        {
            Data = data;
            _httpStatus = httpStatus;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.StatusCode = (int)_httpStatus;
            base.ExecuteResult(context);
        }
        //private string _content;
        //private HttpStatusCode _statusCode;
        //private string _statusDescription;

        //public HttpStatusContentResultForApi(string content,
        //                               HttpStatusCode statusCode = HttpStatusCode.OK,
        //                               string statusDescription = null)
        //{
        //    _content = content;
        //    _statusCode = statusCode;
        //    _statusDescription = statusDescription;
        //}

        //public override void ExecuteResult(ControllerContext context)
        //{
        //    var response = context.HttpContext.Response;
        //    response.StatusCode = (int)_statusCode;
        //    if (_statusDescription != null)
        //    {
        //        response.StatusDescription = _statusDescription;
        //    }

        //    if (_content != null)
        //    {
        //        context.HttpContext.Response.Write(_content);
        //    }
        //}
    }
}
