using HotelBooking.Application.CQRS.RoomType.DTOs;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public record CreateRoomTypeCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
        public decimal Price { get; init; }
        public List<BedTypeSetupDetailDto> BedTypeSetupDetails { get; set; } = new List<BedTypeSetupDetailDto>();
        public List<AmenitySetupDetailDto> AmenitySetupDetails { get; set; } = new List<AmenitySetupDetailDto>();
    }
} 