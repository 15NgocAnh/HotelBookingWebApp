using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IDynamicPricingService
    {
        Task<ServiceResponse<List<DynamicPricingDTO>>> GetAllAsync();
        Task<ServiceResponse<DynamicPricingDTO>> GetByIdAsync(int id);
        Task<ServiceResponse<DynamicPricingDTO>> CreateAsync(CreateDynamicPricingDTO createDynamicPricingDTO);
        Task<ServiceResponse<DynamicPricingDTO>> UpdateAsync(int id, UpdateDynamicPricingDTO updateDynamicPricingDTO);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<List<DynamicPricingDTO>>> GetByRoomTypeIdAsync(int roomTypeId);
        Task<ServiceResponse<List<DynamicPricingDTO>>> GetActiveRulesForDateAsync(DateTime date);
        Task<ServiceResponse<List<DynamicPricingDTO>>> GetActiveRulesForDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 