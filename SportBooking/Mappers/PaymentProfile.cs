using AutoMapper;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentCreateDto, Payment>();
            CreateMap<PaymentDTO, Payment>().ReverseMap();
        }
    }
}
