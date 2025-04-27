using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Floor)
                .Include(r => r.RoomType)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Floor)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Room>> GetByFloorIdAsync(int floorId)
        {
            return await _context.Rooms
                .Include(r => r.Floor)
                .Include(r => r.RoomType)
                .Where(r => r.FloorId == floorId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByRoomTypeIdAsync(int roomTypeId)
        {
            return await _context.Rooms
                .Include(r => r.Floor)
                .Include(r => r.RoomType)
                .Where(r => r.RoomTypeId == roomTypeId && !r.IsDeleted)
                .ToListAsync();
        }
    }
}
