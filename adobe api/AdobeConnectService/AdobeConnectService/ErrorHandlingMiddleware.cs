using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AdobeConnectService
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        protected virtual async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            if (null == exception) return;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode =StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(exception.Message);
        }
    }
}
