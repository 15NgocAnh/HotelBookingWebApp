using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Services.Interfaces
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<RoomDTO>>> GetAllAsync();
        Task<ServiceResponse<RoomDetailsDTO>> SaveAsync(RoomDetailsDTO newRoom);
        Task<ServiceResponse<RoomDetailsDTO>> FindByIdAsync(int id);
        Task<ServiceResponse<RoomDetailsDTO>> UpdateAsync(int id, RoomDetailsDTO room);
        Task<ServiceResponse<object>> DeleteAsync(int id);
        Task<ServiceResponse<object>> UndoDeletedAsync(int id);
        Task<ServiceResponse<List<RoomDTO>>> SearchRoomsAsync(RoomCondition condition);
    }
}
