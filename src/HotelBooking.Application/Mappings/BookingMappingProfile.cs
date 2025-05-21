using AutoMapper;
using HotelBooking.Application.CQRS.Booking.Commands.CreateBooking;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Domain.AggregateModels.BookingAggregate;

namespace HotelBooking.Application.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingDto>();
        CreateMap<CreateBookingCommand, Booking>();
    }
} 