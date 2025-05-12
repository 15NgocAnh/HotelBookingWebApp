using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public record UpdateRoomTypeCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public List<BedTypeSetupDetailDto> BedTypeSetupDetails { get; init; }
        public List<AmenitySetupDetailDto> AmenitySetupDetails { get; init; }
    }
} 