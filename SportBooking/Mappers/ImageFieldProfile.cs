using AutoMapper;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class ImageFieldProfile : Profile
    {
        public ImageFieldProfile()
        {
            CreateMap<ImageField, ImageFieldDTO>().ReverseMap();
        }
    }
}
