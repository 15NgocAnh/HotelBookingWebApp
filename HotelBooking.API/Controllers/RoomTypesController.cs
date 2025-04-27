using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using HotelBooking.Domain.DTOs.RoomType;
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
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypesController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var serviceResponse = await _roomTypeService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var serviceResponse = await _roomTypeService.GetByIdAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRoomTypeDTO createRoomTypeDTO)
        {
            var serviceResponse = await _roomTypeService.CreateAsync(createRoomTypeDTO);
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return CreatedAtAction(nameof(GetById), new { id = serviceResponse.Data.Id }, serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateRoomTypeDTO updateRoomTypeDTO)
        {
            var serviceResponse = await _roomTypeService.UpdateAsync(id, updateRoomTypeDTO);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var serviceResponse = await _roomTypeService.DeleteAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }
    }
} 