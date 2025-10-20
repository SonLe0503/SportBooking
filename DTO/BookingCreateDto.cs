using System;
using System.ComponentModel.DataAnnotations;

namespace SportBooking.DTO
{
    public class BookingCreateDto
    {
        [Required(ErrorMessage = "Mã người dùng là bắt buộc.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Mã sân là bắt buộc.")]
        public int FieldId { get; set; }

        [Required(ErrorMessage = "Ngày đặt là bắt buộc.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Giờ bắt đầu là bắt buộc.")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "Giờ kết thúc là bắt buộc.")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền không hợp lệ.")]
        public decimal TotalPrice { get; set; }

        public string? Status { get; set; }
    }
}
