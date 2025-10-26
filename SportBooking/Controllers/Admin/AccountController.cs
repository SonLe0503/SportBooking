using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SportBookingDbContext _context;
        private readonly IMapper _mapper;

        public AccountController(SportBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ✅ Lấy toàn bộ người dùng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        // ✅ Lấy theo ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }

        // ✅ Tạo mới (ID tự tăng)
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserCreateDto dto)
        {
            // Check username trùng
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest($"Username '{dto.Username}' đã tồn tại!");

            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, result);
        }

        // ✅ Cập nhật thông tin
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserDto dto)
        {
            if (id != dto.UserId)
                return BadRequest("UserId không khớp");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Cập nhật thông tin
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Role = dto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Xoá người dùng
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
