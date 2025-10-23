using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageFieldController : Controller
    {
        private readonly SportBookingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ImageFieldController(SportBookingDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        // GET: api/ImageField
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageFieldDTO>>> GetImageFields()
        {
            var imageFields = await _context.ImageFields.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ImageFieldDTO>>(imageFields));
        }

        // GET: api/ImageField/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageFieldDTO>> GetImageField(int id)
        {
            var imageField = await _context.ImageFields.FindAsync(id);
            if (imageField == null)
                return NotFound();

            return Ok(_mapper.Map<ImageFieldDTO>(imageField));
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)] // giới hạn 10MB
        public async Task<IActionResult> Create([FromForm] ImageFieldCreateDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest("Ảnh không hợp lệ");

            // Thư mục lưu ảnh: wwwroot/images/fields
            var folder = Path.Combine(_env.WebRootPath, "images", "fields");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Tạo tên file duy nhất
            var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
            var fullPath = Path.Combine(folder, fileName);

            // Lưu ảnh
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }

            // Lưu thông tin vào DB
            var imageField = new ImageField
            {
                FieldId = dto.FieldId,
                ImageUrl = $"/images/fields/{fileName}",
                CreatedAt = DateTime.Now
            };

            _context.ImageFields.Add(imageField);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageField), new { id = imageField.ImageId }, imageField);
        }

        // PUT: api/ImageField/5
        [HttpPut("{id}")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> Update(int id, [FromForm] ImageFieldCreateDto dto)
        {
            var imageField = await _context.ImageFields.FindAsync(id);
            if (imageField == null) return NotFound();

            // Nếu người dùng upload ảnh mới → xóa ảnh cũ & lưu lại ảnh mới
            if (dto.ImageFile != null)
            {
                // Xóa ảnh cũ
                if (!string.IsNullOrEmpty(imageField.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, imageField.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var folder = Path.Combine(_env.WebRootPath, "images", "fields");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                imageField.ImageUrl = $"/images/fields/{fileName}";
            }

            imageField.FieldId = dto.FieldId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ImageField/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var imageField = await _context.ImageFields.FindAsync(id);
            if (imageField == null) return NotFound();

            // Xóa file thật nếu có
            if (!string.IsNullOrEmpty(imageField.ImageUrl))
            {
                var fullPath = Path.Combine(_env.WebRootPath, imageField.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            _context.ImageFields.Remove(imageField);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
