namespace SportBooking.DTO.Admin
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalFields { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<MonthlyRevenueDto> MonthlyRevenues { get; set; } = new();
    }

}

