using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace test_identity_api.Controllers
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
                Message = "hello world"
            });
        }
    }
}
