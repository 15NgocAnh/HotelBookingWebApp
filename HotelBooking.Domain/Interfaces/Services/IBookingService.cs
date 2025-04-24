using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingDTO> CreateBookingAsync(BookingDTO bookingDTO);
        Task<BookingDTO> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetBookingsByGuestIdAsync(int guestId);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus status);
    }
}