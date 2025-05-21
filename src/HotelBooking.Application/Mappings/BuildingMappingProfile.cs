using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Domain.AggregateModels.BuildingAggregate;

namespace HotelBooking.Application.Mappings;
public class BuildingMappingProfile : Profile
{
    public BuildingMappingProfile()
    {
        CreateMap<Building, BuildingDto>().ReverseMap();
        CreateMap<Floor, FloorDto>().ReverseMap();
    }
}
