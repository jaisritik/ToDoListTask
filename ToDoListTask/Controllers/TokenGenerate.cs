using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoListTask.Models;

namespace ToDoListTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenGenerate : ControllerBase
    {
        public IConfiguration _configuration;

        public TokenGenerate(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequest _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var userValidationResult = await ValidateUserAsync(_userData.Email, _userData.Password);

                if (userValidationResult.Item1) // Valid user
                {
                    var user = userValidationResult.Item2;

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.UserId.ToString()),
                        new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest(userValidationResult.Item3); // Return specific error message
                }
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }

        private async Task<Tuple<bool, User, string>> ValidateUserAsync(string email, string password)
        {
            List<User> users = await UserDetailsAsync();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return Tuple.Create(false, (User)null, "Email is invalid");
            }

            if (user.Password != password)
            {
                return Tuple.Create(false, user, "Password is invalid");
            }

            return Tuple.Create(true, user, string.Empty);
        }

        private async Task<List<User>> UserDetailsAsync()
        {
            return await Task.Run(() =>
            {
                List<User> users = new List<User>
                {
                    new User
                    {
                        UserId = Guid.NewGuid(),
                        FirstName = "Admin",
                        LastName = "Test",
                        Email = "admin@gmail.com",
                        Password = "Admin"
                    }
                };
                return users;
            });
        }
    }
}
