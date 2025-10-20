using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBooking.Models;

namespace SportBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly SportBookingDbContext _context;

        public DashboardController(SportBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("facilityOwner")]
        public async Task<IActionResult> GetDashboardData()
        {
            // Lấy dữ liệu 12 tháng gần nhất
            var currentYear = DateTime.Now.Year;
            var prevYear = currentYear - 1;

            //   Dữ liệu hiện tại (năm nay) 
            var currentIncome = await _context.Payments
                .Where(p => p.PaymentDate.HasValue && p.PaymentDate.Value.Year == currentYear && p.Status == "Success")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var currentProfit = currentIncome * 0.25m; // giả sử 25% là lợi nhuận
            var totalField = await _context.Fields.CountAsync();
            var totalUser = await _context.Users.CountAsync();

            //   Dữ liệu năm trước 
            var prevIncome = await _context.Payments
                .Where(p => p.PaymentDate.HasValue && p.PaymentDate.Value.Year == prevYear && p.Status == "Success")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var prevProfit = prevIncome * 0.25m;
            var prevTotalField = await _context.Fields.CountAsync(); 
            var prevTotalUser = await _context.Users.CountAsync();   

            //  3 Biểu đồ doanh thu theo tháng (dataLine) 
            var monthlyData = await _context.Payments
                .Where(p => p.PaymentDate.HasValue && p.PaymentDate.Value.Year == currentYear && p.Status == "Success")
                .GroupBy(p => p.PaymentDate.Value.Month)
                .Select(g => new
                {
                    month = g.Key,
                    value = g.Sum(p => p.Amount)
                })
                .OrderBy(x => x.month)
                .ToListAsync();

            // Bổ sung tháng chưa có giao dịch
            var fullMonths = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    month = m.ToString(),
                    value = monthlyData.FirstOrDefault(x => x.month == m)?.value ?? 0
                })
                .ToList();
            //  Biểu đồ số lượng user theo tháng (userLine)
            var userMonthlyData = await _context.Users
                .Where(u => u.CreatedAt.HasValue && u.CreatedAt.Value.Year == currentYear)
                .GroupBy(u => u.CreatedAt.Value.Month)
                .Select(g => new
                {
                    month = g.Key,
                    count = g.Count()
                })
                .OrderBy(x => x.month)
                .ToListAsync();

            var fullUserMonths = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    month = m.ToString(),
                    count = userMonthlyData.FirstOrDefault(x => x.month == m)?.count ?? 0
                })
                .ToList();
            //  Biểu đồ số lượng đơn đặt sân theo tháng (bookingLine) 
            var bookingMonthlyData = await _context.Bookings
                .Where(b => b.BookingDate.HasValue && b.BookingDate.Value.Year == currentYear)
                .GroupBy(b => b.BookingDate.Value.Month)
                .Select(g => new
                {
                    month = g.Key,
                    count = g.Count()
                })
                .OrderBy(x => x.month)
                .ToListAsync();

            var fullBookingMonths = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    month = m.ToString(),
                    count = bookingMonthlyData.FirstOrDefault(x => x.month == m)?.count ?? 0
                })
                .ToList();
            //  dữ liệu trả về ===
            var result = new
            {
                currentData = new
                {
                    income = currentIncome,
                    profit = currentProfit,
                    totalField,
                    totalUser
                },
                prevData = new
                {
                    income = prevIncome,
                    profit = prevProfit,
                    totalField = prevTotalField,
                    totalUser = prevTotalUser
                },
                dataLine = fullMonths,
                userLine = fullUserMonths,
                bookingLine = fullBookingMonths
            };

            return Ok(result);
        }
    }
}
