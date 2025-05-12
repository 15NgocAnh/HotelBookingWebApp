using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public record DeleteExtraCategoryCommand(int Id) : IRequest<Result>;
} 