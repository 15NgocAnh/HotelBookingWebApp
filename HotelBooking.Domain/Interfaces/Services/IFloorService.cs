using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IFloorService
    {
        Task<ServiceResponse<List<FloorDTO>>> GetAllAsync();
        Task<ServiceResponse<FloorDTO>> GetByIdAsync(int id);
        Task<ServiceResponse<FloorDTO>> CreateAsync(CreateFloorDTO createFloorDTO);
        Task<ServiceResponse<FloorDTO>> UpdateAsync(int id, UpdateFloorDTO updateFloorDTO);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}