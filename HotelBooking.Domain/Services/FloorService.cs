using AutoMapper;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Infrastructure.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IMapper _mapper;

        public FloorService(IFloorRepository floorRepository, IMapper mapper)
        {
            _floorRepository = floorRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<FloorDTO>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<FloorDTO>>();
            try
            {
                var floors = await _floorRepository.GetAllAsync();

                if (floors != null && floors.Any())
                {
                    serviceResponse.Data = _mapper.Map<List<FloorDTO>>(floors);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Get all floors successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "No floors found.";
                }
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<FloorDTO>> GetByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<FloorDTO>();
            var floor = await _floorRepository.GetByIdAsync(id);
            
            if (floor == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Floor not found.";
                return serviceResponse;
            }

            serviceResponse.Data = _mapper.Map<FloorDTO>(floor);
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Get floor successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<FloorDTO>> CreateAsync(CreateFloorDTO createFloorDTO)
        {
            var serviceResponse = new ServiceResponse<FloorDTO>();
            try
            {
                var floor = _mapper.Map<Floor>(createFloorDTO);

                await _floorRepository.AddAsync(floor);
                serviceResponse.Data = _mapper.Map<FloorDTO>(floor);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Create floor successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<FloorDTO>> UpdateAsync(int id, UpdateFloorDTO updateFloorDTO)
        {
            var serviceResponse = new ServiceResponse<FloorDTO>();
            var floor = await _floorRepository.GetByIdAsync(id);
            
            if (floor == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Floor not found.";
                return serviceResponse;
            }

            try
            {
                floor.Name = updateFloorDTO.Name;
                floor.Description = updateFloorDTO.Description;
                floor.IsActive = updateFloorDTO.IsActive;

                await _floorRepository.UpdateAsync(floor);
                serviceResponse.Data = _mapper.Map<FloorDTO>(floor);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Update floor successfully!";
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
                var floor = await _floorRepository.GetByIdAsync(id);

                if (floor == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Floor not found.";
                    return serviceResponse;
                }

                await _floorRepository.SoftDeleteAsync(floor);

                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Delete floor successfully!";
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