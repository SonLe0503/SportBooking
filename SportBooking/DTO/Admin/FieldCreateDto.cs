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
        public decimal? FixedPrice { get; set; } // Giữ nguyên (đã sửa)
        public string? Link { get; set; }
        public string? CourtDetails { get; set; } // Giữ nguyên (đã sửa)

        // Giữ nguyên ImageFile
        public IFormFile? ImageFile { get; set; }

        // SỬA LẠI THEO YÊU CẦU MỚI:
        // Đổi từ string? Avatar thành IFormFile? AvatarFile
        public IFormFile? AvatarFile { get; set; }
    }
}
