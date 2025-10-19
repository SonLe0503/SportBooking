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
    }
}
