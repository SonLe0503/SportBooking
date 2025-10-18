using AutoMapper;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile() { 
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.Password,opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<User, RegisterRequest>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        }
    }
}
