using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(scr => scr.PhoneNumber))
            .ReverseMap();
    }
} 