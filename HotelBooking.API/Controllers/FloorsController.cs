using Asp.Versioning;
using HotelBooking.Domain.DTOs.Floor;
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
    public class FloorsController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorsController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var serviceResponse = await _floorService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var serviceResponse = await _floorService.GetByIdAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateFloorDTO createFloorDTO)
        {
            var serviceResponse = await _floorService.CreateAsync(createFloorDTO);
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return CreatedAtAction(nameof(GetById), new { id = serviceResponse.Data.Id }, serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateFloorDTO updateFloorDTO)
        {
            var serviceResponse = await _floorService.UpdateAsync(id, updateFloorDTO);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var serviceResponse = await _floorService.DeleteAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }
    }
} 