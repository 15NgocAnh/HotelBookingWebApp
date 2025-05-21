using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Application.Mappings;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleDto>()
            .ReverseMap();
    }
} 