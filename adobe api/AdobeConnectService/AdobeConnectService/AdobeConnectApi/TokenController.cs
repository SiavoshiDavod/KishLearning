using Microsoft.AspNetCore.Mvc;

namespace AdobeConnectService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get([FromQuery]TokenModel model )
        {
            return Token.GenerateToken(model);
        }
    }
}