using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Domain.Response;
using HotelBooking.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Domain.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository, IMapper mapper, ILogger<RoomService> logger)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<RoomDetailsDTO>> SaveAsync(RoomDetailsDTO newRoom)
        {
            var serviceResponse = new ServiceResponse<RoomDetailsDTO>();
            try
            {
                // Check if room number already exists
                var existingRoom = await _roomRepository.GetAllRooms()
                    .FirstOrDefaultAsync(r => r.RoomNumber == newRoom.RoomNo);

                if (existingRoom != null)
                {
                    throw new InvalidOperationException("Room number already exists");
                }
                var room = _mapper.Map<RoomModel>(newRoom);
                var roomType = await _roomTypeRepository.GetByIdAsync(newRoom.RoomType);
                room.RoomType = roomType ?? new();
                await _roomRepository.AddAsync(room);
                serviceResponse.Data = newRoom;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Add Room successfully";
            }
            catch (InvalidOperationException)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = "An error occurred while adding a room.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomDetailsDTO>> FindByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<RoomDetailsDTO>();
            try
            {
                serviceResponse.Data = await _mapper.ProjectTo<RoomDetailsDTO>(_roomRepository.GetAllQueryable())
                    .AsNoTracking()
                    .FirstAsync(c => c.Id == id);
                serviceResponse.ResponseType = EResponseType.Success;
            }
            catch (InvalidOperationException)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Room not found.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomDetailsDTO>> UpdateAsync(int id, RoomDetailsDTO roomDTO)
        {
            var serviceResponse = new ServiceResponse<RoomDetailsDTO>();
            try
            {
                var room = await _roomRepository.GetAllQueryable().FirstOrDefaultAsync(c => c.Id == id);
                if (room != null)
                {
                    room = await _roomRepository.UpdateRoomAsync(id, roomDTO);
                    serviceResponse.Data = roomDTO;
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Update Room successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Room not found";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = CJConstant.SOMETHING_WENT_WRONG;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var room = await _roomRepository.GetByIdAsync(id);
                if (room != null)
                {
                    await _roomRepository.SoftDeleteAsync(room);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Delete Room successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Room deletion failed. The Room has been deleted.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = CJConstant.SOMETHING_WENT_WRONG;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> UndoDeletedAsync(int id)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var room = await _roomRepository.GetByIdAsync(id);
                if (room != null)
                {
                    if (room.IsDeleted)
                    {
                        await _roomRepository.RestoreDeletedRoomAsync(room);
                        serviceResponse.ResponseType = EResponseType.Success;
                        serviceResponse.Message = "Room has been undo delete successfully.";
                    }
                    else
                    {
                        serviceResponse.ResponseType = EResponseType.BadRequest;
                        serviceResponse.Message = "The Room has not been deleted.";
                    }
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Room not found";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = CJConstant.SOMETHING_WENT_WRONG;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<RoomDTO>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<RoomDTO>>();
            try
            {
                var rooms = await _roomRepository.GetAllQueryable().ToListAsync();
                if (rooms != null && rooms.Count > 0)
                {
                    serviceResponse.Data = _mapper.Map<List<RoomDTO>>(rooms);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Retrieved rooms successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "No rooms found.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = CJConstant.SOMETHING_WENT_WRONG;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<RoomDTO>>> SearchRoomsAsync(RoomCondition condition)
        {
            var serviceResponse = new ServiceResponse<List<RoomDTO>>();
            try
            {
                var query = _roomRepository.GetAllQueryable()
                    .Where(r => r.Status == RoomStatus.Available);

                // Filter by room type if specified
                if (condition.RoomType > 0)
                {
                    query = query.Where(r => r.RoomTypeId == condition.RoomType);
                }

                // Filter by maximum occupancy
                var totalGuests = condition.AdultsCnt + condition.ChildrenCnt;
                query = query.Where(r => r.MaxOccupancy >= totalGuests);

                // Filter by availability dates
                // Note: This is a simple check. In a real application, you would need to check against actual bookings
                query = query.Where(r => r.LastBookedDate == null || 
                    r.LastBookedDate.Value.AddDays(1) <= condition.CheckInDate);

                var rooms = await query.ToListAsync();
                
                if (rooms != null && rooms.Count > 0)
                {
                    serviceResponse.Data = _mapper.Map<List<RoomDTO>>(rooms);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Found available rooms matching your criteria!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "No rooms found matching your criteria.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching rooms");
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = CJConstant.SOMETHING_WENT_WRONG;
            }
            return serviceResponse;
        }
    }
}
