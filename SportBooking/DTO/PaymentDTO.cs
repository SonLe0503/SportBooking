﻿namespace SportBooking.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int? BookingId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
    }
    public class PaymentCreateDto
    {
        public int? BookingId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
    }
}
