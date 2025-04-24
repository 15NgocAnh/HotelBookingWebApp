using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repository
{
    public class RoomRepository : GenericRepository<RoomModel>, IRoomRepository
    {
        public RoomRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IQueryable<RoomModel> GetAllRooms() =>
            _context.Rooms.Where(p => !p.IsDeleted);

        public IQueryable<RoomDTO> GetRooms()
        {
            return _context.Rooms
                .Where(p => !p.IsDeleted)
                .Include(r => r.RoomType)
                .Select(r => new RoomDTO
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType.Name,
                    PricePerNight = r.PricePerNight,
                    Status = r.Status.ToString(),
                    Area = r.Area,
                    ImageUrl = r.ImageUrl,
                    MaxOccupancy = r.MaxOccupancy,
                    BedCount = r.BedCount,
                    Facilities = r.Facilities,
                    Description = r.Description
                });
        }

        public async Task<RoomModel> UpdateRoomAsync(int id, RoomDetailsDTO roomDTO)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room != null)
                {
                    _mapper.Map(roomDTO, room);
                    room.RoomType = null;
                    await _context.SaveChangesAsync();
                }
                return room;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating room: {ex.Message}");
                throw;
            }
        }

        public async Task RestoreDeletedRoomAsync(RoomModel room)
        {
            room.IsDeleted = false;
            await _context.SaveChangesAsync();
        }
    }
}
