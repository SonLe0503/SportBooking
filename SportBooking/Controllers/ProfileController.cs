using AutoMapper;
using Microsoft.AspNetCore.Authorization; // <-- QUAN TRỌNG
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // <-- Thêm
using SportBooking.DTO;
using SportBooking.Models;
using System.IdentityModel.Tokens.Jwt; // <-- Thêm
using System.Security.Claims; // <-- QUAN TRỌNG
using System.Text; // <-- Thêm

namespace SportBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // <-- TOÀN BỘ CONTROLLER NÀY YÊU CẦU ĐĂNG NHẬP
    public class ProfileController : ControllerBase
    {
        private readonly SportBookingDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration; // <-- Thêm IConfiguration

        [HttpPost("avatar")]
        [Authorize] // <-- Bắt buộc user phải đăng nhập
        [RequestSizeLimit(5_000_000)] // Giới hạn 5MB
        public async Task<IActionResult> UpdateUserAvatar([FromForm] AvatarUploadDTO dto) // <-- 1. Dùng DTO
        {
            // 2. Kiểm tra file (lấy từ dto)
            if (dto.AvatarFile == null || dto.AvatarFile.Length == 0)
            {
                return BadRequest(new { message = "Bạn chưa chọn file ảnh." });
            }

            // 3. Lấy file ra từ DTO
            var avatarFile = dto.AvatarFile; // <-- KHÔNG CÒN LỖI

            // 4. Lấy UserId của user đang đăng nhập từ Token (JWT)
            var userIdClaim = User.FindFirst("userID");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "UserId trong token không hợp lệ." });
            }

            // 5. Tìm user trong Database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }

            // 6. Xử lý lưu file
            var folder = Path.Combine(_env.WebRootPath, "images", "users");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 7. Xóa file avatar cũ (nếu có)
            if (!string.IsNullOrEmpty(user.Avatar))
            {
                var oldAvatarPath = Path.Combine(_env.WebRootPath, user.Avatar.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldAvatarPath))
                {
                    System.IO.File.Delete(oldAvatarPath);
                }
            }

            // 8. Lưu file avatar mới
            var avatarFileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
            var avatarFilePath = Path.Combine(folder, avatarFileName);

            using (var stream = new FileStream(avatarFilePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            // 9. Cập nhật URL mới vào database
            user.Avatar = $"/images/users/{avatarFileName}";

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // 10. Tạo token mới với thông tin avatar mới
            var newJwtToken = GenerateJwtToken(user); // Đảm bảo bạn có hàm này

            return Ok(new
            {
                message = "Cập nhật avatar thành công",
                avatarUrl = user.Avatar,
                token = newJwtToken // Trả về token mới
            });
        }

        // -----------------------------------------------------------------
        //
        // HÀM HELPER: TẠO TOKEN (Copy từ AuthController của bạn)
        //
        // -----------------------------------------------------------------
        private string GenerateJwtToken(User user)
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key not configured");

            var key = Encoding.UTF8.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userID", user.UserId.ToString()),
                    new Claim("username", user.Username ?? ""),
                    new Claim("role", user.Role ?? ""), // Sửa lỗi null
                    new Claim("avatar", user.Avatar ?? "") // Giờ đã có avatar mới
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}