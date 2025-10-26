using AutoMapper;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingCreateDto, Booking>();
            CreateMap<Booking, BookingDTO>().ReverseMap();
        }
    }
}
