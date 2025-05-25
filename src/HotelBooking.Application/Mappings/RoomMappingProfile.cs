using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Application.Mappings
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDto>().ReverseMap();
        }
    }
}
