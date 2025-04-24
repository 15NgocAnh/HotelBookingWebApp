using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Domain.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomTypeService> _logger;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository, IMapper mapper, ILogger<RoomTypeService> logger) 
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<RoomTypeModel>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<RoomTypeModel>>();
            try
            {
                _logger.LogInformation("Attempting to retrieve all room types");
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                
                if (roomTypes == null || !roomTypes.Any())
                {
                    _logger.LogWarning("No room types found in the database");
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "No room types found";
                    return serviceResponse;
                }

                serviceResponse.Data = _mapper.Map<List<RoomTypeModel>>(roomTypes);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Successfully retrieved all room types";
                _logger.LogInformation($"Successfully retrieved {roomTypes.Count()} room types");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while retrieving room types");
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = CJConstant.SOMETHING_WENT_WRONG;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving room types");
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = "An unexpected error occurred";
            }

            return serviceResponse;
        }
    }
}
