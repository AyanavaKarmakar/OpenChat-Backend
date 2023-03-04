using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Chat.Models;

namespace OpenChat.Authentication.Controllers
{
    [ApiController]
    [Route("/api/v1/auth")]
    public class ApplicationUser : IdentityUser { }

    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            // check if a user with the same email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return Conflict("user with the same email already exists!");
            }

            // create a new user
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}