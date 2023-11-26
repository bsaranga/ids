using Microsoft.AspNetCore.Mvc;

namespace AspNetIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new
            {
                Status = "Works",
                DateTime = DateTime.UtcNow
            });
        }
    }
}
