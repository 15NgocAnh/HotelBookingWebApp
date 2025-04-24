using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Room;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<RoomModel>
    {
        IQueryable<RoomModel> GetAllRooms();
        IQueryable<RoomDTO> GetRooms();
        Task<RoomModel> UpdateRoomAsync(int id, RoomDetailsDTO roomDTO);
        Task RestoreDeletedRoomAsync(RoomModel room);
    }
}
