namespace SportBooking.DTO.Admin
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; } = "USER";
    }
}
