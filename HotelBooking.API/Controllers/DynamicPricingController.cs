using System;
using System.Threading.Tasks;
using Asp.Versioning;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]/")]
    public class DynamicPricingController : ControllerBase
    {
        private readonly IDynamicPricingService _dynamicPricingService;

        public DynamicPricingController(IDynamicPricingService dynamicPricingService)
        {
            _dynamicPricingService = dynamicPricingService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var serviceResponse = await _dynamicPricingService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var serviceResponse = await _dynamicPricingService.GetByIdAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("roomtype/{roomTypeId}")]
        public async Task<ActionResult> GetByRoomTypeId(int roomTypeId)
        {
            var serviceResponse = await _dynamicPricingService.GetByRoomTypeIdAsync(roomTypeId);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult> GetActiveRulesForDate(DateTime date)
        {
            var serviceResponse = await _dynamicPricingService.GetActiveRulesForDateAsync(date);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("daterange")]
        public async Task<ActionResult> GetActiveRulesForDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var serviceResponse = await _dynamicPricingService.GetActiveRulesForDateRangeAsync(startDate, endDate);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDynamicPricingDTO createDynamicPricingDTO)
        {
            var serviceResponse = await _dynamicPricingService.CreateAsync(createDynamicPricingDTO);
            if (serviceResponse.ResponseType == EResponseType.BadRequest)
                return BadRequest(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateDynamicPricingDTO updateDynamicPricingDTO)
        {
            var serviceResponse = await _dynamicPricingService.UpdateAsync(id, updateDynamicPricingDTO);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.BadRequest)
                return BadRequest(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var serviceResponse = await _dynamicPricingService.DeleteAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }
    }
} 