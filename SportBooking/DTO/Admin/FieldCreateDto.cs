namespace SportBooking.DTO.Admin
{
    public class FieldCreateDto
    {
        public string FieldName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int OwnerId { get; set; }

        // Đây là phần quan trọng cho upload ảnh
        public IFormFile? ImageFile { get; set; }
    }
}
