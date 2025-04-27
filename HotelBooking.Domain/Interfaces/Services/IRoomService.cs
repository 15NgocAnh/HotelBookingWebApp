using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<RoomDTO>>> GetAllAsync();
        Task<ServiceResponse<RoomDTO>> GetByIdAsync(int id);
        Task<ServiceResponse<RoomDTO>> CreateAsync(CreateRoomDTO createRoomDTO);
        Task<ServiceResponse<RoomDTO>> UpdateAsync(int id, UpdateRoomDTO updateRoomDTO);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<List<RoomDTO>>> GetByFloorIdAsync(int floorId);
        Task<ServiceResponse<List<RoomDTO>>> GetByRoomTypeIdAsync(int roomTypeId);
    }
}