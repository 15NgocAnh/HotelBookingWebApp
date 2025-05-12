using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Commands
{
    public record DeleteBedTypeCommand(int Id) : IRequest<Result>;
} 