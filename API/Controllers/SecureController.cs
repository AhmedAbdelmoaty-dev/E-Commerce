using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public ActionResult<string> GetSecureData()
        {
            return Ok("This is a secure endpoint, accessible only to authenticated users.");
        }
    }
}
