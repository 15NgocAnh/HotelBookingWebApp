using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;

namespace HotelBooking.Application.Mappings
{
    public class RoomTypeMappingProfile : Profile
    {
        public RoomTypeMappingProfile()
        {
            CreateMap<RoomType, RoomTypeDto>().ReverseMap();
            CreateMap<BedTypeSetupDetail, BedTypeSetupDetailDto>().ReverseMap();
            CreateMap<AmenitySetupDetail, AmenitySetupDetailDto>().ReverseMap();
        }
    }
}
