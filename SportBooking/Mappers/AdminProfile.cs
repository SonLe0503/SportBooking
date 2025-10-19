using AutoMapper;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class AdminProfile : Profile
    {
        public AdminProfile() {
            CreateMap<Field, FieldsDto>().ReverseMap();

            // Ánh xạ khi tạo sân
            CreateMap<FieldCreateDto, Field>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}

