namespace SportBooking.DTO.Admin
{
    public class FieldsDto
    {
        public int FieldId { get; set; }
        public string? FieldName { get; set; }
        public string? Location { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? OwnerId { get; set; }

        public string? Type { get; set; }
        public TimeOnly? OpenTime { get; set; }
        public TimeOnly? CloseTime { get; set; }
        public string? OpenDays { get; set; }

        // SỬA ĐỔI 1: Đổi từ bool? IsFixedPrice thành decimal? FixedPrice
        public decimal? FixedPrice { get; set; }

        public string? Link { get; set; }

        // SỬA ĐỔI 2: Thêm Avatar
        public string? Avatar { get; set; }

        // SỬA ĐỔI 3: Thêm CourtDetails
        public string? CourtDetails { get; set; }
    }
}
