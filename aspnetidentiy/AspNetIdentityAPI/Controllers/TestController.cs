using Microsoft.AspNetCore.Mvc;

namespace AspNetIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return await Task.FromResult(Ok(new
            {
                Status = "Works",
                DateTime = DateTime.UtcNow
            }));
        }
    }
}
