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

        // SỬA 1: Đổi tên và kiểu dữ liệu từ bool? sang decimal?
        public decimal? FixedPrice { get; set; }

        public string? Link { get; set; }
        public IFormFile? ImageFile { get; set; } // Giữ nguyên để upload ảnh

        // SỬA 2: Thêm Avatar (nếu bạn gán Avatar qua URL)
        public string? Avatar { get; set; }

        // SỬA 3: Thêm CourtDetails
        public string? CourtDetails { get; set; }
    }
}
