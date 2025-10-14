using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportBooking.DTO;
using SportBooking.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SportBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly SportBookingDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthController(SportBookingDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Email already exists");
            }
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest("Username already exists");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = _mapper.Map<User>(request);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered successfully");
        }
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username" });
            }
            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!verified)
            {
                return Unauthorized(new { message = "Invalid password" });
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("userID", user.UserId.ToString()),
                    new System.Security.Claims.Claim("username", user.Username),
                    new System.Security.Claims.Claim("role", user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return Ok(new { message = "Đăng nhập thành công", token = jwtToken });
        }

    }
}
