using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/manage/facilityOwner/[controller]")]
    [ApiController]
    public class ManageBookingController : ControllerBase
    {
        private readonly SportBookingDbContext _context;

        public ManageBookingController(SportBookingDbContext context)
        {
            _context = context;
        }

        //  GET
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Field)
                .Include(b => b.User)
                .Select(b => new
                {
                    b.BookingId,
                    b.UserId,
                    CustomerName = b.User != null ? b.User.Username : "N/A",
                    b.FieldId,
                    FieldName = b.Field != null ? b.Field.FieldName : "N/A",
                    b.BookingDate,
                    b.StartTime,
                    b.EndTime,
                    b.TotalPrice,
                    b.Status
                })
                .ToListAsync();

            return Ok(bookings);
        }

        //  POST
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.StartTime >= dto.EndTime)
                return BadRequest("Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            var fieldExists = await _context.Fields.AnyAsync(f => f.FieldId == dto.FieldId);

            if (!userExists)
                return BadRequest("Người dùng không tồn tại.");
            if (!fieldExists)
                return BadRequest("Sân không tồn tại.");

            //  Kiểm tra trùng giờ đặt sân
            var hasConflict = await _context.Bookings.AnyAsync(b =>
                b.FieldId == dto.FieldId &&
                b.BookingDate == dto.BookingDate &&
                b.Status != "Cancelled" &&
                (
                    (dto.StartTime >= b.StartTime && dto.StartTime < b.EndTime) ||
                    (dto.EndTime > b.StartTime && dto.EndTime <= b.EndTime)
                )
            );

            if (hasConflict)
                return BadRequest("Khung giờ này đã có người đặt.");

            var booking = new Booking
            {
                UserId = dto.UserId,
                FieldId = dto.FieldId,
                BookingDate = dto.BookingDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TotalPrice = dto.TotalPrice,
                Status = dto.Status ?? "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm đặt sân thành công.", booking });
        }

        //  PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBooking(int id, [FromBody] BookingUpdateDto dto)
        {
            var existing = await _context.Bookings.FindAsync(id);
            if (existing == null)
                return NotFound("Không tìm thấy đặt sân.");

            if (dto.StartTime != null && dto.EndTime != null && dto.StartTime >= dto.EndTime)
                return BadRequest("Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");

            // Cập nhật các trường
            existing.UserId = dto.UserId ?? existing.UserId;
            existing.FieldId = dto.FieldId ?? existing.FieldId;
            existing.BookingDate = dto.BookingDate ?? existing.BookingDate;
            existing.StartTime = dto.StartTime ?? existing.StartTime;
            existing.EndTime = dto.EndTime ?? existing.EndTime;
            existing.TotalPrice = dto.TotalPrice ?? existing.TotalPrice;
            existing.Status = dto.Status ?? existing.Status;

            //  Kiểm tra trùng giờ nếu có thay đổi
            var hasConflict = await _context.Bookings.AnyAsync(b =>
                b.BookingId != id &&
                b.FieldId == existing.FieldId &&
                b.BookingDate == existing.BookingDate &&
                b.Status != "Cancelled" &&
                (
                    (existing.StartTime >= b.StartTime && existing.StartTime < b.EndTime) ||
                    (existing.EndTime > b.StartTime && existing.EndTime <= b.EndTime)
                )
            );

            if (hasConflict)
                return BadRequest("Khung giờ này đã có người đặt.");

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật đặt sân thành công.", existing });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound("Không tìm thấy đặt sân.");

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt sân đã được hủy (xóa nguội)." });
        }
    }
}
