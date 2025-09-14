using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantAB.Data;
using RestaurantAB.DTOs;
using RestaurantAB.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly RestaurantABDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(RestaurantABDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public IActionResult Register(AdminRegisterDTO newAdmin)
        {
            // Check if username already exists
            if (_context.Admins.Any(a => a.Username == newAdmin.Username))
            {
                return BadRequest(new { message = "Användarnamnet är redan taget" });
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(newAdmin.Password);
            
            var admin = new Admin
            {
                Username = newAdmin.Username,
                PasswordHash = passwordHash,
                Email = newAdmin.Email,
                Role = "Admin"
            };

            _context.Admins.Add(admin);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginAdminDTO loginAdmin)
        {
            var admin = _context.Admins.SingleOrDefault(a => a.Username == loginAdmin.Username);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(loginAdmin.Password, admin.PasswordHash))
            {
                return Unauthorized(new { message = "Ogiltigt användarnamn eller lösenord" });
            }

            var token = GenerateJwToken(admin);

            return Ok(new {token});
        }

        //public static class JwtGenerator
        //{
        //    public static string GenerateJwt(IConfiguration _config, Admin admin)
        //    {
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var claims = new[]
        //        {
        //    new Claim(ClaimTypes.Name, admin.Username)
        //};

        //        var token = new JwtSecurityToken(
        //            issuer: _config["Jwt:Issuer"],
        //            audience: _config["Jwt:Audience"],
        //            claims: claims,
        //            expires: DateTime.UtcNow.AddHours(1),
        //            signingCredentials: credentials
        //            );

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }
        //}

        private string GenerateJwToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
               new Claim(ClaimTypes.Name, $"{admin.Username}"),
               new Claim(ClaimTypes.Role, admin.Role)
           };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

      
        }

}
