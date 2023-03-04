using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Chat.User;
using Chat.UserDto;

namespace OpenChat.Auth.Controllers
{
    [ApiController]
    [Route("/api/v1/auth")]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        [HttpPost("register")]
        public Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Task.FromResult<ActionResult<User>>(new ActionResult<User>(user));
        }

        [HttpPost("login")]
        public Task<ActionResult<string>> Login(UserDto request)
        {
            if (user.Username == request.Username)
            {
                if (VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return Task.FromResult<ActionResult<string>>(new ActionResult<string>("User logged in"));
                }
            }

            return Task.FromResult<ActionResult<string>>(new ActionResult<string>("User not found"));
        }

        // create a password hash and salt
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // verify password hash and salt
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }
    }
}