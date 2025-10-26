using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly SportBookingDbContext _context;
        private readonly IMapper _mapper;
        public BookingController(SportBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BookingDTO>>(bookings));
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(_mapper.Map<BookingDTO>(booking));
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateBooking([FromBody] BookingCreateDto bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            booking.BookingDate = booking.BookingDate ?? DateTime.Now;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<BookingDTO>(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, createdDto);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, BookingDTO bookingDto)
        {
            if (id != bookingDto.BookingId)
                return BadRequest();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            _mapper.Map(bookingDto, booking);
            _context.Entry(booking).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
