using HotelBooking.Domain.DTOs.Guest;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IGuestService
    {
        Task<ServiceResponse<GuestDTO>> GetGuestByIdAsync(int id);
        Task<ServiceResponse<GuestDTO>> GetGuestByIdentityNumberAsync(string identityNumber);
        Task<ServiceResponse<List<GuestDTO>>> GetAllGuestsAsync();
        Task<ServiceResponse<GuestDTO>> CreateGuestAsync(CreateGuestDTO guestDTO);
        Task<ServiceResponse<GuestDTO>> UpdateGuestAsync(int id, CreateGuestDTO guestDTO);
        Task<ServiceResponse<bool>> DeleteGuestAsync(int id);
    }
} 