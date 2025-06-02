using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Domain.AggregateModels.HotelAggregate;

namespace HotelBooking.Application.Mappings
{
    public class HotelMappingProfile : Profile
    {
        public HotelMappingProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
