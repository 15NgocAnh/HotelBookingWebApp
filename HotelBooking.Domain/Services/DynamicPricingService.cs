using AutoMapper;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Infrastructure.Services
{
    public class DynamicPricingService : IDynamicPricingService
    {
        private readonly IDynamicPricingRepository _dynamicPricingRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IExtraChargesRepository _extraChargesRepository;
        private readonly IMapper _mapper;

        public DynamicPricingService(
            IDynamicPricingRepository dynamicPricingRepository,
            IRoomTypeRepository roomTypeRepository,
            IExtraChargesRepository extraChargesRepository,
            IMapper mapper)
        {
            _dynamicPricingRepository = dynamicPricingRepository;
            _roomTypeRepository = roomTypeRepository;
            _extraChargesRepository = extraChargesRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<DynamicPricingDTO>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<DynamicPricingDTO>>();
            var rules = await _dynamicPricingRepository.GetAllAsync();

            if (rules != null && rules.Any())
            {
                serviceResponse.Data = _mapper.Map<List<DynamicPricingDTO>>(rules);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get all dynamic pricing rules successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No dynamic pricing rules found.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<DynamicPricingDTO>> GetByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<DynamicPricingDTO>(); 
            try
            {
                var rule = await _dynamicPricingRepository.GetByPricingIdAsync(id);

                if (rule == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Dynamic pricing rule not found.";
                    return serviceResponse;
                }

                serviceResponse.Data = _mapper.Map<DynamicPricingDTO>(rule);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get dynamic pricing rule successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<DynamicPricingDTO>> CreateAsync(CreateDynamicPricingDTO createDynamicPricingDTO)
        {
            var serviceResponse = new ServiceResponse<DynamicPricingDTO>();
            try
            {
                // Validate room type exists
                var roomType = await _roomTypeRepository.GetByIdAsync(createDynamicPricingDTO.RoomTypeId);
                if (roomType == null)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Room type not found.";
                    return serviceResponse;
                }

                var rule = _mapper.Map<DynamicPricingRule>(createDynamicPricingDTO);

                // Xử lý HourlyPrices
                if (createDynamicPricingDTO.HourlyPrices != null)
                {
                    rule.HourlyPrices = createDynamicPricingDTO.HourlyPrices.Select(hp => new HourlyPrice
                    {
                        HourSetting = hp.HourSetting,
                        Price = hp.Price
                    }).ToList();
                }
                // Xử lý ExtraCharges
                if (createDynamicPricingDTO.ExtraCharges != null)
                {
                    rule.ExtraCharges = createDynamicPricingDTO.ExtraCharges.Select(ec => new ExtraCharge
                    {
                        ChargeType = ec.ChargeType,
                        HourSetting = ec.HourSetting,
                        Price = ec.Price
                    }).ToList();
                }

                await _dynamicPricingRepository.AddAsync(rule);
                serviceResponse.Data = _mapper.Map<DynamicPricingDTO>(rule);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Create dynamic pricing rule successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<DynamicPricingDTO>> UpdateAsync(int id, UpdateDynamicPricingDTO updateDynamicPricingDTO)
        {
            var serviceResponse = new ServiceResponse<DynamicPricingDTO>();
            var rule = await _dynamicPricingRepository.GetByIdAsync(id);

            if (rule == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Dynamic pricing rule not found.";
                return serviceResponse;
            }

            try
            {
                // Validate room type exists
                var roomType = await _roomTypeRepository.GetByIdAsync(updateDynamicPricingDTO.RoomTypeId);
                if (roomType == null)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Room type not found.";
                    return serviceResponse;
                }

                _mapper.Map(updateDynamicPricingDTO, rule);

                // Xử lý ExtraCharges
                await _extraChargesRepository.DeleteAllByDynamicPricingIdAsync(updateDynamicPricingDTO.Id);

                await _dynamicPricingRepository.UpdateAsync(rule);

                serviceResponse.Data = _mapper.Map<DynamicPricingDTO>(rule);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Update dynamic pricing rule successfully!";
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
                var rule = await _dynamicPricingRepository.GetByIdAsync(id);
                if (rule == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Dynamic pricing rule not found.";
                    return serviceResponse;
                }

                await _dynamicPricingRepository.SoftDeleteAsync(rule);
                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Delete dynamic pricing rule successfully!";
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<DynamicPricingDTO>>> GetByRoomTypeIdAsync(int roomTypeId)
        {
            var serviceResponse = new ServiceResponse<List<DynamicPricingDTO>>();
            var rules = await _dynamicPricingRepository.GetByRoomTypeIdAsync(roomTypeId);

            if (rules != null && rules.Any())
            {
                serviceResponse.Data = _mapper.Map<List<DynamicPricingDTO>>(rules);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get dynamic pricing rules by room type successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No dynamic pricing rules found for this room type.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<DynamicPricingDTO>>> GetActiveRulesForDateAsync(DateTime date)
        {
            var serviceResponse = new ServiceResponse<List<DynamicPricingDTO>>();
            var rules = await _dynamicPricingRepository.GetActiveRulesForDateAsync(date);

            if (rules != null && rules.Any())
            {
                serviceResponse.Data = _mapper.Map<List<DynamicPricingDTO>>(rules);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get active dynamic pricing rules for date successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No active dynamic pricing rules found for this date.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<DynamicPricingDTO>>> GetActiveRulesForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = new ServiceResponse<List<DynamicPricingDTO>>();
            var rules = await _dynamicPricingRepository.GetActiveRulesForDateRangeAsync(startDate, endDate);

            if (rules != null && rules.Any())
            {
                serviceResponse.Data = _mapper.Map<List<DynamicPricingDTO>>(rules);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get active dynamic pricing rules for date range successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No active dynamic pricing rules found for this date range.";
            }
            return serviceResponse;
        }
    }
}