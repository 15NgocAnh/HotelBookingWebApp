using AutoMapper;
using HotelBooking.Application.CQRS.Auth.DTOs;
using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, LoginResponseDto>().ReverseMap();
    }
} 