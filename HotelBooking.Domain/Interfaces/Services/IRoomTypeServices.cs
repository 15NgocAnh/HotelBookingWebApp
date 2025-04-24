using HotelBooking.Data.Models;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IRoomTypeService
    {
        Task<ServiceResponse<List<RoomTypeModel>>> GetAllAsync();
    }
}
