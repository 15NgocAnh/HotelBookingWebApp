using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IRoomTypeService
    {
        Task<ServiceResponse<List<RoomTypeDTO>>> GetAllAsync();
        Task<ServiceResponse<RoomTypeDTO>> GetByIdAsync(int id);
        Task<ServiceResponse<RoomTypeDTO>> CreateAsync(CreateRoomTypeDTO createRoomTypeDTO);
        Task<ServiceResponse<RoomTypeDTO>> UpdateAsync(int id, UpdateRoomTypeDTO updateRoomTypeDTO);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}