using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly SportBookingDbContext _context;

        public ReportsController(SportBookingDbContext context)
        {
            _context = context;
        }

        // Báo cáo doanh thu theo tháng
        [HttpGet("MonthlyRevenue")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            var monthlyRevenue = await _context.Bookings
    .Where(b => b.BookingDate != null && b.TotalPrice != null)
    .GroupBy(b => new {
        Year = b.BookingDate.Value.Year,
        Month = b.BookingDate.Value.Month
    })
    .Select(g => new MonthlyRevenueDto
    {
        MonthLabel = g.Key.Month + "/" + g.Key.Year,
        TotalRevenue = g.Sum(b => b.TotalPrice ?? 0)
    })
    .OrderBy(x => x.MonthLabel)
    .ToListAsync();

            return Ok(monthlyRevenue);
        }


        // Báo cáo tổng quan: tổng doanh thu, số sân, số user
        [HttpGet("Summary")]
        public async Task<IActionResult> GetSummary()
        {
            var totalRevenue = await _context.Bookings.SumAsync(b => b.TotalPrice ?? 0);
            var totalBookings = await _context.Bookings.CountAsync();
            var totalFields = await _context.Fields.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            var summary = new
            {
                TotalRevenue = totalRevenue,
                TotalBookings = totalBookings,
                TotalFields = totalFields,
                TotalUsers = totalUsers
            };

            return Ok(summary);
        }

        // Top sân có doanh thu cao nhất
        [HttpGet("TopFields")]
        public async Task<IActionResult> GetTopFields()
        {
            var data = await _context.Bookings
     .Include(b => b.Field)
     .Where(b => b.TotalPrice != null)
     .GroupBy(b => new { b.FieldId, b.Field.FieldName, b.Field.Image })
     .Select(g => new TopFieldDto
     {
         FieldName = g.Key.FieldName,
         Image = g.Key.Image,
         TotalRevenue = g.Sum(x => x.TotalPrice ?? 0)
     })
     .OrderByDescending(x => x.TotalRevenue)
     .Take(5)
     .ToListAsync();


            return Ok(data);
        }
    }

}
