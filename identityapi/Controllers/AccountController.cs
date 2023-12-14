using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace identityapi.Controllers
{
    [Route("id")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IDataProtectionProvider dataProtectionProvider;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IDataProtectionProvider dataProtectionProvider)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.dataProtectionProvider = dataProtectionProvider;
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

        [HttpPost("claims")]
        public async Task<IActionResult> AddUsernameClaim([FromQuery] string username)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUser = await userManager.GetUserAsync(User);
                
                if (currentUser != null)
                    await userManager.AddClaimAsync(currentUser, new Claim("username", username));
                else return BadRequest();

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                Dictionary<string,string> allClaims = new();

                foreach (var claim in User.Claims)
                {
                    allClaims.Add(claim.Type, claim.Value);
                }

                return allClaims.Any() ? Ok(allClaims.Select(k => new { Type = k.Key, Value = k.Value })) : Ok("No claims");
            }
                return BadRequest();
        }
    }
}
