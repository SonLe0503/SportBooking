namespace SportBooking.DTO.Admin
{
    public class UserDto
    {
        public int UserId { get; set; }

        // SỬA 1: Khớp với Model (cho phép null)
        public string? Username { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }

        // SỬA 2: Khớp với Model (cho phép null)
        public DateTime? CreatedAt { get; set; }

        // SỬA 3: Thêm trường Avatar
        public string? Avatar { get; set; }
    }
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; } = "USER";

        // SỬA 4: Thêm trường Avatar (nếu Admin được set lúc tạo)
        public string? Avatar { get; set; }
    }
}
