using System.ComponentModel.DataAnnotations;

namespace SportBooking.DTO
{
    public class AvatarUploadDTO
    {
        [Required] // Bắt buộc phải có file
        public IFormFile AvatarFile { get; set; }
    }
}
