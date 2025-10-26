using AutoMapper;
using SportBooking.DTO.Admin;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class AdminProfile : Profile
    {
        public AdminProfile() {
            CreateMap<Field, FieldsDto>().ReverseMap();

            CreateMap<FieldCreateDto, Field>();
        }
    }
}

