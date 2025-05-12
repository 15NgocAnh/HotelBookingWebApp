using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Commands
{
    public record DeleteAmenityCommand(int Id) : IRequest<Result>;
} 