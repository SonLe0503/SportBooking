using System;

namespace SportBooking.DTO
{
    public class BookingUpdateDto
    {
        public int? UserId { get; set; }
        public int? FieldId { get; set; }
        public DateTime? BookingDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Status { get; set; }
    }
}
