using AutoMapper;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class AdminProfile : Profile
    {
        public AdminProfile() {
            CreateMap<Field, FieldsDto>().ReverseMap();

            // SỬA ĐỔI QUAN TRỌNG:
            CreateMap<FieldCreateDto, Field>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}

