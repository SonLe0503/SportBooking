using AutoMapper;
using SportBooking.DTO;
using SportBooking.Models;

namespace SportBooking.Mappers
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDTO>().ReverseMap();
        }
    }
}
