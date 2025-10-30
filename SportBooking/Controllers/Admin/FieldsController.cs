using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO.Admin;
using SportBooking.Models;
using Microsoft.AspNetCore.Http;

namespace SportBooking.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class FieldsController : ControllerBase
    {
        private readonly SportBookingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public FieldsController(SportBookingDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldsDto>>> GetAll()
        {
            var fields = await _context.Fields.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<FieldsDto>>(fields));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FieldsDto>> GetById(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return NotFound();
            return Ok(_mapper.Map<FieldsDto>(field));
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 10MB
        public async Task<IActionResult> Create([FromForm] FieldCreateDto dto)
        {
            // Map dữ liệu (AutoMapper sẽ bỏ qua Image và Avatar)
            var field = _mapper.Map<Field>(dto);

            // Tạo thư mục nếu chưa có
            var folder = Path.Combine(_env.WebRootPath, "images", "fields");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // SỬA ĐỔI 1: Xử lý upload ImageFile (nếu có)
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                field.Image = $"/images/fields/{fileName}"; // gán Image thủ công
            }

            // SỬA ĐỔI 2: Xử lý upload AvatarFile (nếu có)
            if (dto.AvatarFile != null && dto.AvatarFile.Length > 0)
            {
                var avatarFileName = Guid.NewGuid() + Path.GetExtension(dto.AvatarFile.FileName);
                var avatarFilePath = Path.Combine(folder, avatarFileName);
                using (var stream = new FileStream(avatarFilePath, FileMode.Create))
                {
                    await dto.AvatarFile.CopyToAsync(stream);
                }
                field.Avatar = $"/images/fields/{avatarFileName}"; // gán Avatar thủ công
            }

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = field.FieldId }, _mapper.Map<FieldsDto>(field));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] FieldCreateDto dto)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return NotFound();

            // Map các trường text (AutoMapper bỏ qua Image, Avatar)
            _mapper.Map(dto, field);

            var folder = Path.Combine(_env.WebRootPath, "images", "fields");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // SỬA ĐỔI 3: Xử lý upload ImageFile (cải tiến: xóa file cũ nếu có)
            if (dto.ImageFile != null)
            {
                // Xóa file Image cũ
                if (!string.IsNullOrEmpty(field.Image))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, field.Image.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                // Lưu file Image mới
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                field.Image = $"/images/fields/{fileName}";
            }

            // SỬA ĐỔI 4: Xử lý upload AvatarFile (cải tiến: xóa file cũ nếu có)
            if (dto.AvatarFile != null)
            {
                // Xóa file Avatar cũ
                if (!string.IsNullOrEmpty(field.Avatar))
                {
                    var oldAvatarPath = Path.Combine(_env.WebRootPath, field.Avatar.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldAvatarPath))
                        System.IO.File.Delete(oldAvatarPath);
                }

                // Lưu file Avatar mới
                var avatarFileName = Guid.NewGuid() + Path.GetExtension(dto.AvatarFile.FileName);
                var avatarFilePath = Path.Combine(folder, avatarFileName);
                using (var stream = new FileStream(avatarFilePath, FileMode.Create))
                {
                    await dto.AvatarFile.CopyToAsync(stream);
                }
                field.Avatar = $"/images/fields/{avatarFileName}";
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return NotFound();

            // Xóa ảnh Image vật lý
            if (!string.IsNullOrEmpty(field.Image))
            {
                var fullPath = Path.Combine(_env.WebRootPath, field.Image.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            // SỬA ĐỔI 5: Xóa ảnh Avatar vật lý
            if (!string.IsNullOrEmpty(field.Avatar))
            {
                var fullAvatarPath = Path.Combine(_env.WebRootPath, field.Avatar.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(fullAvatarPath))
                    System.IO.File.Delete(fullAvatarPath);
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}