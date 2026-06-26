using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace AdobeConnectService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class BaseApiController : ControllerBase
    {
        //public BaseApiController([FromHeader(Name = "Token")] string token)
        //{
        //    Token.GetUserByToken(token);
        //}
        public TokenModel CurrentTokenModel
        {
            get
            {
               return Token.GetUserByToken(HttpContext?.Request?.Headers?["Token"].FirstOrDefault());
            }
        }
        public ClassUsingSdk AdobeConnectSdk
        {
            get
            {
                var user = CurrentTokenModel;
                return new ClassUsingSdk(user.Email, user.Pass);
            }
        }
    }
}
