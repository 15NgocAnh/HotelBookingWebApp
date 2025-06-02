using HotelBooking.Application.CQRS.Booking.DTOs;

namespace HotelBooking.Application.CQRS.Booking.Commands.UpdateBooking;

public class UpdateBookingCommand : ICommand<Result>
{
    public int Id { get; set; }

    public int? RoomId { get; set; }

    public DateTime? CheckInDate { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public string? Notes { get; set; }

    public List<GuestDto> Guests { get; set; } = new();

    public List<ExtraUsageDto> ExtraUsages { get; set; } = new();
} 