namespace SportBooking.DTO.Admin
{
    public class TopFieldDto
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
