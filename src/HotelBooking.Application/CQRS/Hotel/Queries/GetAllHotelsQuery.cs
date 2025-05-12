using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Queries
{
    public record GetAllHotelsQuery : IRequest<Result<List<HotelDto>>>;
} 