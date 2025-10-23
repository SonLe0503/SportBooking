using AutoMapper;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDto>();

            // Map khi tạo mới
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // ✅ Bỏ qua ID
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
