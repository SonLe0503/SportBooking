using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly SportBookingDbContext _context;
        private readonly IMapper _mapper;

        public PaymentController(SportBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Payment/GetPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments()
        {
            var payments = await _context.Payments.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PaymentDTO>>(payments));
        }

        // GET: api/Payment/GetPayment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            return Ok(_mapper.Map<PaymentDTO>(payment));
        }

        // POST: api/Payment/CreatePayment
        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> CreatePayment(PaymentCreateDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            payment.PaymentDate ??= DateTime.Now; // nếu null thì gán ngày hiện tại

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<PaymentDTO>(payment);
            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, createdDto);
        }

        // PUT: api/Payment/UpdatePayment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentDTO paymentDto)
        {
            if (id != paymentDto.PaymentId)
                return BadRequest("ID không khớp");

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            _mapper.Map(paymentDto, payment);
            _context.Entry(payment).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Payment/DeletePayment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
