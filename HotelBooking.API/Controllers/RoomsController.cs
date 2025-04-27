using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using HotelBooking.Domain.DTOs.Room;
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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var serviceResponse = await _roomService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var serviceResponse = await _roomService.GetByIdAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("floor/{floorId}")]
        public async Task<ActionResult> GetByFloorId(int floorId)
        {
            var serviceResponse = await _roomService.GetByFloorIdAsync(floorId);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("type/{roomTypeId}")]
        public async Task<ActionResult> GetByRoomTypeId(int roomTypeId)
        {
            var serviceResponse = await _roomService.GetByRoomTypeIdAsync(roomTypeId);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRoomDTO createRoomDTO)
        {
            var serviceResponse = await _roomService.CreateAsync(createRoomDTO);
            if (serviceResponse.ResponseType == EResponseType.BadRequest)
                return BadRequest(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return CreatedAtAction(nameof(GetById), new { id = serviceResponse.Data.Id }, serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateRoomDTO updateRoomDTO)
        {
            var serviceResponse = await _roomService.UpdateAsync(id, updateRoomDTO);
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
            var serviceResponse = await _roomService.DeleteAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }
    }
}
