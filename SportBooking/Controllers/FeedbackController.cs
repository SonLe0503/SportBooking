using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly SportBookingDbContext _context;
        private readonly IMapper _mapper;
        public FeedbackController(SportBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Field)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks));
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Field)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);

            if (feedback == null)
                return NotFound();

            return Ok(_mapper.Map<FeedbackDTO>(feedback));
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<FeedbackDTO>> CreateFeedback(FeedbackDTO feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.CreatedAt = DateTime.Now;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<FeedbackDTO>(feedback);
            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.FeedbackId }, createdDto);
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, FeedbackDTO feedbackDto)
        {
            //if (id != feedbackDto.FeedbackId)
            //    return BadRequest();

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
                return NotFound();

            _mapper.Map(feedbackDto, feedback);
            _context.Entry(feedback).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
                return NotFound();

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
