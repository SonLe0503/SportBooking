namespace SportBooking.DTO
{
    public class ImageFieldDTO
    {
        public int ImageId { get; set; }
        public int FieldId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
    public class ImageFieldCreateDto
    {
        public int FieldId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
