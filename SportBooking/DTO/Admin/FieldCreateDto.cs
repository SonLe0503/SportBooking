namespace SportBooking.DTO.Admin
{
    public class FieldCreateDto
    {
        public string FieldName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? OwnerId { get; set; }

        public string? Type { get; set; }
        public TimeOnly? OpenTime { get; set; }
        public TimeOnly? CloseTime { get; set; }
        public string? OpenDays { get; set; }
        public bool? IsFixedPrice { get; set; }
        public string? Link { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
