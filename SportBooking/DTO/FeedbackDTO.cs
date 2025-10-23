namespace SportBooking.DTO
{
    public class FeedbackDTO
    {
        public int FeedbackId { get; set; }
        public int? UserId { get; set; }
        public int? FieldId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
