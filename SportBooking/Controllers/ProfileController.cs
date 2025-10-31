using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SportBooking.DTO;
using SportBooking.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SportBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly SportBookingDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ProfileController(SportBookingDbContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }

        [HttpPost("avatar")]
        [RequestSizeLimit(5_000_000)]
        public async Task<IActionResult> UpdateUserAvatar([FromForm] AvatarUploadDTO dto)
        {
            if (dto.AvatarFile == null || dto.AvatarFile.Length == 0)
                return BadRequest(new { message = "Bạn chưa chọn file ảnh." });

            // ✅ Lấy userID từ token theo chuẩn mới
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Token không hợp lệ hoặc thiếu userID." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng." });

            var folder = Path.Combine(_env.WebRootPath, "images", "users");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Xóa avatar cũ
            if (!string.IsNullOrEmpty(user.Avatar))
            {
                var oldPath = Path.Combine(_env.WebRootPath, user.Avatar.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            // Lưu file mới
            var fileName = Guid.NewGuid() + Path.GetExtension(dto.AvatarFile.FileName);
            var filePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.AvatarFile.CopyToAsync(stream);
            }

            // Cập nhật DB
            user.Avatar = $"/images/users/{fileName}";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Sinh lại token mới
            var newToken = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Cập nhật avatar thành công",
                avatarUrl = user.Avatar,
                token = newToken
            });
        }

        private string GenerateJwtToken(User user)
        {
            var keyString = _configuration["Jwt:Key"];
            var key = Encoding.UTF8.GetBytes(keyString);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? ""),
                    new Claim("avatar", user.Avatar ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
