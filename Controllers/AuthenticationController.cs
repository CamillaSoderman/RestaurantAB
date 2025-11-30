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
            if (_context.Admins.Any(a => a.Email == newAdmin.Email))
            {
                return BadRequest(new { message = "Användarnamnet är redan taget" });
            }

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
            return Ok(new { message = "Admin skapad" });
        }

        [HttpPost("login")]
        public IActionResult Login(AdminLoginDTO loginAdmin)
        {
            var admin = _context.Admins.SingleOrDefault(a => a.Email == loginAdmin.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(loginAdmin.Password, admin.PasswordHash))
            {
                return Unauthorized(new { code = "INVALID_CREDENTIALS", message = "Ogiltigt användarnamn eller lösenord" });
            }

            var token = GenerateJwtToken(admin);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Admin admin)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, admin.Role) // <-- Viktigt för Authorize(Roles="Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
