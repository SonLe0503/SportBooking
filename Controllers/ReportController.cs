using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly SportBookingDbContext _context;

        public ReportController(SportBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/report/facilityOwner
        [HttpGet("facilityOwner")]
        public async Task<IActionResult> GetReportsForFacilityOwner()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Field)
                .Select(f => new
                {
                    Id = f.FeedbackId,
                    Name = f.User != null ? f.User.Username : "Ẩn danh",
                    NameField = f.Field != null ? f.Field.FieldName : "Không xác định",
                    Description = f.Comment,
                    Rating = f.Rating
                })
                .ToListAsync();

            return Ok(feedbacks);
        }
    }
}
