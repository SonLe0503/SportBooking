using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly SportBookingDbContext _context;

        public DashboardController(SportBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var data = await _context.Bookings
                .Where(b => b.BookingDate != null && b.TotalPrice != null)
                .GroupBy(b => new
                {
                    Year = b.BookingDate.Value.Year,
                    Month = b.BookingDate.Value.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Revenue = g.Sum(e => e.TotalPrice ?? 0)
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            // 👇 Format lại dữ liệu sau khi EF đã truy vấn xong
            var result = data.Select(d => new MonthlyRevenueDto
            {
                MonthLabel = $"{d.Month}/{d.Year}", // xử lý ở C#
                TotalRevenue = d.Revenue
            }).ToList();

            return Ok(result);
        }

    }

}
