using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace identityapi.Controllers
{
    [Route("id")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}
