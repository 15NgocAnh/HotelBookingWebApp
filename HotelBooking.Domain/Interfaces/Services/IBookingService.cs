using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IBookingService
    {
        Task<ServiceResponse<BookingDTO>> CreateBookingAsync(CreateBookingDTO bookingDTO);
        Task<ServiceResponse<BookingDTO>> GetBookingByIdAsync(int bookingId);
        Task<ServiceResponse<List<BookingDTO>>> GetBookingsByGuestIdAsync(int guestId);
        Task<ServiceResponse<bool>> CancelBookingAsync(int bookingId);
        Task<ServiceResponse<bool>> UpdateBookingStatusAsync(int bookingId, BookingStatus status);
        Task<ServiceResponse<bool>> CheckInAsync(int bookingId);
        Task<ServiceResponse<bool>> CheckOutAsync(int bookingId);
    }
}