using JWT_Authentication_Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Authentication_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private static readonly IEnumerable<User> Users = [ 
            new User() {
            Id = 1, 
                Name = "Sam",
                EmailAddress = "sam@gmail.com",
                Password = "test1"
            },
            new User() {
            Id = 2,
                Name = "Ram",
                EmailAddress = "ram@gmail.com",
                Password = "test2"
            }
        ];

        public UsersController(IConfiguration config, ILogger<UsersController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string emailAddress, [FromQuery] string password)
        {
            User user = Users.FirstOrDefault(u => u.EmailAddress == emailAddress && u.Password == password);

            if (user == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("EmailAdddress", user.EmailAddress)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: signIn
                );
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenStr);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Users);
        }
    }
}
