using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace identityapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public TestController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Okay"
            });
        }

        [Authorize]
        [HttpGet("/secured")]
        public IActionResult GetSecured()
        {
            var currentUser = User;

            return Ok(new
            {
                Message = "This is a secured endpoint"
            });
        }
    }
}