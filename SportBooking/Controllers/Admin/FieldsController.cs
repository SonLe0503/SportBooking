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
        [RequestSizeLimit(10_000_000)] // giới hạn 10MB
        public async Task<IActionResult> Create([FromForm] FieldCreateDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest("Ảnh không hợp lệ");

            // Lưu ảnh vào wwwroot/images/fields
            var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/fields", fileName);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }

            // Lưu vào DB (chỉ lưu tên ảnh)
            var field = new Field
            {
                FieldName = dto.FieldName,
                Location = dto.Location,
                Price = dto.Price,
                Description = dto.Description,
                Image = fileName,
                OwnerId = dto.OwnerId
            };

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = field.FieldId }, field);
        }

            [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] FieldCreateDto dto)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return NotFound();

            field.FieldName = dto.FieldName;
            field.Location = dto.Location;
            field.Price = dto.Price;
            field.Description = dto.Description;
            field.OwnerId = dto.OwnerId;

            if (dto.ImageFile != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "images", "fields");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                field.Image = $"/images/fields/{fileName}";
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return NotFound();

            // Xóa ảnh thật nếu có
            if (!string.IsNullOrEmpty(field.Image))
            {
                var fullPath = Path.Combine(_env.WebRootPath, field.Image.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
