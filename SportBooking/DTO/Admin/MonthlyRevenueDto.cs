namespace SportBooking.DTO.Admin
{
    public class MonthlyRevenueDto
    {
        public string MonthLabel { get; set; } = string.Empty; // đổi từ Month -> MonthLabel
        public decimal TotalRevenue { get; set; }
    }
}
