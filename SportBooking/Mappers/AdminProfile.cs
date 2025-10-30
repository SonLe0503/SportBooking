using AutoMapper;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class AdminProfile : Profile
    {
        public AdminProfile() {
            CreateMap<Field, FieldsDto>().ReverseMap();

            CreateMap<FieldCreateDto, Field>()
                // Đã có từ trước (vì ImageFile cần xử lý thủ công)
                .ForMember(dest => dest.Image, opt => opt.Ignore())

                // SỬA LẠI THEO YÊU CẦU MỚI:
                // Thêm dòng này để bỏ qua AvatarFile
                .ForMember(dest => dest.Avatar, opt => opt.Ignore());
        }
    }
}

