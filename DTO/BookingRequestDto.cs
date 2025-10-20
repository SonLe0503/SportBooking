namespace SportBooking.DTO
{
    public class BookingRequestDto
    {
        public string FieldName { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public int TotalHour { get; set; }
        public decimal TotalPrice { get; set; }
        public string NameCustomer { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
