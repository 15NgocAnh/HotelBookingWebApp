using HotelBooking.Data.Models;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Services.Interfaces
{
    public interface IRoomTypeService
    {
        Task<ServiceResponse<List<RoomTypeModel>>> GetAllAsync();
    }
}
