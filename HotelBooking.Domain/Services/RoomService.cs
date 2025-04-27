using AutoMapper;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public RoomService(
            IRoomRepository roomRepository,
            IFloorRepository floorRepository,
            IRoomTypeRepository roomTypeRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _floorRepository = floorRepository;
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<RoomDTO>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<RoomDTO>>();
            var rooms = await _roomRepository.GetAllAsync();
            
            if (rooms != null && rooms.Any())
            {
                serviceResponse.Data = await MapRoomsToDTOs(rooms);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get all rooms successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No rooms found.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomDTO>> GetByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<RoomDTO>();
            var room = await _roomRepository.GetByIdAsync(id);
            
            if (room == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Room not found.";
                return serviceResponse;
            }

            serviceResponse.Data = await MapRoomToDTO(room);
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Get room successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomDTO>> CreateAsync(CreateRoomDTO createRoomDTO)
        {
            var serviceResponse = new ServiceResponse<RoomDTO>();
            try
            {
                // Validate floor and room type exist
                var floor = await _floorRepository.GetByIdAsync(createRoomDTO.FloorId);
                var roomType = await _roomTypeRepository.GetByIdAsync(createRoomDTO.RoomTypeId);

                if (floor == null || roomType == null)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Invalid floor or room type";
                    return serviceResponse;
                }

                var room = _mapper.Map<Room>(createRoomDTO);

                await _roomRepository.AddAsync(room);
                serviceResponse.Data = _mapper.Map<RoomDTO>(createRoomDTO);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Create room successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomDTO>> UpdateAsync(int id, UpdateRoomDTO updateRoomDTO)
        {
            var serviceResponse = new ServiceResponse<RoomDTO>();
            var room = await _roomRepository.GetByIdAsync(id);
            
            if (room == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Room not found.";
                return serviceResponse;
            }

            try
            {
                // Validate floor and room type exist
                var floor = await _floorRepository.GetByIdAsync(updateRoomDTO.FloorId);
                var roomType = await _roomTypeRepository.GetByIdAsync(updateRoomDTO.RoomTypeId);

                if (floor == null || roomType == null)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Invalid floor or room type";
                    return serviceResponse;
                }

                room.RoomNumber = updateRoomDTO.RoomNumber;
                room.FloorId = updateRoomDTO.FloorId;
                room.RoomTypeId = updateRoomDTO.RoomTypeId;
                room.IsActive = updateRoomDTO.IsActive;

                await _roomRepository.UpdateAsync(room);
                serviceResponse.Data = _mapper.Map<RoomDTO>(updateRoomDTO);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Update room successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<bool>();

            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Room not found.";
                return serviceResponse;
            }

            await _roomRepository.SoftDeleteAsync(room);

            serviceResponse.Data = true;
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Delete room successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<RoomDTO>>> GetByFloorIdAsync(int floorId)
        {
            var serviceResponse = new ServiceResponse<List<RoomDTO>>();
            var rooms = await _roomRepository.GetByFloorIdAsync(floorId);
            
            if (rooms != null && rooms.Any())
            {
                serviceResponse.Data = await MapRoomsToDTOs(rooms);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get rooms by floor successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No rooms found for this floor.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<RoomDTO>>> GetByRoomTypeIdAsync(int roomTypeId)
        {
            var serviceResponse = new ServiceResponse<List<RoomDTO>>();
            var rooms = await _roomRepository.GetByRoomTypeIdAsync(roomTypeId);
            
            if (rooms != null && rooms.Any())
            {
                serviceResponse.Data = await MapRoomsToDTOs(rooms);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get rooms by room type successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No rooms found for this room type.";
            }
            return serviceResponse;
        }

        private async Task<RoomDTO> MapRoomToDTO(Room room)
        {
            var floor = await _floorRepository.GetByIdAsync(room.FloorId);
            var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeId);

            var result = _mapper.Map<RoomDTO>(room);
            result.FloorName = floor?.Name;
            result.RoomTypeName = roomType?.Name;
            result.Status = room.Status.ToString();

            return result;
        }

        private async Task<List<RoomDTO>> MapRoomsToDTOs(IEnumerable<Room> rooms)
        {
            var roomDTOs = new List<RoomDTO>();
            foreach (var room in rooms)
            {
                roomDTOs.Add(await MapRoomToDTO(room));
            }
            return roomDTOs;
        }
    }
} 