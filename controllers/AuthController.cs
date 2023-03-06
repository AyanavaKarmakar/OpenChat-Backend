using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Chat.UserDto;
using Chat.Models;

namespace OpenChat.Auth.Controllers
{
    [ApiController]
    [Route("/api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ChatDB _context;
        private readonly IConfiguration _configuation;

        public AuthController(ChatDB context, IConfiguration configuation)
        {
            _context = context;
            _configuation = configuation;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            if (await _context.Users.AnyAsync(user => user.Username == request.Username))
            {
                var response = new
                {
                    message = "Username already taken"
                };
                return BadRequest(response);
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new UserEntity
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = CreateToken(user);
            var SuccessResponse = new
            {
                username = user.Username,
                token = token
            };

            return Ok(SuccessResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Username == request.Username);

            if (user == null)
            {
                var response = new
                {
                    message = "User doesn't exist"
                };
                return Unauthorized(response);
            }
            else if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                var response = new
                {
                    message = "Login failed"
                };
                return Unauthorized(response);
            }

            var token = CreateToken(user);
            var SuccessResponse = new
            {
                username = user.Username,
                token = token
            };

            return Ok(SuccessResponse);
        }

        private string CreateToken(UserEntity user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuation
                .GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
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
        private bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            if (passwordHash == null || passwordSalt == null)
            {
                return false;
            }

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}