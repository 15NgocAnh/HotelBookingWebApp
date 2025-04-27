using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Infrastructure.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IMapper _mapper;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomTypeService(IMapper mapper, IRoomTypeRepository roomTypeRepository)
        {
            _mapper = mapper;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<ServiceResponse<List<RoomTypeDTO>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<RoomTypeDTO>>();
            var roomTypes = await _roomTypeRepository.GetAllAsync();
            
            if (roomTypes != null && roomTypes.Any())
            {
                serviceResponse.Data = roomTypes.Select(rt => new RoomTypeDTO
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    Description = rt.Description,
                    NumberOfAdults = rt.NumberOfAdults,
                    NumberOfChildrent = rt.NumberOfChildrent,
                    RoomTypeSymbol = rt.RoomTypeSymbol,
                }).ToList();
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get all room types successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No room types found.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomTypeDTO>> GetByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<RoomTypeDTO>();
            var roomType = await _roomTypeRepository.GetByIdAsync(id);
            
            if (roomType == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Room type not found.";
                return serviceResponse;
            }

            serviceResponse.Data = new RoomTypeDTO
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Description = roomType.Description,
                NumberOfAdults = roomType.NumberOfAdults,
                NumberOfChildrent = roomType.NumberOfChildrent,
                RoomTypeSymbol = roomType.RoomTypeSymbol,
            };
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Get room type successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomTypeDTO>> CreateAsync(CreateRoomTypeDTO createRoomTypeDTO)
        {
            var serviceResponse = new ServiceResponse<RoomTypeDTO>();
            try
            {
                var roomType = _mapper.Map<RoomType>(createRoomTypeDTO);

                await _roomTypeRepository.AddAsync(roomType);
                serviceResponse.Data = _mapper.Map<RoomTypeDTO>(roomType);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Create room type successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoomTypeDTO>> UpdateAsync(int id, UpdateRoomTypeDTO updateRoomTypeDTO)
        {
            var serviceResponse = new ServiceResponse<RoomTypeDTO>();
            var roomType = await _roomTypeRepository.GetByIdAsync(id);
            
            if (roomType == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Room type not found.";
                return serviceResponse;
            }

            try
            {
                roomType.Name = updateRoomTypeDTO.Name;
                roomType.Description = updateRoomTypeDTO.Description;
                roomType.RoomTypeSymbol = updateRoomTypeDTO.RoomTypeSymbol;
                roomType.NumberOfAdults = updateRoomTypeDTO.NumberOfAdults;
                roomType.NumberOfChildrent = updateRoomTypeDTO.NumberOfChildrent;

                await _roomTypeRepository.UpdateAsync(roomType);
                serviceResponse.Data = _mapper.Map<RoomTypeDTO>(roomType);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Update room type successfully!";
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
            try
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(id);

                if (roomType == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Room type not found.";
                    return serviceResponse;
                }

                await _roomTypeRepository.SoftDeleteAsync(roomType);
                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Delete room type successfully!";
                return serviceResponse;
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
                return serviceResponse;
            }
        }
    }
} 